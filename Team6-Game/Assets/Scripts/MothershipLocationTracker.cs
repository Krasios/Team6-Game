using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//--Trevor--//
public class MothershipLocationTracker : MonoBehaviour
{
    public GameObject player;
    public GameObject motherShip;
    private Vector3 directionToMother;
    private Color currentColor = Color.green;
    private float currentDistance;
    private float maxDistance = 750.0f;
    public Image arrow;

    private void Start()
    {
        arrow.GetComponent<Image>().color = currentColor;
        currentDistance = directionToMother.magnitude;
        
    }
    // Update is called once per frame
    private void Update()
    {
        RotateUITracker();
        DistanceTracker();
    }

    private void RotateUITracker()
    {
        // Updates the vector pointing towards the mothership
        directionToMother = player.transform.position - motherShip.transform.position;
        // Determine roation speed
        float rotSpeed = 15.0f;
        // Use arc tan to get radians of in between angle then convert to degrees 
        float directionAngle = Mathf.Atan2(directionToMother.x, -directionToMother.y) * Mathf.Rad2Deg;
        // Convert the degrees into a quaternion around the world's z axis
        Quaternion quat = Quaternion.Euler(0.0f, 0.0f, directionAngle);
        // Roate the tracker to point towards the mothership
        transform.rotation = Quaternion.Slerp(transform.rotation, quat, Time.deltaTime * rotSpeed);
    }

    private void DistanceTracker()
    {
        // Gets color from the UI image
        arrow.GetComponent<Image>().color = currentColor;
        // Finds the current distance from the mothership
        currentDistance = directionToMother.magnitude;
        float redDistanceRatio = currentDistance / maxDistance;
        float greenDistandeRatio = 750f / currentDistance;
        if (redDistanceRatio > 1)
        {
            redDistanceRatio = 1;
        }
        // Applies the ratios to the red and green channels depenging on how far you are from the ship using 0.0 - 1.0 for each color R,G,B
        currentColor = new Color(redDistanceRatio, greenDistandeRatio, 0);
    }
}
