using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// The Flock (a list of Boid objects)
// adapted from https://processing.org/examples/flocking.html

public class Flock {
    List<Boid> boids; // An List for all the boids

    public Flock() {
        boids = new List<Boid>(); // Initialize the List
    }

    public void Update() {
        foreach (var boid in boids) {
            boid.Update(boids);  // Passing the entire list of boids to each boid individually
        }
    }

    public void AddBoid(Boid b) {
        boids.Add(b);
    }

}
