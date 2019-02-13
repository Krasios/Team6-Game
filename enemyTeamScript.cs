using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyTeamScript : MonoBehaviour
{
    public float s;             //Floating point variable to store the enemy's max speed limit.
    public GameObject player;
    private Rigidbody2D rb2d;       //Store a reference to the Rigidbody2D component required to use 2D Physics.
    private Rigidbody2D prb;

    // Use this for initialization
    void Start()
    {
        //Get and store a reference to the Rigidbody2D component so that we can access it.
        rb2d = GetComponent<Rigidbody2D>();
        prb = player.GetComponent<Rigidbody2D>();
    }

    //FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    void FixedUpdate()
    {
        //Store the current horizontal input in the float moveHorizontal.
        float moveHorizontal = 0.25f * (prb.position.x - rb2d.position.x);
        //Store the current vertical input in the float moveVertical.
        float moveVertical = 0.25f * (prb.position.y - rb2d.position.y);
        //Use the two store floats to create a new Vector2 variable movement.
        float speed = Random.Range(1, s);
        //sharp turns
        if ((moveHorizontal * moveVertical >= 0 && rb2d.velocity.x * rb2d.velocity.y <= 0) || (moveHorizontal * moveVertical <= 0 && rb2d.velocity.x * rb2d.velocity.y >= 0))
        {
            rb2d.AddForce(new Vector2(Random.Range(50, 100) * moveHorizontal * speed, Random.Range(50, 100) * moveVertical * speed));
        }
        //normal acceleration
        else
        {
            rb2d.AddForce(new Vector2(moveHorizontal * speed, moveVertical * speed));
        }

        //max velocity
        if (rb2d.velocity.x > 2.6f * s)
        {
            rb2d.velocity = new Vector2(2.6f * s, rb2d.velocity.y);
        }
        if (rb2d.velocity.x < -2.6f * s)
        {
            rb2d.velocity = new Vector2(-2.6f * s, rb2d.velocity.y);
        }
        if (rb2d.velocity.y > 2.6f * s)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, 2.6f * s);
        }
        if (rb2d.velocity.y < -2.6f * s)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, -2.6f * s);
        }
    }
}
