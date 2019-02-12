using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RopeScript : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public GameObject ropePrefab; // Change to lightBondPrefab

    private List<GameObject> joints = new List<GameObject>();
    private float playerRadius = 6f;
    private float ropeSegmentSize = 2.896f; // Set the scale

    void Start() {
        Vector2 player1Position = player1.transform.position;
        Vector2 player2Position = player2.transform.position;
        float ropeLength = Vector2.Distance(player1Position,player2Position) / ropeSegmentSize; // Set the number of rope segments
        float jointXOffset = (player2Position.x - player1Position.x) / ropeLength;
        float jointYOffset = (player2Position.y - player1Position.y) / ropeLength;
        for (float i = 0; i < ropeLength - playerRadius; i+=1) {
            Vector3 position = new Vector2(player1Position.x + playerRadius + jointXOffset * i, player1Position.y + jointYOffset * i);
            GameObject ropeJoint = Instantiate(ropePrefab, position, Quaternion.identity);
            // Bind the first segment to the 1st player
            if (i == 0) {
                ropeJoint.GetComponent<DistanceJoint2D>().connectedBody = player1.GetComponent<Rigidbody2D>();
                ropeJoint.GetComponent<DistanceJoint2D>().distance = playerRadius;
            }
            else { // Add joints to the rope
                ropeJoint.GetComponent<DistanceJoint2D>().connectedBody = joints[(int)i-1].GetComponent<Rigidbody2D>();
            }
            // Bind the last segment to the 2nd player
            if (i+1 >= ropeLength-playerRadius) {
                DistanceJoint2D dj2d = ropeJoint.AddComponent<DistanceJoint2D>();
                dj2d.connectedBody = player2.GetComponent<Rigidbody2D>();
                dj2d.autoConfigureDistance = false;
                dj2d.distance = playerRadius;
            }
            // Add the current joint into the list
            joints.Add(ropeJoint);
        }

    }

    void FixedUpdate() {
        // Update the rotation
        for (int i = 0; i < joints.Count; i+=1) {
            GameObject prev;
            GameObject next;

            if (i == 0) {
                prev = player1;
                next = joints[i+1];
            } else if (i+1 >= joints.Count) {
                prev = joints[i-1];
                next = player2;
            } else {
                prev = joints[i-1];
                next = joints[i+1];
            }
            Vector3 vectorToTarget = prev.transform.position - joints[i].transform.position;
            float angle = (Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg);
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            joints[i].transform.rotation = Quaternion.Slerp(joints[i].transform.rotation, q, Time.deltaTime * 10);
        }
    }
    void OnTriggerEnter2D(Collider2D other) {

    }
}
