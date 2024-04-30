using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiDisplayInfo : MonoBehaviour
{
    private static UiDisplayInfo _instance;
    public static UiDisplayInfo Instance { get { return _instance; } }
    [SerializeField] [Tooltip("UI Round display text")]
    private TMP_Text roundText;
    
    [SerializeField] [Tooltip("UI text displaying build currency")]
    private TMP_Text seedsText;
    [SerializeField] [Tooltip("UI text displaying current enemy count")]
    private TMP_Text enemiesText;
    [SerializeField] [Tooltip("UI text displaying tutorial")]
    private TMP_Text tutorialText;
    
    [SerializeField] [Tooltip("List of tutorial lines for player")]
    private string[] tutorials;
    
    private int _tutorialIndex = 0;
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }
    public void UpdateRound(int roundNumber)
    {
        roundText.text = "Round: " + roundNumber;
    }
    public void UpdateSeeds(int seedCount)
    {
        seedsText.text = "Seeds: " + seedCount;
    }
    public void UpdateEnemies(int enemyCount)
    {
        enemiesText.gameObject.SetActive(enemyCount != 0);
        enemiesText.text = "Enemies left: " + enemyCount;
    }
    public void UpdateTutorial(bool isRoundActive)
    {
        if (!isRoundActive)
        {
            tutorialText.text = tutorials[_tutorialIndex];
            _tutorialIndex = (_tutorialIndex + 1) % tutorials.Length;
        }
        else
            tutorialText.text = "Round is active, please deal with all the enemies!";
    }
}
