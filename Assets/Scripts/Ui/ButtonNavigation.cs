using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonNavigation : MonoBehaviour
{    
    public static ButtonNavigation Instance { get; private set; }
    public GameObject mainMenu; 
    public GameObject optionsMenu; 
    public GameObject gameOverScreen;
    [SerializeField]
    [Tooltip("The default button when this menu is open")]
    public GameObject defaultMainMenuButton; 
    [SerializeField]
    [Tooltip("The default button when this menu is open")]
    public GameObject defaultOptionsButton; 


   
    private void Awake()
    {
        if (Instance == null)
        
            Instance = this;
        else
            Destroy(gameObject);
    }
    public void OpenOptions()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
        
        
        EventSystem.current.SetSelectedGameObject(null); // Remove current selection
        EventSystem.current.SetSelectedGameObject(defaultOptionsButton);
    }

  
    public void CloseOptions()
    {
        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);
        
        // Set the default button selected in the main menu
        EventSystem.current.SetSelectedGameObject(null); // Remove current selection
        EventSystem.current.SetSelectedGameObject(defaultMainMenuButton);
    }

}
