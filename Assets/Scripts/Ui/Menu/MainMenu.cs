using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{

    public static void MainScreen()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
   public void PlayGame()
   {
      SceneManager.LoadScene(1,  LoadSceneMode.Single);
   }

   public void QuitGame()
   {
      Application.Quit();
   }
}
