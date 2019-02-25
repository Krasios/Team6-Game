using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class tutorialCtrl : MonoBehaviour
{
    public Text lightText;
    public static tutorialCtrl instance;
    private int lightLeft = 10;
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
        lightText.text = "Gather "+lightLeft+" light sources";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void lightGathered() {
        lightLeft -=1;
        if (lightLeft > 0){
            lightText.text = "Gather "+lightLeft+" light sources";
        }else {
            SceneManager.LoadScene("RopeTest", LoadSceneMode.Single);
        }
    }
}
