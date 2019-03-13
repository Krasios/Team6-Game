using UnityEngine;
using System.Collections;

public class SwarmCtrl : MonoBehaviour {

    // adapted from https://processing.org/examples/flocking.html

    public int numberOfAliens = 100;
    public float spawnLocationX = 300;
    public float spawnLocationY = 300;
    public float areaWidth = 990;
    public float areaHeight = 540;
    public float alienSize = 5f;
    public float maxSpeed = 2f;
    public float maxForce = 0.1f;
    public float separationWeight = 2.5f;
    public float alignmentWeight = 1;
    public float cohesionWeight = 1;
    public float avoidPlayerWeight = 2f;
    public float attractPlayerWeight = 2f;
    public GameObject swarmMember;
    public GameObject player;

    Flock flock;

    void Start () {
        flock = new Flock();
        for ( int i = 0; i < numberOfAliens; i++ ) {
            var alien = new Boid(spawnLocationX+Random.Range(-2,2), spawnLocationY+Random.Range(-2,2), this);
            var alienObject = Instantiate(swarmMember);
            alienObject.SetActive(true);
            alienObject.GetComponent<memberMovement>().alienBoid = alien;
            flock.AddBoid(alien);
        }
    }
    
    void Update () {
        flock.Update();
    }

}