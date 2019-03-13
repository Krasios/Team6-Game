using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class upgradeCtrl : MonoBehaviour
{
    public Text upgradeTxt;

    public Button shotgun;
    public Button homing;
    public Button machinegun;
    public Button chargegun;
    public Button bulletdmg;
    public Button firerate;

    int uPoints;

    // Start is called before the first frame update
    void Start()
    {
        uPoints = 2;
        upgradeTxt.text = "Upgrade Points: " + uPoints;

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
        Debug.Log("Load next level button");
    }
}
