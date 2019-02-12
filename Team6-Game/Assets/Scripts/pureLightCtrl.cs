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
}
