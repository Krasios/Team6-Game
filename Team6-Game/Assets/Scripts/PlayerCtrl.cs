using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{
    public float speed = 3.5f; // Increase player movement speed
    public string name;
    public GameObject bulletPrefab;

    private Light playerLight;
    private Rigidbody2D rigidB;


    float xRot;
    float yRot;

    void Start() {
        rigidB = GetComponent<Rigidbody2D>();
        playerLight = GetComponent<Light>();
    }

    void FixedUpdate() {
        //-- Code by Trevor and Connor --//
        // Get inputs from xbox controller
        float moveHorizontal = Input.GetAxis("Joystick-Horizontal-p1"); // Gamepad 1
        float moveVertical = Input.GetAxis("Joystick-Vertical-p1"); // Gamepad 1

        // Get the rotation from the right stick
        xRot = Input.GetAxis("stickXrotion-p1");
        yRot = Input.GetAxis("stickYrotion-p1");

        // Prevent snapping back to neutral position
        if (Mathf.Abs(xRot) > 0.0 || Mathf.Abs(yRot) > 0.0 )
        {
            UpdatePlayerRot();
        }


        // Add force to the player
        rigidB.AddForce(new Vector2(moveHorizontal, moveVertical) * speed);

        // Check to see if the player is moving too fast
        float maxVelX = 50.0f;
        float maxVelY = 50.0f;
        // Check x max velocity
        if (Mathf.Abs(rigidB.velocity.x) > maxVelX)
        {
            // Cap the velocity
            if (rigidB.velocity.x < 0)
            {
                //negative
                rigidB.velocity = new Vector2(-maxVelX, rigidB.velocity.y);
            }
            else
            {
                //positive
                rigidB.velocity = new Vector2(maxVelX, rigidB.velocity.y);
            }
        }
        // Check y max velocity
        if (Mathf.Abs(rigidB.velocity.y) > maxVelY)
        {
            // Cap the velocity
            if (rigidB.velocity.y < 0)
            {
                //negative
                rigidB.velocity = new Vector2(rigidB.velocity.x, -maxVelY);
            }
            else
            {
                //positive
                rigidB.velocity = new Vector2(rigidB.velocity.x, maxVelY);
            }
        }

        if (playerLight.range > 5){
            playerLight.range -= 0.01f;
        }
        if (Input.GetButton("Jump")) {
            Instantiate(bulletPrefab, transform.position, transform.rotation);
        }
    }

    void UpdatePlayerRot()
    {
        float rotSpeed = 15.0f; //  Determine how fast the ship rotates
        //Use arc tan to get radians of in between angle then convert to degrees 
        float playerDirection = Mathf.Atan2(xRot, yRot) * Mathf.Rad2Deg;

        // Convert the degrees into a quaternion around the world's z axis
        Quaternion quat = Quaternion.Euler(0.0f, 0.0f, playerDirection); 

        // Update the rotation of the ship to match the angle of the right stick
        transform.rotation = Quaternion.Slerp(transform.rotation, quat, Time.deltaTime * rotSpeed);
    }

    void OnTriggerEnter2D(Collider2D other) {

    }
}
