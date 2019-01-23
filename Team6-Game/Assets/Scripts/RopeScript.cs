using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RopeScript : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public GameObject ropePrefab;

    private List<GameObject> joints = new List<GameObject>();

    void Start() {
        Vector2 player1Position = player1.transform.position;
        Vector2 player2Position = player2.transform.position;
        transform.position = (player1Position + player2Position)/2;
        float ropeLength = Vector2.Distance(player1Position,player2Position);
        float jointXOffset = (player2Position.x-player1Position.x)/ropeLength;
        float jointYOffset = (player2Position.y-player1Position.y)/ropeLength;
        for (float i = 0; i < ropeLength; i+=1) {
            Vector3 position = new Vector2(player1Position.x + jointXOffset * (1+i), player1Position.y + jointYOffset * (1+i));
            //Quaternion rotation = new Quaternion(1, 1, 1, 1);
            GameObject ropeJoint = Instantiate(ropePrefab, position, transform.rotation);
            if (i == 0) {
                ropeJoint.GetComponent<DistanceJoint2D>().connectedBody = player1.GetComponent<Rigidbody2D>();
            }else {
                ropeJoint.GetComponent<DistanceJoint2D>().connectedBody = joints[(int)i-1].GetComponent<Rigidbody2D>();
            }
            if (i+1 > ropeLength) {
                DistanceJoint2D dj2d = ropeJoint.AddComponent<DistanceJoint2D>();
                dj2d.connectedBody = player2.GetComponent<Rigidbody2D>();
                dj2d.distance = 1.0f;
            }
            joints.Add(ropeJoint);
        }

    }

    void FixedUpdate() {
    }
    void OnTriggerEnter2D(Collider2D other) {

    }
}
