using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class upgradeCtrl : MonoBehaviour
{
    // Connor and Trevor for all code here
    public Text upgradeTxt;

    // Shop buttons
    public Button shotgun;
    public Button homing;
    public Button machinegun;
    public Button chargegun;
    public Button firerate;

    // Equip buttons
    public Button Eqshotgun, Eqhoming, Eqmachinegun, Eqchargegun;

    private int levelNumber; // PlayerPrefs

    int uPoints;

    // Connor, Start is called before the first frame update
    void Start()
    {
        uPoints = 2;

        // If the upgrade text is not null
        if (upgradeTxt)
        {
            upgradeTxt.text = "Shop Points: " + uPoints;
        }
        
        levelNumber = PlayerPrefs.GetInt("LevelNumber");

    }

    // Connor
    void Update()
    {
        // Shop scene, stop players from buying gear they already have
        if ((shotgun) != null && PlayerPrefs.GetFloat("ShotgunStrength") > 0)
        {
            shotgun.interactable = false;
        }

        if ((homing) != null && PlayerPrefs.GetFloat("HomingStrength") > 0)
        {
            homing.interactable = false;
        }

        if ((machinegun) != null && PlayerPrefs.GetFloat("MachineStrength") > 0)
        {
            machinegun.interactable = false;
        }

        if ((chargegun) != null && PlayerPrefs.GetFloat("ChargeStrength") > 0)
        {
            chargegun.interactable = false;
        }

        if ((firerate) != null && (PlayerPrefs.GetFloat("newFireRatePref") <= 0.2f))
        {
            firerate.interactable = false;
        }

        // Equip Scene, Check if the gun hasnt been bought and set it to unclickable
        if ((Eqshotgun) != null && PlayerPrefs.GetFloat("ShotgunStrength") == 0)
        {
            Eqshotgun.interactable = false;
        }

        if ((Eqhoming) != null && PlayerPrefs.GetFloat("HomingStrength") == 0)
        {
            Eqhoming.interactable = false;
        }

        if ((Eqmachinegun) != null && PlayerPrefs.GetFloat("MachineStrength") == 0)
        {
            Eqmachinegun.interactable = false;
        }

        if ((Eqchargegun) != null && PlayerPrefs.GetFloat("ChargeStrength") == 0)
        {
            Eqchargegun.interactable = false;
        }
    }

    // Connor, Let the player spend upgrade points and disable buttons when out of upoints
    public void spendPoint()
    {
        if (uPoints > 0)
        {
            uPoints -= 1;
            upgradeTxt.text = "Shop Points: " + uPoints;
        }

        // If player has 0 points disable all upgrade buttons
        if (uPoints == 0)
        {
            shotgun.interactable = false;
            homing.interactable = false;
            machinegun.interactable = false;
            chargegun.interactable = false;
            firerate.interactable = false;
        }
    }

    // Connor, Load the equip level before the next battle level
    public void loadEquipScene()
    {
        SceneManager.LoadScene("LevelEquipShip", LoadSceneMode.Single);
    }

    // Connor and Trevor, Load the next battle level
    public void loadNextLevel()
    {
        // --Trevor-- checks previous level number, stored in PlayerPrefs, and loads the next level accordingly
        switch (levelNumber) //was levelNumber
        {
            case 1:
                SceneManager.LoadScene("Level2Demo", LoadSceneMode.Single);
                break;
            case 2:
                SceneManager.LoadScene("BonusLevel", LoadSceneMode.Single);
                break;
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
