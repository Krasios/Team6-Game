using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class tutorialCtrl : MonoBehaviour
{
    public Text tutorialText;
    public static tutorialCtrl instance;
    private int lightLeft = 10;
    private int asteroidsLeft = 5;
    private int enemiesLeft = 10;
    GenerateOnce[] obstacleGenerators;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null) {
            instance = this;
        }else{
            Destroy(gameObject);
        }
    }
    void Start()
    {
        obstacleGenerators = GetComponents<GenerateOnce>();
        obstacleGenerators[0].enabled = true;
        tutorialText.text = "Gather "+lightLeft+" light sources";
    }

    // Update is called once per frame
    void Update()
    {
        if (lightLeft > 0){
            tutorialText.text = "Gather "+lightLeft+" light sources";
        }else if (asteroidsLeft > 0){
            tutorialText.text = "Destroy "+asteroidsLeft+" asteroids with your shield";
        }else if (enemiesLeft > 0){
            tutorialText.text = "Destroy some alien ships";
        }else{
            tutorialText.text = "Good job";
            SceneSwitch();
        }        
    }
    public void lightGathered() {
        lightLeft -=1;
        if (lightLeft <= 0){
            obstacleGenerators[1].enabled = true;
        }
    }
    public void asteroidZapped() {
        asteroidsLeft-=1;
        if (asteroidsLeft <=0){
            enemyShot();
            obstacleGenerators[2].enabled = true;
        }
    }
    public void enemyShot(){
        enemiesLeft-=1;
    }

    void SceneSwitch()
    {
        SceneManager.LoadScene("RopeTest", LoadSceneMode.Single);
    }
}
