using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void PlayGame()
    {
        // Makes sure HaungsMode is off
        PlayerPrefs.SetInt("HaungsMode", 0);
        PlayerPrefs.Save();
        // Load the first scene in the build order after this one
        SceneManager.LoadScene("Tutorial2", LoadSceneMode.Single);
    }
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void HaungsMode()
    {
        // Turn on HaungsMode invincibility 
        PlayerPrefs.SetInt("HaungsMode", 1);
        PlayerPrefs.Save();
        // Load the first scene in the build order after this one
        SceneManager.LoadScene("Tutorial2", LoadSceneMode.Single);
    }
}
