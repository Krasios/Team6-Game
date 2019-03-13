using UnityEngine;
using System.Collections;

public class memberMovement : MonoBehaviour {
    
    public float timeOffset = 0;
    public float radius = 50;
    public float speed = 1;
    public Boid alienBoid;

    // Update is called once per frame
    void Update () {
        transform.position = new Vector2(alienBoid.location.x, alienBoid.location.y);
        transform.rotation = Quaternion.FromToRotation(Vector3.up, new Vector3( alienBoid.velocity.x, alienBoid.velocity.y, 0));
    }
}
