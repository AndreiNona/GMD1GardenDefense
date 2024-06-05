using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    RoundInactive,
    RoundActive,
    GameOver
}
public class GameManager : MonoBehaviour
{
    
    //Singleton 
    public static GameManager Instance;
    
    //Not used
    [SerializeField] [Tooltip("Main objective")]
    private GameObject treeOfLife;
    
    [SerializeField] [Tooltip("Delay (seconds) between tutorial prompts")]
    private int tutorialDelay=5;

    
    [SerializeField] [Tooltip("Enemy spawners")]
    private GameObject[] spawners; // Assign the 3 spawner objects with SpawnEnemy components
    [SerializeField] [Tooltip("Collectable spawner object")]
    private GameObject collectableSpawner;
    
    [SerializeField] [Tooltip("Starting round")]
    private int roundNumber = 1;
    
    [SerializeField] [Tooltip("The seeds gained as a base at the end of each round")]
    private int baseSeeds = 0;

    //UI
    private UiDisplayInfo uiDisplayInfo;
    
    //Round reward
    private int seeds = 10;
    private bool roundActive = false;
    
    //Time
    public float checkInterval = 5.0f; // Time in seconds between checks
    private float timer;
    
    //Statistics
    private String _looseReason;
    private int _robotsHealed;
    private int _treesPlanted;
    private int _seedsGathered;
    private int _hitsTaken;
    private int _cogsFired;
    private int _structuresLost;
    
    public GameState CurrentState { get; private set; } = GameState.RoundInactive;
    
    
    public bool IsRoundActive { get; private set; }
    
    
       private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        uiDisplayInfo = UiDisplayInfo.Instance;
        uiDisplayInfo.UpdateSeeds(seeds);
    }

    private void Start()
    {
        StartCoroutine(Tutorial());
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= checkInterval)
        {
            CheckGame();
            timer = 0;
        }
    }
    private IEnumerator Tutorial()
    {
        while (CurrentState != GameState.GameOver)
        {
            uiDisplayInfo.UpdateTutorial(CurrentState == GameState.RoundActive);
            yield return new WaitForSeconds(tutorialDelay);
        }
    }
    // ReSharper disable Unity.PerformanceAnalysis
    private void CheckGame()
    {
        
        int enemiesAlive = GameObject.FindGameObjectsWithTag("Enemy").Length;
        Debug.Log($"Checking game state... Enemies alive: {enemiesAlive}");
        if (CurrentState == GameState.RoundActive && enemiesAlive < 2)
            EndRound();
        else
            uiDisplayInfo.UpdateEnemies(enemiesAlive);

        GameObject treeOfLife = GameObject.Find("Tree of Life");
        GameObject player = GameObject.Find("Player_C");

        if ((treeOfLife == null || player == null) && CurrentState != GameState.GameOver)
        {
            string missingObjects = "";
            if (treeOfLife == null)
            {
                _looseReason = "'Tree of Life' has been destroyed ";
                missingObjects += "'Tree of Life' not found! ";
            }
            else
            {
                missingObjects += "Player 'Ruby_0' not found! ";
                _looseReason = "You died!";
            }
            
            Debug.Log($"Game Over: {missingObjects.Trim()}");
            CurrentState = GameState.GameOver;
            var endGameController = FindObjectOfType<EndGameMenuController>();
            endGameController.DisplayEndGameStats(_looseReason,_robotsHealed, _treesPlanted, _seedsGathered, _hitsTaken, _cogsFired, _structuresLost);
            //MainMenu.MainScreen();
        }
    }


    private void StartRound()
    {
        if (CurrentState == GameState.RoundInactive) 
        {
            CurrentState = GameState.RoundActive;
            int enemiesToSpawn = CalculateEnemiesToSpawn(roundNumber);
            float enemyHealth = CalculateEnemyHealth(roundNumber);

            Debug.Log($"Starting Round {roundNumber} with {enemiesToSpawn} enemies, each having {enemyHealth} health.");

            foreach (GameObject spawner in spawners)
            {
                SpawnEnemy spawnEnemy = spawner.GetComponent<SpawnEnemy>();
                if (spawnEnemy != null)
                    spawnEnemy.StartSpawning(enemiesToSpawn);
            }

            uiDisplayInfo.UpdateRound(roundNumber);
        }
        else
            Debug.Log("Attempted to start round while not in RoundInactive state.");
    }

    private void EndRound()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
            Destroy(enemy);
        
        //Manually updates enemies 
        int endOfRoundReward = GameObject.FindGameObjectsWithTag("Chaseable").Length;
        
        StructureReward[] structureRewards = FindObjectsOfType<StructureReward>();
        foreach (StructureReward structureReward in structureRewards)
            if (structureReward.IsRewardable)
                endOfRoundReward += structureReward.EndOfRoundReward;

        if (structureRewards.Length == 0 || !structureRewards.Any(sr => sr.IsRewardable))
            Debug.Log("No structure rewards available or none are rewardable.");
        
        CurrentState = GameState.RoundInactive;
        roundNumber++;
        SpawnCollectables();

        uiDisplayInfo.UpdateEnemies(0);
        uiDisplayInfo.UpdateRound(roundNumber, true);
        AddSeeds(endOfRoundReward+ baseSeeds);

        Debug.Log($"Round ended. Total score: {seeds}, with additional rewards from structures.");
    }
    
    private void AddSeeds(int number)
    {
        seeds = baseSeeds + number;
        _seedsGathered +=number;
        uiDisplayInfo.UpdateSeeds(seeds);
    }

    public bool TryRemoveSeeds(int amount)
    {
        if (seeds >= amount)
        {
            seeds -= amount;
            IncrementTreesPlanted(); //Change if other ways of spending seeds are added
            uiDisplayInfo.UpdateSeeds(seeds);
            return true;
        }
        return false;
    }

    private void SpawnCollectables()
    {
        collectableSpawner.GetComponent<CollectableSpawn>().SpawnCollectables();
    }
    private int CalculateEnemiesToSpawn(int roundNumber)
    {
   
        //Add 2 more each round
        return 12 + 2 * (roundNumber - 1);
    }

    private float CalculateEnemyHealth(int roundNumber)
    {
        // Increase by 10% each round
        return 10 * Mathf.Pow(1.1f, roundNumber - 1);
    }
    public void IncrementRobotsHealed()
    {
        _robotsHealed++;
        Debug.Log("Robots Healed: " + _robotsHealed);
    }
    public void IncrementTreesPlanted()
    {
        _treesPlanted++;
    }
    public void IncrementHitsTaken()
    {
        _hitsTaken++;
    }
    public void IncrementCogsFired()
    {
        _cogsFired++;
    }
    public void IncrementTreesLost()
    {
        _structuresLost++;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !IsRoundActive)
        {
            Debug.Log("Round started by: " + other.gameObject.name);
            StartRound();
        }
    }
}
