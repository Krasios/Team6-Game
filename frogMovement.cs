using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class frogMovement : MonoBehaviour
{
    public float s;             //Floating point variable to store the enemy's max speed limit.
    public GameObject player;
    private Rigidbody2D rb2d;       //Store a reference to the Rigidbody2D component required to use 2D Physics.
    private Rigidbody2D prb;
    private Transform t;
    private int scaler;

    // Use this for initialization
    void Start()
    {
        //Get and store a reference to the Rigidbody2D component so that we can access it.
        rb2d = GetComponent<Rigidbody2D>();
        prb = player.GetComponent<Rigidbody2D>();
        t = GetComponent<Transform>();
        scaler = 100;
    }

    //FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    void FixedUpdate()
    {
        if (Random.Range(1, 100) >= 95)
        {
            moveing();
            scaler = 0;
        }
            rb2d.velocity = 0.9f * rb2d.velocity;
        //max velocity
        if (rb2d.velocity.x > 2.5f * s)
        {
            rb2d.velocity = new Vector2(2.5f * s, rb2d.velocity.y);
        }
        if (rb2d.velocity.x < -2.5f * s)
        {
            rb2d.velocity = new Vector2(-2.5f * s, rb2d.velocity.y);
        }
        if (rb2d.velocity.y > 2.5f * s)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, 2.5f * s);
        }
        if (rb2d.velocity.y < -2.5f * s)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, -2.5f * s);
        }
        t.localScale = new Vector2(0.25f+0.75f*scaler/100f, 0.25f+0.75f*scaler/100f);
        scaler++;
    }
    void moveing()
    {
        //Store the current horizontal input in the float moveHorizontal.
        float moveHorizontal = 0.1f * ((prb.position.x - rb2d.position.x)+2*Random.Range(-s,s));
        //Store the current vertical input in the float moveVertical.
        float moveVertical = 0.1f * ((prb.position.y - rb2d.position.y)+2*Random.Range(-s,s));
        //Use the two store floats to create a new Vector2 variable movement.
        float speed = Random.Range(1, s);

        //normal acceleration
        rb2d.AddForce(new Vector2(moveHorizontal * speed*Random.Range(50, 100), moveVertical * speed* Random.Range(50, 100)));

    }
}
