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
    public static GameManager Instance;
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

    private UiDisplayInfo uiDisplayInfo;
    
    private int seeds = 10;
    private bool roundActive = false;
    
    public float checkInterval = 5.0f; // Time in seconds between checks
    private float timer;
    
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
        {
            Destroy(enemy);
        }
        
        int endOfRoundReward = GameObject.FindGameObjectsWithTag("Chaseable").Length;
        
        StructureReward[] structureRewards = FindObjectsOfType<StructureReward>();
        foreach (StructureReward structureReward in structureRewards)
        {
            if (structureReward.IsRewardable)
            {
                endOfRoundReward += structureReward.EndOfRoundReward;
            }
        }

        // If there are no StructureReward components or none are rewardable, log this information
        if (structureRewards.Length == 0 || !structureRewards.Any(sr => sr.IsRewardable))
            Debug.Log("No structure rewards available or none are rewardable.");
        

        AddSeeds(endOfRoundReward+ baseSeeds);
        CurrentState = GameState.RoundInactive;
        roundNumber++;
        SpawnCollectables();
        Debug.Log($"Round ended. Total score: {seeds}, with additional rewards from structures.");
    }

    private void CheckGame()
    {
        int enemiesAlive = GameObject.FindGameObjectsWithTag("Enemy").Length;
        Debug.Log($"Checking game state... Enemies alive: {enemiesAlive}");
        if (CurrentState == GameState.RoundActive && enemiesAlive < 2)
            EndRound();
        else
            uiDisplayInfo.UpdateEnemies(enemiesAlive);

        GameObject treeOfLife = GameObject.Find("Tree of Life");
        GameObject player = GameObject.Find("Ruby_0");

        if (treeOfLife == null || player == null)
        {
            string missingObjects = "";
            if (treeOfLife == null)
                missingObjects += "'Tree of Life' not found! ";
            if (player == null)
                missingObjects += "Player 'Ruby_0' not found! ";

            Debug.Log($"Game Over: {missingObjects.Trim()}");
            CurrentState = GameState.GameOver;
            MainMenu.MainScreen();
        }
    }

    private void AddSeeds(int number)
    {
        seeds = baseSeeds + number;
        uiDisplayInfo.UpdateSeeds(seeds);
    }

    public bool TryRemoveSeeds(int amount)
    {
        if (seeds >= amount)
        {
            seeds -= amount;
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
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !IsRoundActive)
        {
            Debug.Log("Round started by: " + other.gameObject.name);
            StartRound();
        }
    }
}
