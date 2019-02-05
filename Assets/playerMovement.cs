using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public float speed;             //Floating point variable to store the player's movement speed.

    private Rigidbody2D rb2d;       //Store a reference to the Rigidbody2D component required to use 2D Physics.

    // Use this for initialization
    void Start()
    {
        //Get and store a reference to the Rigidbody2D component so that we can access it.
        rb2d = GetComponent<Rigidbody2D>();
    }

    //FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    void FixedUpdate()
    {
        //Store the current horizontal input in the float moveHorizontal.
        float moveHorizontal = Input.GetAxis("Horizontal");

        //Store the current vertical input in the float moveVertical.
        float moveVertical = Input.GetAxis("Vertical");

        //Use the two store floats to create a new Vector2 variable movement.
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        //Call the AddForce function of our Rigidbody2D rb2d supplying movement multiplied by speed to move our player.
        if (moveHorizontal * moveVertical >= 0 && rb2d.velocity.x*rb2d.velocity.y <= 0)
        {
            rb2d.AddForce(10*movement * speed);
        } else if(moveHorizontal * moveVertical <= 0 && rb2d.velocity.x * rb2d.velocity.y >= 0)
        {
            rb2d.AddForce(10 * movement * speed);
        }
        else
        {
            rb2d.AddForce(movement * speed);
        }
        //max velocity
        if (rb2d.velocity.x > 50)
        {
            rb2d.velocity = new Vector2(50, rb2d.velocity.y);
        }
        if (rb2d.velocity.x < -50)
        {
            rb2d.velocity = new Vector2(-50, rb2d.velocity.y);
        }
        if (rb2d.velocity.y > 50)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x,50);
        }
        if (rb2d.velocity.y < -50)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x,-50);
        }
    }
}
