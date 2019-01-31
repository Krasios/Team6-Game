using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{
    public float speed;
    public string name;
    public GameObject bulletPrefab;

    //public Light playerLight;
    private Rigidbody2D rigidB;

    // Light Variables -- Connor --
    public int lightCount;
    public Text lightText;
    public int shootCost;
    private ParticleSystem shipLightParticles;
    float maxParticleSpawnRate = 50; // Maximum spawn rate of particles


    void Start() {
        rigidB = GetComponent<Rigidbody2D>();
        shipLightParticles = GetComponent<ParticleSystem>();
        // playerLight = GetComponent<Light>();

        // Set initial light properties
        lightCount = 100; // Start with 10 units of light
        lightText.text = "Light: " + lightCount;
        shootCost = 2; // Shooting costs 2 light units
    }

    void FixedUpdate() {
        float moveHorizontal = Input.GetAxis("Horizontal_"+name);
        float moveVertical = Input.GetAxis("Vertical_"+name);

        rigidB.AddForce(new Vector2(moveHorizontal,moveVertical)*speed);
        //if (playerLight.range > 5){
        //    playerLight.range -= 0.01f;
        //}
        if (Input.GetButton("Jump") && (lightCount > 0)) { // If press space and have more than 0 light
            Instantiate(bulletPrefab, transform.position, transform.rotation);
            lightCount -= shootCost; // Subtract the shoot cost from the light total
            updateLightText(); // Update the UI's text
        }

        //-- Connor --//
        // Change the particle birth rate based on light
        float spawnRate = lightCount / 2;
        if (spawnRate > maxParticleSpawnRate)
        {
            spawnRate = maxParticleSpawnRate;
        }
        
        var shipPartEmiss = shipLightParticles.emission; // need to make a ref b4 you can set the rate
        shipPartEmiss.rateOverTime = spawnRate; // Set the particle spawn rate

        // Rotate the ship
        if (Input.GetButton("Fire2"))
        {
            int rotspd = 5;
            transform.Rotate(Vector3.forward, rotspd);
        }

    }

    //-- Connor --//
    // Change the light text after updating the lightCount
    void updateLightText()
    {
        lightText.text = "Light: " + lightCount;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false); // Remove light from game
            lightCount += 30;
            updateLightText();
        }
    }
}
