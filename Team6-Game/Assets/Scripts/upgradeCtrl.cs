using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class upgradeCtrl : MonoBehaviour
{
    public Text upgradeTxt;

    public Button shotgun;
    public Button homing;
    public Button machinegun;
    public Button chargegun;
    public Button bulletdmg;
    public Button firerate;
    private int levelNumber; // PlayerPrefs

    int uPoints;

    // Start is called before the first frame update
    void Start()
    {
        uPoints = 2;
        upgradeTxt.text = "Upgrade Points: " + uPoints;
        levelNumber = PlayerPrefs.GetInt("LevelNumber");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spendPoint()
    {
        if (uPoints > 0)
        {
            uPoints -= 1;
            upgradeTxt.text = "Upgrade Points: " + uPoints;
        }

        // If player has 0 points disable all upgrade buttons
        if (uPoints == 0)
        {
            shotgun.interactable = false;
            homing.interactable = false;
            machinegun.interactable = false;
            chargegun.interactable = false;
            bulletdmg.interactable = false;
            firerate.interactable = false;
        }
    }

    public void loadNextLevel()
    {
        // --Trevor-- checks previous level number, stored in PlayerPrefs, and loads the next level accordingly
        switch (levelNumber)
        {
            case 1:
                SceneManager.LoadScene("Level2Demo", LoadSceneMode.Single);
                break;
            //case 2:
                //SceneManager.LoadScene(" -- Put Level 3 Name Here-- ", LoadSceneMode.Single);
                //break;
            //case 3:
                //SceneManager.LoadScene(" -- Put Level 4 Name Here-- ", LoadSceneMode.Single);
                //break;
                //  .
                //  .
                //  .
            default:
                Debug.Log("This didnt work");
                break;
        }

        Debug.Log("Load next level button");
    }
}
