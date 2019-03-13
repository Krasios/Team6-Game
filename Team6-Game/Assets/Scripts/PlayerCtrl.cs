using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerCtrl : MonoBehaviour
{
    public float speed;
    public string name;
    public GameObject bulletPrefab;
    private Camera camera;

    //public Light playerLight;
    private Rigidbody2D rigidB;

    
    // Light Variables -- Connor --
    public float lightCount;
    public Text lightText;
    public int shootCost;
    public bool joystick;
    private Light lightComp;

    private ParticleSystem shipLightParticles;
    float maxParticleSpawnRate = 50; // Maximum spawn rate of particles
 
    float xRot;
    float yRot;
    float moveHorizontal;
    float moveVertical;

    // UI elements -- Trevor -- 
    public Slider healthSlider;
    public Slider lightSlider;
    public float currentHealth = 100;
    public float maxHealth = 100;
    public float maxLight = 1000;

    // Bullet var - Duy
    //ways to shoot: press regular bullet, hold charged shotgun for right trigger.
    //hold alternate fire for rapid fire stream, double tap for big bullet spreading bomb 
    //tap both fire buttons for stream of bullets in shotgun spread (laser substitute)
    //recommendation for playtesting: ask players to try each separate button's control scheme.
    public float fireRate; //regular bullet
    public float chargeRate; //charged exploding bullet
    //public float laserRate;
    //public float laserLength;
    public float rapidRate;
    public float superRate;
    public float superLength;
    public int spreadNum; // num of initial sprays
    public float spreadRate; //time it takes to spread
    public float spreadAngle; //10 default?
    public bool enableSpray;
    private bool isFirstShot;
    private bool isCharging;
    //private bool isLaser;
    //private float laserTimer;
    private bool isSuper;
    private float superTimer;
    private float nextFire;
    private float nextCharge;
    private float nextSpread;
    private float spread;
    public int explosiveCost;
    private AudioSource bulletsound;
    public bool canShoot; // Connor

    private int primaryGun; // playerPref key-value --Trevor
    private int specialGun; // ''                ''

    private int shotgunStrength;
    private int homingStrength;
    private int machineStrength;
    private int chargeStrength;
    private int lazerStrength;
    private int bulletStrength;
    private int fireRateStrength;


    void Start() {
        rigidB = GetComponent<Rigidbody2D>();
        shipLightParticles = GetComponent<ParticleSystem>();
        lightComp = GetComponent<Light>();

        // Set initial light properties --Connor--
        lightCount = 100; // Start with 100 units of light
        lightText.text = "Light: " + lightCount;
        shootCost = 2; // Shooting costs 2 light units

        // Sets the health bar to full instantly at the start of the level
        if (healthSlider)
        {
            healthSlider.value = 1;
        }
        camera = Camera.main; // Set the cam var to the current main camera

        // Duy and Connor, Shooting Stuff 
        bulletsound = GetComponent<AudioSource>();
        nextCharge = 0;
        nextSpread = 0;
        nextFire = fireRate;
        isCharging = false;
        //isLaser = false;
        //laserTimer = 0;
        isSuper = false;
        isFirstShot = true;
        superTimer = 0;

        // Pulls current values for which gun number is equipped weapons --Trevor
        primaryGun = PlayerPrefs.GetInt("PrimaryGun");
        specialGun = PlayerPrefs.GetInt("SpecialGun");
        Debug.Log(primaryGun.ToString() +  specialGun.ToString());

        // Pulls current upgrade strength for each weapon --Trevor
        shotgunStrength = PlayerPrefs.GetInt("ShotgunStrength");
        homingStrength = PlayerPrefs.GetInt("HomingStrength");
        machineStrength = PlayerPrefs.GetInt("MachineStrength");
        chargeStrength = PlayerPrefs.GetInt("ChargeStrength");
        lazerStrength = PlayerPrefs.GetInt("LazerStrength");
        bulletStrength = PlayerPrefs.GetInt("BulletStrength");
        fireRateStrength = PlayerPrefs.GetInt("FireRateStrength");
        Debug.Log(shotgunStrength.ToString() + homingStrength.ToString() + machineStrength.ToString()
        + chargeStrength.ToString() + lazerStrength.ToString() + bulletStrength.ToString() + fireRateStrength.ToString());

    }

    private void Update()
    {
        // Animates the light and health bars as they gain or lose value --Trevor--
        if (healthSlider != null)
        {
            healthSlider.value = Mathf.Lerp(healthSlider.value, currentHealth / maxHealth, 0.05f);
        }
        if (lightSlider != null)
        {
            lightSlider.value = Mathf.Lerp(lightSlider.value, lightCount / maxLight, 0.05f);
        }

        // Connor, Load the death scene
        if (currentHealth <= 0)
        {
            SceneManager.LoadScene("LevelDeath", LoadSceneMode.Single);
        }



        // Prevent snapping back to neutral position and rotate player when using joystick
        if ((Mathf.Abs(xRot) > 0.0 || Mathf.Abs(yRot) > 0.0) && joystick)
        {
            UpdatePlayerRotStick();
        }
        else // Make the player face the mouse when using mouse and keyboard
        {
            faceMouse(); // Connor
        }

        // Connor, Set the intensity of the player's light based on their resource count
        lightComp.intensity = ((lightCount + 300) / 1000) * 3;

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

            // Unused now
            //xRot = Input.GetAxis("keyboardXrotion-" + name);
            //yRot = Input.GetAxis("keyboardYrotion-" + name);
            
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


        // Cooldown fo weapons
        nextFire += Time.deltaTime;
        //nextSpread += Time.deltaTime;

        Vector3 spawn = transform.position - transform.up * 5; //spawn bullet at tip of gun

        //----- Shooting 
        // Duy: shooting mechanics, Connor : Cleaned up code and changed it to work with upgrade system...
        // ... that Trevor implemented
        if (canShoot == true) // If the player can shoot
        {
            // Check the left mouse button was clicked
            if (Input.GetMouseButton(0))
            {
                Debug.Log("Player fired left gun, Gun is: " + primaryGun.ToString());

                switch (primaryGun)
                {
                    // Single Shot
                    case 0:
                        Debug.Log("Player fired p_singleshot");
                        if (nextFire > fireRate)
                            RegularFire(spawn);
                        nextCharge += Time.deltaTime;
                        break;
                    // Shotgun
                    case 1:
                        Debug.Log("Player fired p_shotgunshot");
                        SpreadFire(spawn);
                        break;
                    // Shotgun
                    case 2:
                        Debug.Log("Player fired p_homingshot");
                        RapidFire(spawn);
                        break;
                }
            }

            // Check the right mouse button was clicked
            if (Input.GetMouseButton(1))
            {
                Debug.Log("Player fired right gun");
                switch (primaryGun)
                {
                    // Single Shot
                    case 0:
                        Debug.Log("Player fired p_singleshot");
                        if (nextFire > fireRate)
                            RegularFire(spawn);
                        nextCharge += Time.deltaTime;
                        break;
                    // Shotgun
                    case 1:
                        Debug.Log("Player fired p_shotgunshot");
                        SpreadFire(spawn);
                        break;
                    // Shotgun
                    case 2:
                        Debug.Log("Player fired p_homingshot");
                        RapidFire(spawn);
                        break;
                }
            }

            // Check if the spacebar was pressed
            if (Input.GetButton("Keyboard-Jump") )
            {
                Debug.Log("Player fired special weapon");
                switch(specialGun)
                {
                    case 0:
                        Debug.Log("No Special Weapon Equipped");

                        // Temp for testing
                        ExplosiveFire(spawn);

                        break;
                    // Machine Gun
                    case 1:
                        Debug.Log("Fire machine gun");
                        if (nextFire > rapidRate)
                        {
                            RapidFire(spawn);
                        }
                        break;
                    // Machine Gun
                    case 2:
                        Debug.Log("Fire Charge gun");

                        // Only fire if the gun has cooled down
                        if (nextCharge > fireRate)
                            ExplosiveFire(spawn);
                        break;
                }
            }
        }


        // if shooting is enabled
        //if (canShoot == true)
        //{

        //    if (superTimer < superLength && isSuper == true)
        //    { //super state, can't shoot anything else
        //        isCharging = false;
        //        superTimer += Time.deltaTime;
        //        SuperFire(spawn);
        //    }
        //    else if (Input.GetButton("Jump-" + name) || Input.GetButton("Fire-" + name) || Input.GetMouseButton(0))
        //    // If press button and have more than 0 light
        //    {
        //        isSuper = false;
        //        nextSpread = 0;
        //        isFirstShot = true;
        //        if (nextFire > fireRate && isCharging == false) //only fire once when hold
        //            RegularFire(spawn);
        //        nextCharge += Time.deltaTime;
        //        isCharging = true;
        //    }
        //    else if (Input.GetButtonUp("Jump-" + name) || Input.GetButtonUp("Fire-" + name) || Input.GetMouseButtonUp(0))
        //    { //if let go of button
        //        isCharging = false;
        //        if (nextCharge > chargeRate && chargeRate != 0)
        //        {
        //            ExplosiveFire(spawn);
        //        }
        //    }
        //    else if (Input.GetMouseButton(1)) // Right mouse button
        //    {
        //        nextSpread += Time.deltaTime;
        //        if (nextSpread > spreadRate)
        //        {
        //            if (isFirstShot)
        //            {
        //                SpreadFire(spawn);
        //                isFirstShot = false;
        //            }
        //            if (nextFire > rapidRate)
        //            {
        //                RapidFire(spawn);
        //            }
        //        }
        //    }
        //    else if ((Input.GetButton("Jump-" + name) || Input.GetButton("Fire-" + name) || Input.GetMouseButton(0)) && Input.GetMouseButton(1))
        //    {//press both fire buttons
        //        if (lightCount == maxLight)
        //        {
        //            superTimer = 0;
        //            isSuper = true;
        //            nextCharge = 0;
        //            bulletsound.Play(); //to be replaced with super blast sound
        //        }
        //    }
        //    else
        //    {
        //        isSuper = false;
        //        isCharging = false;
        //        nextSpread = 0;
        //        isFirstShot = true;
        //    }
        //}


        //-- Change the particle birth rate based on player's light -- Connor
        float spawnRate = lightCount / 2;
        if (spawnRate > maxParticleSpawnRate)
        {
            spawnRate = maxParticleSpawnRate;
        }
        var shipPartEmiss = shipLightParticles.emission; // need to make a ref b4 you can set the rate
        shipPartEmiss.rateOverTime = spawnRate; // Set the particle spawn rate

        

    }

    //----- Store Related Functions -- Connor && Trevor --
    public void buyShotgun()
    {
        switch (shotgunStrength)
        {
            case 0:
                PlayerPrefs.SetInt("ShotgunStrength", 1);
                break;
            case 1:
                PlayerPrefs.SetInt("ShotgunStrength", 2);
                break;
            case 2:
                PlayerPrefs.SetInt("ShotgunStrength", 3);
                break;
            case 3:
                Debug.Log("Cant Upgrade this any more");
                break;
            default:
                Debug.Log("Something went wrong upgrading the shotgun Upgrade");
                break;
        }
        PlayerPrefs.Save();
        Debug.Log("Bought a shotgun");
    }

    public void equipShotgun()
    {
        PlayerPrefs.SetInt("PrimaryGun", 1);       // Note: cHange these to gun selection in the equip screen
        PlayerPrefs.Save();
    }

    public void buyHominggun()
    {
        switch (homingStrength)
        {
            case 0:
                PlayerPrefs.SetInt("HomingStrength", 1);
                break;
            case 1:
                PlayerPrefs.SetInt("HomingStrength", 2);
                break;
            case 2:
                PlayerPrefs.SetInt("HomingStrength", 3);
                break;
            case 3:
                Debug.Log("Cant Upgrade this any more");
                break;
            default:
                Debug.Log("Something went wrong upgrading the homing upgrade");
                break;
        }
        PlayerPrefs.Save();
        Debug.Log("Bought a homing gun");

    }

    public void equipHominggun()
    {
        PlayerPrefs.SetInt("PrimaryGun", 2);
        PlayerPrefs.Save();
    }

    public void buyMachinegun()
    {
        switch (machineStrength)
        {
            case 0:
                PlayerPrefs.SetInt("MachineStrength", 1);
                break;
            case 1:
                PlayerPrefs.SetInt("MachineStrength", 2);
                break;
            case 2:
                PlayerPrefs.SetInt("MachineStrength", 3);
                break;
            case 3:
                Debug.Log("Cant Upgrade this any more");
                break;
            default:
                Debug.Log("Something went wrong upgrading the machineGun upgrade");
                break;
        }
        PlayerPrefs.Save();
        Debug.Log("Bought a machine gun");

    }

    public void equipMachinegun()
    {
        PlayerPrefs.SetInt("SpecialGun", 1);
        PlayerPrefs.Save();
    }

    public void buyChargegun()
    {
        switch (chargeStrength)
        {
            case 0:
                PlayerPrefs.SetInt("ChargeStrength", 1);
                break;
            case 1:
                PlayerPrefs.SetInt("ChargeStrength", 2);
                break;
            case 2:
                PlayerPrefs.SetInt("ChargeStrength", 3);
                break;
            case 3:
                Debug.Log("Cant Upgrade this any more");
                break;
            default:
                Debug.Log("Something went wrong upgrading the chargeGun upgrade");
                break;
        }
        PlayerPrefs.Save();
        Debug.Log("Bought a Charge gun");
    }

    public void equipChargegun()
    {
        PlayerPrefs.SetInt("SpecialGun", 2);
        PlayerPrefs.Save();
    }

    public void buyLaser()
    {
        switch (lazerStrength)
        {
            case 0:
                PlayerPrefs.SetInt("LazerStrength", 1);
                break;
            case 1:
                PlayerPrefs.SetInt("LazerStrength", 2);
                break;
            case 2:
                PlayerPrefs.SetInt("LazerStrength", 3);
                break;
            case 3:
                Debug.Log("Cant Upgrade this any more");
                break;
            default:
                Debug.Log("Something went wrong upgrading the lazerGun upgrade");
                break;
        }
        PlayerPrefs.Save();
        Debug.Log("Bought a laser gun");

    }

    public void equipLazer()
    { 
        PlayerPrefs.SetInt("SpecialGun", 3);
        PlayerPrefs.Save();
    }

    public void buyUpgradeBullets()
    { 
        switch (bulletStrength)
        {
            case 1:
                PlayerPrefs.SetInt("BulletStrength", 2);
                break;
            case 2:
                PlayerPrefs.SetInt("BulletStrength", 3);
                break;
            case 3:
                PlayerPrefs.SetInt("BulletStrength", 4);
                break;
            case 4:
                Debug.Log("Cant Upgrade this any more");
                break;
            default:
                Debug.Log("Something went wrong upgrading the bulletStrengh upgrade");
                break;
        }
        PlayerPrefs.Save();
        Debug.Log("Bought an Upgrade for Bullets");

    }

    public void buyUpgradeFireRate()
    { 
        switch (fireRateStrength)
        {
            case 1:
                PlayerPrefs.SetInt("FireRateStrength", 2);
                break;
            case 2:
                PlayerPrefs.SetInt("FireRateStrength", 3);
                break;
            case 3:
                PlayerPrefs.SetInt("FireRateStrength", 4);
                break;
            case 4:
                Debug.Log("Cant Upgrade this any more");
                break;
            default:
                Debug.Log("Something went wrong upgrading the fireRate upgrade");
                break;
        }
        PlayerPrefs.Save();
        Debug.Log("Bought an Upgrade for Fire Rate");

    }


    // Rotate the player to face the mouse cursor -- Connor
    void faceMouse()
    {
        //Vector3 mouseWorldPos = camera.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        Vector3 mousWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Connor, Set the rotation of the sprite so it always faces the player
        Vector2 relPos = new Vector2(transform.position.x - mousWorldPos.x,
            transform.position.y - mousWorldPos.y); // Get the relative position from player to this object

        transform.up = relPos; // Change the direction of up to be the relative position's x and y coordinates
    }

    // Rotate the player character when using a joystick -- Connor and Trevor
    void UpdatePlayerRotStick()
    {
        float rotSpeed = 15.0f; //  Determine how fast the ship rotates
        //Use arc tan to get radians of in between angle then convert to degrees 
        float playerDirection = Mathf.Atan2(xRot, yRot) * Mathf.Rad2Deg;

        // Convert the degrees into a quaternion around the world's z axis
        Quaternion quat = Quaternion.Euler(0.0f, 0.0f, playerDirection);

        // Update the rotation of the ship to match the angle of the right stick
        transform.rotation = Quaternion.Slerp(transform.rotation, quat, Time.deltaTime * rotSpeed);
    }

    //-- Connor --// // Edited negative light check --Trevor-- //
    // Change the light text after updating the lightCount
    public void updateLightText()
    {
        lightText.text = "Light: " + lightCount;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") && lightCount > 0)
        {
            // If the player hits an enemy lose light
            lightCount -= 10;
            // If light is now negative, set to 0 instead 
            if (lightCount < 0)
            {
                lightCount = 0;
            }
            updateLightText();
        }
        // If the player is hit without shields up, reduce players health --Trevor//edited for tutorial Sharon--
        if (other.gameObject.CompareTag("Enemy") && 
            ((GetComponent<ShieldScript>()!=null && GetComponent<ShieldScript>().shieldActive==false) ||
            (GetComponent<tutorialShield>()!=null && GetComponent<tutorialShield>().shieldActive==false)))
        {
            currentHealth -= 10;
            Debug.Log("Player hit, health: " + currentHealth);
        }
    }

    //---- Duy Do and Connor: action/shooting code
    void RegularFire(Vector3 spawn)
    {
        Instantiate(bulletPrefab, spawn, transform.rotation);
        bulletsound.Play();
        //should regular bullet not cost light?
        lightCount -= shootCost; // Subtract the shoot cost from the light total
        updateLightText(); // Update the UI's text
        nextFire = 0;
    }
    void RapidFire(Vector3 spawn) //currently, like regular fire
    {
        Instantiate(bulletPrefab, spawn, transform.rotation);
        bulletsound.Play();
        //should regular bullet not cost light?
        lightCount -= shootCost; // Subtract the shoot cost from the light total
        updateLightText(); // Update the UI's text
        nextFire = 0;
    }
    void SpreadFire(Vector3 spawn)
    {
        if (enableSpray == true)
        {
            for (int a = 0; a < spreadNum; a++)
            {
                spread = Random.Range(-spreadAngle, spreadAngle);
                GameObject bullet = Instantiate(bulletPrefab, spawn, transform.rotation);
                bullet.GetComponent<Rigidbody2D>().rotation += spread;
            }
        }
        else
        {
            for (int a = 0; a < spreadNum; a++)
            {
                GameObject bullet = Instantiate(bulletPrefab, spawn, transform.rotation);
                bullet.GetComponent<Rigidbody2D>().rotation += (-spreadAngle + spreadAngle * 2 * a / (spreadNum - 1));
                //the calculation to figure this out took embarassingly long
            }
        }
        bulletsound.Play();
        lightCount -= shootCost * spreadNum; // Subtract the shoot cost from the light total
        updateLightText(); // Update the UI's text
        nextCharge = 0;
    }
    void ExplosiveFire(Vector3 spawn) //scale bullet up, in bullet code: if above size 15, explode
    {
        GameObject bullet = Instantiate(bulletPrefab, spawn, transform.rotation);
        bullet.gameObject.transform.localScale += bullet.gameObject.transform.localScale;
        bulletsound.Play();
        lightCount -= explosiveCost; // Subtract the shoot cost from the light total
        updateLightText(); // Update the UI's text
        nextCharge = 0;
    }
    void SuperFire(Vector3 spawn) //like spread, but active per frame
    {
        if (enableSpray == true)
        {
            for (int a = 0; a < spreadNum; a++)
            {
                spread = Random.Range(-spreadAngle, spreadAngle);
                GameObject bullet = Instantiate(bulletPrefab, spawn, transform.rotation);
                bullet.GetComponent<Rigidbody2D>().rotation += spread;
            }
        }
        else
        {
            for (int a = 0; a < spreadNum; a++)
            {
                GameObject bullet = Instantiate(bulletPrefab, spawn, transform.rotation);
                bullet.GetComponent<Rigidbody2D>().rotation += -spreadAngle + spreadAngle * 2 * a * (spreadAngle - 1);
                //the calculation to figure this out took embarassingly long
            }
        }
        lightCount -= maxLight;
        updateLightText(); // Update the UI's text
        nextCharge = 0;
    }
}
