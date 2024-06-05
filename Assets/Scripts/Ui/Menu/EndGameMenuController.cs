using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class EndGameMenuController : MonoBehaviour
{

    [SerializeField] private PlayerInput _playerInput;
    public KeyCode replayKey = KeyCode.C;
    public KeyCode menuKey = KeyCode.Q;
    public GameObject endGameMenu;
    public TMP_Text looseReasonText;
    public TMP_Text robotsHealedText;
    public TMP_Text treesPlantedText;
    public TMP_Text hitsTakenText;
    public TMP_Text seedsGainedText;
    public TMP_Text cogsFiredText;
    public TMP_Text structuresLostText;
    private void Start()
    {
        endGameMenu.SetActive(false);
    }

    private void Update()
    {
        if (endGameMenu.activeSelf)
        {
            bool buttonX = _playerInput.actions["X"].WasPressedThisFrame();
            bool buttonY = _playerInput.actions["Y"].WasPressedThisFrame();
            if (buttonX)
                MainMenu.MainScreen();
            if (buttonY)
                MainMenu.PlayGame(); 
            if (Input.GetKeyDown(menuKey))
                MainMenu.MainScreen();
            if (Input.GetKeyDown(replayKey))
                MainMenu.PlayGame(); 
        }

    }

    IEnumerator SelectButtonNextFrame(GameObject button)
    {
        yield return null;  // Wait for the next frame
        EventSystem.current.SetSelectedGameObject(button);
    }
    public void DisplayEndGameStats(String looseReason,int robotsHealed, int treesPlanted, int seedsGained,int hitsTaken, int cogsFired, int structuresLost)
    {
        looseReasonText.text = looseReason;
        robotsHealedText.text = "Robots Healed: " + robotsHealed;
        treesPlantedText.text = "Trees Planted: " + treesPlanted;
        seedsGainedText.text = "Seeds Gained: " + seedsGained;
        hitsTakenText.text = "Hits Taken: " + hitsTaken;
        cogsFiredText.text = "Cogs Fired: " + cogsFired;
        structuresLostText.text = "Structures Lost: " + structuresLost;
  
        GameObject defaultGameOverButton = transform.Find("MainMenuButton")?.gameObject;
        try
        {
            StartCoroutine(SelectButtonNextFrame(defaultGameOverButton));
            EventSystem.current.SetSelectedGameObject(defaultGameOverButton);
        }
        catch (Exception e)
        {
            Console.WriteLine("Main Menu button not found!");
            throw;
        }
        
        endGameMenu.SetActive(true);
    }


    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }


    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
