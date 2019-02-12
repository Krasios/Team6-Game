using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Connor
public class shipSpotCtrl : MonoBehaviour
{
    // Reference a player's ship
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Set the lights position to the ships
        transform.position = player.transform.position;
        // Update the Rotation of the light, not as simple as it seems
    }
}
