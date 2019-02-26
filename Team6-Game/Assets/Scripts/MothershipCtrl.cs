using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MothershipCtrl : MonoBehaviour
{
    public float mothershipLight;

    // Start is called before the first frame update
    void Start()
    {
        // Start with no light
        mothershipLight = 0.0f;

    }

    // Update is called once per frame
    void Update()
    {
        
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
