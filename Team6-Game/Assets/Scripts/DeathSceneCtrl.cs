using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathSceneCtrl : MonoBehaviour
{
    // Connor
    
    
    void Update()
    {
        // Check if the player clicked and load the main menu
        if (Input.GetMouseButton(0))
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
    }
}
