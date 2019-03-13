using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// The Boid class
// modified from https://processing.org/examples/flocking.html

public class Boid {

    public Vector2 location;
    public Vector2 velocity;
    Vector2 acceleration;
    float size;        // size of boid
    float viewWidth;
    float viewHeight;
    float maxForce;    // Maximum steering force
    float maxSpeed;    // Maximum speed
    float separationWeight;
    float alignmentWeight;
    float cohesionWeight;
    float avoidWeight;
    float attractWeight;
    SwarmCtrl swarmCtrl;

    private static float MIN_DIST_FROM_PLAYER = 10;
    private static float MAX_DIST_FROM_PLAYER = 500;
    public Boid(float x, float y, SwarmCtrl _swarmCtrl) {
        swarmCtrl = _swarmCtrl;
        acceleration = new Vector2(0, 0);
        velocity = new Vector2(Random.Range(-1,1), Random.Range(-1,1));
        location = new Vector2(x,y);
    }

    public void Update(List<Boid> boids) {

        viewWidth = swarmCtrl.areaWidth/2;
        viewHeight = swarmCtrl.areaHeight/2;
        size = swarmCtrl.alienSize;
        maxSpeed = swarmCtrl.maxSpeed;
        maxForce = swarmCtrl.maxForce;
        separationWeight = swarmCtrl.separationWeight;
        alignmentWeight = swarmCtrl.alignmentWeight;
        cohesionWeight = swarmCtrl.cohesionWeight;
        avoidWeight = swarmCtrl.avoidPlayerWeight;
        attractWeight = swarmCtrl.attractPlayerWeight;

        Flock(boids);
        UpdateLocation();
        Borders();
    }

    void ApplyForce(Vector2 force) {
        // We could add mass here if we want A = F / M
        acceleration += force;
    }

    // We accumulate a new acceleration each time based on three rules
    void Flock(List<Boid> boids) {
        Vector2 separationForce = Separate(boids);   // Separation
        Vector2 alignmentForce  = Align(boids);      // Alignment
        Vector2 cohesionForce  = Cohesion(boids);   // Cohesion
        Vector2 avoidForce = Avoidance(boids);
        Vector2 attractForce = Attract(boids);
        // Arbitrarily weight these forces
        separationForce *= separationWeight;
        alignmentForce *= alignmentWeight;
        cohesionForce *= cohesionWeight;
        avoidForce *= avoidWeight;
        attractForce *= attractWeight;
        // Add the force vectors to acceleration
        ApplyForce(separationForce);
        ApplyForce(alignmentForce);
        ApplyForce(cohesionForce);
        ApplyForce(avoidForce);
        ApplyForce(attractForce);
    }

    // Method to update location
    void UpdateLocation() {
        // Update velocity
        velocity += acceleration;
        // Limit speed
        velocity = Limit(velocity, maxSpeed);
        location += velocity;
        // Reset accelertion to 0 each cycle
        acceleration *= 0;
    }

    // A method that calculates and applies a steering force towards a target
    // STEER = DESIRED MINUS VELOCITY
    Vector2 Seek(Vector2 target) {
        Vector2 desired = target -location;  // A vector pointing from the location to the target
        // Scale to maximum speed
        desired.Normalize();
        desired *= maxSpeed;
        Vector2 steer = desired - velocity;
        steer = Limit(steer,maxForce);  // Limit to maximum steering force
        return steer;
    }

    // want enemies to skirt the edge instead of teleporting
    void Borders() {
        if (location.x < -viewWidth+size) {location.x = -viewWidth+size; velocity.x = -velocity.x;}
        if (location.y < -viewHeight+size) {location.y = -viewHeight+size; velocity.y = -velocity.y;}
        if (location.x > viewWidth-size) {location.x = viewWidth-size; velocity.x = -velocity.x;}
        if (location.y > viewHeight-size) {location.y = viewHeight-size; velocity.y = -velocity.y;}
    }
    Vector2 Attract(List<Boid> boids) {
        float desiredDistance = (1/attractWeight) * MAX_DIST_FROM_PLAYER;
        Vector2 steer = new Vector2(0, 0);
        if (Vector2.Distance(location,swarmCtrl.player.transform.position) > desiredDistance) {
            steer = new Vector2(swarmCtrl.player.transform.position.x,swarmCtrl.player.transform.position.y) - location;
        
            if (steer.sqrMagnitude > 0) {
                // Implement Reynolds: Steering = Desired - Velocity
                steer.Normalize();
                steer *= maxSpeed;
                steer -= velocity;
                steer = Limit(steer, maxForce);
            }
        }
        return steer;
    }
    Vector2 Avoidance(List<Boid> boids) {
        float desiredDistance = avoidWeight * MIN_DIST_FROM_PLAYER;
        Vector2 steer = new Vector2(0, 0);
        if (Vector2.Distance(location,swarmCtrl.player.transform.position) < desiredDistance) {
            steer =  location - new Vector2(swarmCtrl.player.transform.position.x,swarmCtrl.player.transform.position.y);
            if (steer.sqrMagnitude > 0) {
                // Implement Reynolds: Steering = Desired - Velocity
                steer.Normalize();
                steer *= maxSpeed;
                steer -= velocity;
                steer = Limit(steer, maxForce);
            }
        }
        return steer;
    }
    // Separation
    // Method checks for nearby boids and steers away
    Vector2 Separate (List<Boid> boids) {
        float desiredseparation = size;
        Vector2 steer = new Vector2(0, 0);
        int count = 0;
        // For every boid in the system, check if it's too close
        foreach (var other in boids) {
            if ( other == this ) {continue;}
            Vector2 diff = location - other.location;
            float d = Vector2.SqrMagnitude(diff);
            // If the distance is greater than 0 and less than an arbitrary amount (0 when you are yourself)
            if (d > 0 && d < desiredseparation*desiredseparation) {
                // Calculate vector pointing away from neighbor
                diff /= d;        // Weight by distance
                steer += diff;
                count++;            // Keep track of how many
            }
        }
        // Average -- divide by how many
        if (count > 0) {
            steer /= (float)count;
        }

        // As long as the vector is greater than 0
        if (steer.sqrMagnitude > 0) {
            steer.Normalize();
            steer *= maxSpeed;
            steer -= velocity;
            steer = Limit(steer, maxForce);
        }
        return steer;
    }

    // Alignment
    // For every nearby boid in the system, calculate the average velocity
    Vector2 Align (List<Boid> boids) {
        float neighbordist = 50;
        Vector2 sum = new Vector2(0, 0);
        int count = 0;
        foreach (var other in boids) {
            float d = Vector2.Distance(location, other.location);
            if ((d > 0) && (d < neighbordist)) {
                sum += other.velocity;
                count++;
            }
        }
        if (count > 0) {
            sum /= (float)count;
            sum.Normalize();
            sum *= maxSpeed;
            Vector2 steer = sum - velocity;
            steer = Limit(steer,maxForce);
            return steer;
        } 
        else {
            return new Vector2(0, 0);
        }
    }

    // Cohesion
    // For the average location (i.e. center) of all nearby boids, calculate steering vector towards that location
    Vector2 Cohesion (List<Boid> boids) {
        float neighbordist = 50;
        Vector2 sum = new Vector2(0, 0);   // Start with empty vector to accumulate all locations
        int count = 0;
        foreach (var other in boids) {
            float d = Vector2.Distance(location, other.location);
            if ((d > 0) && (d < neighbordist)) {
                sum += other.location; // Add location
                count++;
            }
        }
        if (count > 0) {
            sum /= count;
            return Seek(sum);  // Steer towards the location
        } 
        else {
            return new Vector2(0, 0);
        }
    }

    Vector2 Limit ( Vector2 vector, float max ) {
        if ( vector.sqrMagnitude > max*max )
            return vector.normalized*max;
        else
            return vector;
    }
}
