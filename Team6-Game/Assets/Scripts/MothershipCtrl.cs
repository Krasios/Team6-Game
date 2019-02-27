using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MothershipCtrl : MonoBehaviour
{
    public float mothershipLight;
    public float mothershipGoal;
    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        // Start with no light
        mothershipLight = 0.0f;
        //starts mothership light slider to the far left
        slider.value = 0;
        //how much light needed to win
        mothershipGoal = 500;
    }

    // Update is called once per frame
    void Update()
    {
        //animates the mothership light stored
        slider.value = Mathf.Lerp(slider.value, mothershipLight / mothershipGoal, 0.05f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.gameObject.GetComponent<PlayerCtrl>().lightCount > 0)
            {
                mothershipLight += other.gameObject.GetComponent<PlayerCtrl>().lightCount;
                Debug.Log("Mothership's Current Light: " + mothershipLight.ToString());
                other.gameObject.GetComponent<PlayerCtrl>().lightCount = 0;
                other.gameObject.GetComponent<PlayerCtrl>().updateLightText();
            }
        }
    }
}
