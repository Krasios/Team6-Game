using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_shooting_1 : MonoBehaviour
{
    private enemy_movement_2 movement_variables;
    private Vector2 proximity;
    private Rigidbody2D rigid;
    private Rigidbody2D prigid;
    private GameObject player;
    public GameObject bullet; //prefab for bullets
    public float offset_distance;
    public float spray_angle;
    public float spray_chance;
    //bullet math variables
    Vector3 offset;
    float angle;
    Quaternion q;
    // Start is called before the first frame update
    void Start()
    {
        movement_variables = GetComponent<enemy_movement_2>();
        rigid = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player1");
        prigid = player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        proximity = rigid.position - prigid.position;
        if (proximity.magnitude<movement_variables.enemy_range && Random.Range(0,1000) > (1000-10*spray_chance))
        {
            enemy_shooting();
        }
    }
    void enemy_shooting()
    {
        Vector3 inst_position = transform.position - (offset_distance/proximity.magnitude)*new Vector3(proximity.x, proximity.y, 0);
        offset = new Vector3(prigid.position.x, prigid.position.y, 0) - transform.position;
        angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg+90;
        q = Quaternion.AngleAxis(angle+Random.Range(-spray_angle,spray_angle), Vector3.forward);
        GameObject inst = Instantiate(bullet,inst_position,q);
        inst.gameObject.tag = "Enemy";
    }
}
