using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MothershipCtrl : MonoBehaviour
{
    public float mothershipLight;
    public int mothershipGoal; // PlayerPrefs
    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        // Start with no light
        mothershipLight = 0.0f;
        //starts mothership light slider to the far left
        slider.value = 0;

        // if on the first level hard set the goal
        if (string.Compare(SceneManager.GetActiveScene().name, "RopeTest") == 0)
        {
            mothershipGoal = 2500;
        }
        else // On otherlevels get the goal from playerPrefs
        {
            mothershipGoal = PlayerPrefs.GetInt("MothershipGoal");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        //animates the mothership light stored
        slider.value = Mathf.Lerp(slider.value, mothershipLight / mothershipGoal, 0.05f);

        if (mothershipLight >= mothershipGoal)
        {
            // Load next scene
            SceneSwitch();
            Debug.Log("Trying to switch scene");
        }
    }
    // Trevor, Added heal mechanic from depositing light to the mothership
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.gameObject.GetComponent<PlayerCtrl>().lightCount > 0)
            {
                float currentPlayerLight = other.gameObject.GetComponent<PlayerCtrl>().lightCount;
                mothershipLight += currentPlayerLight;
                Debug.Log("Mothership's Current Light: " + mothershipLight.ToString());
                // Heal the player based on how much light is deposited to the mothership
                other.gameObject.GetComponent<PlayerCtrl>().currentHealth += currentPlayerLight / 10;
                // Cap the heal to the maxHealth of the player
                if (other.gameObject.GetComponent<PlayerCtrl>().currentHealth >= other.gameObject.GetComponent<PlayerCtrl>().maxHealth)
                {
                    other.gameObject.GetComponent<PlayerCtrl>().currentHealth = other.gameObject.GetComponent<PlayerCtrl>().maxHealth;
                }
                other.gameObject.GetComponent<PlayerCtrl>().lightCount = 0;
                other.gameObject.GetComponent<PlayerCtrl>().updateLightText();
            }
        }
        else
        {
            // Destroy all other objects that collide with the mothership
            other.gameObject.SetActive(false);
            Debug.Log(other.gameObject);
        }
    }

    // Connor and Trevor, Set up mothership when scene switches
    void SceneSwitch()
    {
        if (string.Compare(SceneManager.GetActiveScene().name, "RopeTest") == 0)
        {
            // --Trevor-- Sets level number and next mothership Light Goal, to be called after upgrades
            PlayerPrefs.SetInt("LevelNumber", 1);
            PlayerPrefs.SetInt("MothershipGoal", 3750); // Set light to beat the second level
            PlayerPrefs.Save();
            GoToUpgrades();
        }
        else if (string.Compare(SceneManager.GetActiveScene().name, "Level2Demo") == 0)
        {
            PlayerPrefs.SetInt("LevelNumber", 2);
            PlayerPrefs.SetInt("MothershipGoal", 5000);
            PlayerPrefs.Save();
            GoToUpgrades();
        }
        else if (string.Compare(SceneManager.GetActiveScene().name, "BonusLevel") == 0)
        {
            SceneManager.LoadScene("LevelVictory", LoadSceneMode.Single);
        }
    }

    // Trevor, Go to the upgrade ship scene
    void GoToUpgrades()
    {
        SceneManager.LoadScene("LevelUpgradeShip", LoadSceneMode.Single);
    }
}
