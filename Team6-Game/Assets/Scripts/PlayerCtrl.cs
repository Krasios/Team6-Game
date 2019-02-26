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
    public bool joystick;
    private ParticleSystem shipLightParticles;
    float maxParticleSpawnRate = 50; // Maximum spawn rate of particles
 

    float xRot;
    float yRot;
    float moveHorizontal;
    float moveVertical;

    // Bullet var - Duy
    public float fireRate;
    public float chargeRate;
    public float laserRate;
    public float laserLength;
    public int SprayNum; // num of initial sprays
    public float SprayRate; //time it takes spray
    public float SprayAngle; //10 default?
    public bool enableSpray;

    private bool isFirstShot;
    private bool isCharging;
    private bool isLaser;
    private float laserTimer;
    private float nextFire;
    private float nextCharge;
    private float spread;

    private AudioSource bulletsound;


    void Start() {
        rigidB = GetComponent<Rigidbody2D>();
        shipLightParticles = GetComponent<ParticleSystem>();
        // playerLight = GetComponent<Light>();

        // Set initial light properties --Connor--
        lightCount = 100; // Start with 10 units of light
        lightText.text = "Light: " + lightCount;
        shootCost = 2; // Shooting costs 2 light units
        bulletsound = GetComponent<AudioSource>();
        nextCharge = 0;
        nextFire = fireRate;
        isCharging = false;
        isLaser = false;
        laserTimer = 0;
    }

    void FixedUpdate() {
        if (joystick)
        {
            moveHorizontal = Input.GetAxis("Joystick-Horizontal-" + name);
            moveVertical = Input.GetAxis("Joystick-Vertical-" + name);
            xRot = Input.GetAxis("stickXrotion-" + name);
            yRot = Input.GetAxis("stickYrotion-" + name);
        } else
        {
            moveHorizontal = Input.GetAxis("Keyboard-Horizontal-" + name);
            moveVertical = Input.GetAxis("Keyboard-Vertical-" + name);
            //orientation determined by movement
            xRot = Input.GetAxis(/*"keyboardXrotion-"*/"Keyboard-Horizontal-" + name);
            yRot = Input.GetAxis(/*"keyboardYrotion-"*/"Keyboard-Vertical-" + name);
            yRot = -yRot;
        }




        // Prevent snapping back to neutral position
        if (Mathf.Abs(xRot) > 0.0 || Mathf.Abs(yRot) > 0.0)
        {
            UpdatePlayerRot();
        }

        // Decelerate the ship when left stick has no movement
        if ((Mathf.Abs(moveHorizontal) <= 0.05 && Mathf.Abs(moveHorizontal) >= 0)
            && (Mathf.Abs(moveVertical) <= 0.05 && Mathf.Abs(moveVertical) >= 0))
        {
            rigidB.AddForce(-rigidB.velocity * 0.2f); // slow down the ship
        }

        // Add force to the player
        rigidB.AddForce(new Vector2(moveHorizontal, moveVertical) * speed);

        // Check to see if the player is moving too fast
        float maxVel = 50.0f;
        // Cap the max velocity
        if (Mathf.Abs(rigidB.velocity.magnitude) > maxVel)
        {
            rigidB.AddForce(-rigidB.velocity * 1.0f); // Cancel out any new velocity
        }
        //possible gun design: hold individually to shoot shotgun that costs only 1 bullet
        //before it shoots as a continuous spray, draining many bullets

        //current gun design: longest charge for bullet stream. Second longest for shotgun.
        //Third for triple. Tap to fire. Set rate to 0 to disable.
/*        if ((Input.GetButton("Jump-"+name) || Input.GetButton("Fire-"+name)) && (lightCount > 0)) { // If press space and have more than 0 light
            Instantiate(bulletPrefab, transform.position, transform.rotation);
            lightCount -= shootCost; // Subtract the shoot cost from the light total
            updateLightText(); // Update the UI's text
        }*/

        nextFire += Time.deltaTime;
        Vector3 spawn = transform.position - transform.up * 5; //spawn bullet at tip of gun

        if (laserTimer < laserLength && isLaser == true)
        {
            isCharging = false;
            laserTimer += Time.deltaTime;
            if (enableSpray == true)
            {
                spread = Random.Range(-SprayAngle, SprayAngle);
                GameObject bullet = Instantiate(bulletPrefab, spawn, transform.rotation);
                bullet.GetComponent<Rigidbody2D>().rotation += spread;
                bulletsound.Play();
            }
            else
            {
                Instantiate(bulletPrefab, spawn, transform.rotation);
                bulletsound.Play();
            }
        }
        else if ((Input.GetButton("Jump-" + name) || Input.GetButton("Fire-" + name))
            /*&& (lightCount > 0)*/  && isLaser == false)
        // If press space and have more than 0 light
        {
            isLaser = false;
            if (nextFire > fireRate && isCharging == false)
            {
                Instantiate(bulletPrefab, spawn, transform.rotation);
                bulletsound.Play();
                nextFire = 0;
                lightCount -= shootCost; // Subtract the shoot cost from the light total
                updateLightText(); // Update the UI's text
            }
            nextCharge += Time.deltaTime;
            isCharging = true;
        }
        else
        {
            isLaser = false;
            isCharging = false;
            if (nextCharge > laserRate && laserRate !=0/*4 * chargeRate*/)
            {
                laserTimer = 0;
                isLaser = true;
                nextCharge = 0;
            }
            else if (nextCharge > SprayRate && SprayRate != 0) //charge shot: 3x bullets or a charged bullet prefab
            {
                //Instantiate(bulletPrefab, spawn + transform.right * 2, transform.rotation);
                //Instantiate(bulletPrefab, spawn - transform.right * 2, transform.rotation);
                //Instantiate(bulletPrefab, spawn, transform.rotation);
                for (int i = 0; i < SprayNum; i++)
                {
                    spread = Random.Range(-SprayAngle, SprayAngle);
                    GameObject bullet = Instantiate(bulletPrefab, spawn, transform.rotation);
                    bullet.GetComponent<Rigidbody2D>().rotation += spread;
                    //Instantiate(bulletPrefab, spawn, transform.rotation * Quaternion.AngleAxis(spread, transform.up));
                    //Instantiate(bulletPrefab, spawn, transform.rotation * Quaternion.Euler(spread, transform.up));
                }
                bulletsound.Play();
                lightCount -= shootCost; // Subtract the shoot cost from the light total
                updateLightText(); // Update the UI's text
                nextCharge = 0;
            }
            else if (nextCharge > chargeRate && chargeRate != 0)
            {
                Instantiate(bulletPrefab, spawn + transform.right * 2, transform.rotation);
                Instantiate(bulletPrefab, spawn - transform.right * 2, transform.rotation);
                Instantiate(bulletPrefab, spawn, transform.rotation);
                bulletsound.Play();
                lightCount -= shootCost; // Subtract the shoot cost from the light total
                updateLightText(); // Update the UI's text
                nextCharge = 0;
            }
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

        

    }

    // Connor and Trevor
    void UpdatePlayerRot()
    {
        float rotSpeed = 15.0f; //  Determine how fast the ship rotates
        //Use arc tan to get radians of in between angle then convert to degrees 
        float playerDirection = Mathf.Atan2(xRot, yRot) * Mathf.Rad2Deg;

        // Convert the degrees into a quaternion around the world's z axis
        Quaternion quat = Quaternion.Euler(0.0f, 0.0f, playerDirection);

        // Update the rotation of the ship to match the angle of the right stick
        transform.rotation = Quaternion.Slerp(transform.rotation, quat, Time.deltaTime * rotSpeed);
    }

    //-- Connor --//
    // Change the light text after updating the lightCount
    public void updateLightText()
    {
        lightText.text = "Light: " + lightCount;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // If the player hits an enemy lose light
            lightCount -= 10;
            updateLightText();
        }
    }
}
