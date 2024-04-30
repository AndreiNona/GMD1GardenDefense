using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonNavigation : MonoBehaviour
{    
    
    public GameObject mainMenu; 
    public GameObject optionsMenu; 
    [SerializeField]
    [Tooltip("The default button when this menu is open")]
    public GameObject defaultMainMenuButton; 
    [SerializeField]
    [Tooltip("The default button when this menu is open")]
    public GameObject defaultOptionsButton; 

   
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
