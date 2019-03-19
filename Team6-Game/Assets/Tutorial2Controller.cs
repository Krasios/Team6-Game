using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// New tutorial based on feeback --Mostly Trevor and a little bit from Connor--
public class Tutorial2Controller : MonoBehaviour
{
    public Text tutorial;
    public GameObject tutorialLight;
    public GameObject tutorialAsteroids;
    public GameObject marker;
    public GameObject icon;
    public GameObject motherLightBar;
    public GameObject player;
    public GameObject mothership;
    public GameObject enemies;
    public float lightCount;
    public int state = 0;
    bool completedStage = false;

    private float distanceBetween;


    private void Start()
    {
        StartCoroutine(InstructionTimer());
        player.GetComponent<PlayerCtrl>();
        PlayerPrefs.SetInt("PrimaryGun", 0);
        PlayerPrefs.SetInt("SpecialGun", 0);
    }
    // Update is called once per frame
    void Update()
    {
        lightCount = player.GetComponent<PlayerCtrl>().lightCount;
        StageCheck();

        distanceBetween = (mothership.transform.position - player.transform.position).magnitude;
        //Debug.Log(distanceBetween);
        if (distanceBetween <= 1000)
        {
            state = 5;
            TutorialUpdate();
        }

        if ((player.GetComponent<PlayerCtrl>().currentHealth > 50) && state == 5)
        {
            state = 6;
            TutorialUpdate();
        }

    }


    void TutorialUpdate()
    {
        switch (state)
        {
            case 0:
                break;
            case 1:
                tutorial.text = "First, lets try the thrusters. You can move your ship with the W/A/S/D keys. Try picking up some pure light nearby.";
                player.GetComponent<PlayerCtrl>().joystick = false;
                tutorialLight.SetActive(true);
                completedStage = false;
                break;
            case 2:
                tutorial.text = "Good, as you collect more pure light, your light meter will fill up.";
                completedStage = false;
                StartCoroutine(InstructionTimerLong());
                break;
            case 3:
                player.transform.position = new Vector2(1000, 1000);
                player.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
                player.GetComponent<PlayerCtrl>().joystick = true;
                player.GetComponent<PlayerCtrl>().canShoot = true;
                // Connor Tweaked this
                tutorial.text = "Your ship will point towards your mouse cursor. Click the left mouse button to fire your left gun and the right mouse button for your right gun.";
                StartCoroutine(BaseTimerOne());
                completedStage = false;
                break;
            case 4:
                player.transform.position = new Vector2(-4000, 0);
                player.GetComponent<PlayerCtrl>().currentHealth = 50;
                tutorial.text = "It looks like you're safe now. If you are injured, you can heal by depositing pure light to the Mothership. Let's go find it!";
                StartCoroutine(BaseTimerTwo());
                completedStage = false;
                break;
            case 5:
                tutorial.text = "You must fill up the Mothership's light bar so that it has enough energy to warp us to another sector.";
                motherLightBar.SetActive(true);
                StartCoroutine(BaseTimerThree());
                completedStage = false;
                break;
            case 6:
                tutorial.text = "Supplying enough pure light to the Mothership is key to our survival. We must rebuild a new sun!";
                StartCoroutine(BaseTimerFour());
                completedStage = false;
                break;
            case 7:
                SceneManager.LoadScene("RopeTest", LoadSceneMode.Single);
                break;


        }
    }

    //Connor, skip tutorial and load 1st level
    public void SkipTutorial()
    {
        SceneManager.LoadScene("RopeTest", LoadSceneMode.Single);
    }

    void StageCheck()
    {
        if (completedStage)
        {
            state += 1;
            TutorialUpdate();
        }

        if (lightCount > 300 && state == 1)
        {
            completedStage = true;
        }



    }

    IEnumerator InstructionTimer()
    {
        yield return new WaitForSeconds(4);
        state ++;
        TutorialUpdate();
    }

    IEnumerator InstructionTimerLong()
    {
        yield return new WaitForSeconds(8);
        state ++;
        TutorialUpdate();
    }

    IEnumerator BaseTimerOne()
    {
        yield return new WaitForSeconds(1);
        player.GetComponent<PlayerCtrl>().joystick = false;
        yield return new WaitForSeconds(6);
        tutorial.text = "Destorying enemies and obstacles leaves behind pure light.";
        yield return new WaitForSeconds(6);
        tutorial.text = "Look out, Enemies Approaching!";
        enemies.SetActive(true);
        yield return new WaitForSeconds(12);
        state ++;
        TutorialUpdate();
    }

    IEnumerator BaseTimerTwo()
    {
        yield return new WaitForSeconds(8);
        tutorial.text = "The compass in the upper right hand corner of your screen will direct you to the Mothership. The color of the arrow will turn to green, the closer you get.";
        marker.SetActive(true);
        icon.SetActive(true);
    }

    IEnumerator BaseTimerThree()
    {
        yield return new WaitForSeconds(7);
        tutorial.text = "" +
            "Supplying enough pure light to the Mothership is key to our survival. We must rebuild a new sun!";
    }

    IEnumerator BaseTimerFour()
    {
        yield return new WaitForSeconds(6);
        state++;
        TutorialUpdate();
    }
}

