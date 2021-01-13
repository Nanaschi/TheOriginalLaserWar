using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] float theDelay = 2.5f;
    //Loads the next scene in comparison to the current one
    //Important Attach to the game object + attach the gameobject to the button On Click() and add function LoadNextScene() 
    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
    public void LoadStartScene()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void LoadGame()
    {
       SceneManager.LoadScene("Game");
        FindObjectOfType<GameSession>().ResetGame();   //Here we destroy the singleton text!
    }
    IEnumerator WaitAndLoad() ///////VERY IMPORTANT COROUTINE DELAY!
    {
        yield return new WaitForSeconds(theDelay);
        SceneManager.LoadScene("Game Over");

      
    }
    public void LoadGameOver()
    {
        StartCoroutine(WaitAndLoad()); //
           }
}
