using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_movement_2 : MonoBehaviour
{
    //variables
    public float speed;
    private GameObject player;
    //vectors
    public Vector2 move_range;
    public float enemy_range;
    public float withdraw_range;
    private Vector2 proximity;
    private Vector2 prox2;
    private Vector2 dest_generated;
    private float max_speed;
    //rigid bodies and player components
    private Rigidbody2D prigid;
    private Rigidbody2D rigid;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player1"); // Connor, Find the object named player 1 in the scene (get the instance of the player prefab)
        prigid = player.GetComponent<Rigidbody2D>();
        dest_generated = rigid.position+new Vector2(Random.Range(-move_range.x, move_range.x), Random.Range(-move_range.y, move_range.y));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        generate_vectors();
  
        if (proximity.magnitude < enemy_range && Random.Range(1, 100)<85)
        {
            move_player();
        }
        else
        {
            rigid.velocity = 0.99f * rigid.velocity;
            rigid.AddForce(-prox2);
        }
        //max velocity
        if (rigid.velocity.magnitude > speed)
        {
            max_speed = (rigid.velocity.magnitude != 0) ? (speed / (rigid.velocity.magnitude)) : 1;
            rigid.velocity = (rigid.velocity * max_speed);
        }
        
    }

    //void LateUpdate()
    //{
    //    // Connor, Set the rotation of the sprite so it always faces the player
    //    Vector3 relPos = player.transform.position - transform.position; // Get the relative position from player to this object
    //    transform.up = new Vector2(relPos.x, relPos.y); // Change the direction of up to be the relative position's x and y coordinates
    //}

    //PREDEFINED METHODS
    //generates vectors
    void generate_vectors()
    {
        proximity = rigid.position-prigid.position;
        prox2 = rigid.position - dest_generated;
        if ((prox2.magnitude< withdraw_range)||Random.Range(1,300)>298) {
            dest_generated = rigid.position + new Vector2(Random.Range(-move_range.x, move_range.x), Random.Range(-move_range.y, move_range.y));
        }else if(Random.Range(1, 200) > 198)
        {
            dest_generated = prigid.position + new Vector2(Random.Range(-move_range.x, move_range.x), Random.Range(-move_range.y, move_range.y));
        }
        if (proximity.magnitude < enemy_range)
        {
            vector_rotate(prigid.position);
        }
        else
        {
            vector_rotate(dest_generated);
        }
    }
    //rotate to a point (dest_location)
    void vector_rotate(Vector2 pointer)
    {
        Vector3 offset = new Vector3(pointer.x,pointer.y,0) - transform.position;
        float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg-90;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime *2* speed);
    }
    void move_player()
    {
        if (proximity.magnitude < withdraw_range)
        {
            rigid.velocity = 0.9f * rigid.velocity;
        }
        else
        {
            rigid.AddForce(-proximity * speed);
        }
    }
}
