using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pureLightCtrl : MonoBehaviour
{
    int minValue = 10;
    int maxValue = 100;
    public int currentValue;
    float maxSize = 7.0f;

    // Start is called before the first frame update
    void Awake()
    {
        // Generate a pure light value 
        currentValue = Random.Range(minValue, maxValue);
        float curScale = maxSize * (Mathf.Round(currentValue) / Mathf.Round(maxValue)); // if you do int division this wont work, use round to get floats
        // Set the scale of the light relative to the 
        transform.localScale = new Vector3(curScale, curScale, curScale);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            print("Hit Player\n");
            this.gameObject.SetActive(false); // Remove light from game
            PlayerCtrl playerscript = other.gameObject.GetComponent<PlayerCtrl>(); // get a reference to the light u collide with
            playerscript.lightCount += currentValue; // Add the pure light's current value
            playerscript.updateLightText();
        }
    }
}
