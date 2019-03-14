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
    public int specialCost;
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
    // public float rapidRate;
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
    private float nextLeftFire, nextRightFire, nextSpecialFire; // Connor
    private float nextCharge;
    private float nextSpread;
    private float spread;
    public int explosiveCost;
    private AudioSource bulletsound;
    public bool canShoot; // Connor

    private int primaryGun; // playerPref key-value --Trevor
    private int specialGun; // ''                ''

    private float shotgunStrength;
    private float homingStrength;
    private float machineStrength;
    private float chargeStrength;
    private float lazerStrength;
    private float bulletStrength;
    private float fireRateStrength;


    private void Awake()
    {
        //Connor, On player spawn in level 1 or tutorial level
        if (string.Compare(SceneManager.GetActiveScene().name, "RopeTest") == 0 || string.Compare(SceneManager.GetActiveScene().name, "Tutorial") == 0)
        {
            PlayerPrefs.SetFloat("FireRateStrength", 1f);
            PlayerPrefs.SetFloat("BulletStrength", 1f);

            // Setup starting guns
            PlayerPrefs.SetInt("PrimaryGun", 0);
            PlayerPrefs.SetInt("SpecialGun", 0);

            PlayerPrefs.Save();
        }
        // Need this rn or fire rate doesnt work on  levels past tutorial and 1
        else
        {
            PlayerPrefs.SetFloat("FireRateStrength", 1f);
            PlayerPrefs.SetFloat("BulletStrength", 1f);
            PlayerPrefs.Save();
        }
        
    }

    void Start() {
        rigidB = GetComponent<Rigidbody2D>();
        shipLightParticles = GetComponent<ParticleSystem>();
        lightComp = GetComponent<Light>();

        // Set initial light properties --Connor--
        lightCount = 100; // Start with 100 units of light

        // If there is light text set it
        if (lightText)
        {
            lightText.text = "Light: " + lightCount;
        }
        
        specialCost = 5; // Base Special Weapon cost

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
        nextLeftFire = fireRate;
        nextRightFire = fireRate;
        nextSpecialFire = fireRate;
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
        shotgunStrength = PlayerPrefs.GetFloat("ShotgunStrength");
        homingStrength = PlayerPrefs.GetFloat("HomingStrength");
        machineStrength = PlayerPrefs.GetFloat("MachineStrength");
        chargeStrength = PlayerPrefs.GetFloat("ChargeStrength");
        lazerStrength = PlayerPrefs.GetFloat("LazerStrength");
        bulletStrength = PlayerPrefs.GetFloat("BulletStrength");
        fireRateStrength = PlayerPrefs.GetFloat("FireRateStrength");
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
        lightComp.intensity = ((lightCount + 300) / 1000) * 2;

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
            rigidB.AddForce(-rigidB.velocity * 0.4f); // slow down the ship
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


        // Advance weapon cooldown timers
        nextLeftFire += Time.deltaTime;
        nextRightFire += Time.deltaTime;
        nextSpecialFire += (Time.deltaTime / 2f); //Charge slower than regular weapons

        Vector3 spawn = transform.position - transform.up * 5; //spawn bullet at tip of gun

        //----- Shooting 
        // Connor, check to see which gun is selected and call the right function to fire it
        if (canShoot == true) // If the player can shoot
        {
            // Left Primary Gun, Check the left mouse button was clicked
            if (Input.GetMouseButton(0))
            {
                Debug.Log("Player fired left gun, Gun is: " + primaryGun.ToString());

                switch (primaryGun)
                {
                    // Single Shot
                    case 0:
                        Debug.Log("Player fired p_singleshot");
                        // Wait for cooldown to finish before firing
                        if (nextLeftFire > fireRateStrength)
                        {
                            RegularFire(spawn);
                            nextLeftFire = 0; // Reset left gun cooldown timer
                        }
                        break;
                    // Shotgun
                    case 1:
                        Debug.Log("Player fired p_shotgunshot");
                        // Wait for cooldown to finish before firing
                        if (nextLeftFire > (fireRateStrength) )
                        {
                            SpreadFire(spawn);
                            nextLeftFire = 0; // Reset left gun cooldown timer
                        }
                        break;
                    // Homing Gun
                    case 2:
                        Debug.Log("Player fired p_homingshot");
                        if (nextLeftFire > fireRateStrength)
                        {
                            RegularFire(spawn);
                            nextLeftFire = 0; // Reset left gun cooldown timer
                        }
                        break;
                }
                
            }

            // Right Primary Gun, Check the right mouse button was clicked
            if (Input.GetMouseButton(1))
            {
                Debug.Log("Player fired right gun");
                switch (primaryGun)
                {
                    // Single Shot
                    case 0:
                        Debug.Log("Player fired p_singleshot");
                        // Wait for cooldown to finish before firing
                        if (nextRightFire > fireRateStrength)
                        {
                            RegularFire(spawn);
                            nextRightFire = 0; // Reset left gun cooldown timer
                        }
                        break;
                    // Shotgun
                    case 1:
                        Debug.Log("Player fired p_shotgunshot");
                        // Wait for cooldown to finish before firing
                        if (nextRightFire > (fireRateStrength))
                        {
                            SpreadFire(spawn);
                            nextRightFire = 0; // Reset left gun cooldown timer
                        }
                        break;
                    // Homing Gun
                    case 2:
                        Debug.Log("Player fired p_homingshot");
                        if (nextRightFire > fireRateStrength)
                        {
                            RegularFire(spawn);
                            nextRightFire = 0; // Reset left gun cooldown timer
                        }
                        break;
                }
            }

            // Check if the spacebar was pressed and Fire Special Weapon
            if (Input.GetButton("Keyboard-Jump") )
            {
                Debug.Log("Player fired special weapon");
                switch(specialGun)
                {
                    case 0:
                        Debug.Log("No Special Weapon Equipped");
                        break;
                    // Machine Gun
                    case 1:
                        Debug.Log("Fire machine gun");
                        if (nextSpecialFire > (fireRateStrength / 3f ) )
                        {
                            RapidFire(spawn);
                        }
                        nextSpecialFire = 0;
                        break;
                    // Machine Gun
                    case 2:
                        Debug.Log("Fire Charge gun");

                        if (nextSpecialFire > fireRateStrength)
                        {
                            ExplosiveFire(spawn);
                        }
                        nextSpecialFire = 0;
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
        //        if (nextLeftFire > fireRate && isCharging == false) //only fire once when hold
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
        //            if (nextLeftFire > rapidRate)
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
                PlayerPrefs.SetFloat("ShotgunStrength", 1);
                break;
            case 1:
                PlayerPrefs.SetFloat("ShotgunStrength", 2);
                break;
            case 2:
                PlayerPrefs.SetFloat("ShotgunStrength", 3);
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
                PlayerPrefs.SetFloat("HomingStrength", 1);
                break;
            case 1:
                PlayerPrefs.SetFloat("HomingStrength", 2);
                break;
            case 2:
                PlayerPrefs.SetFloat("HomingStrength", 3);
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
                PlayerPrefs.SetFloat("MachineStrength", 1);
                break;
            case 1:
                PlayerPrefs.SetFloat("MachineStrength", 2);
                break;
            case 2:
                PlayerPrefs.SetFloat("MachineStrength", 3);
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
                PlayerPrefs.SetFloat("ChargeStrength", 1);
                break;
            case 1:
                PlayerPrefs.SetFloat("ChargeStrength", 2);
                break;
            case 2:
                PlayerPrefs.SetFloat("ChargeStrength", 3);
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
                PlayerPrefs.SetFloat("LazerStrength", 1);
                break;
            case 1:
                PlayerPrefs.SetFloat("LazerStrength", 2);
                break;
            case 2:
                PlayerPrefs.SetFloat("LazerStrength", 3);
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

    // No implementation yet
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
                PlayerPrefs.SetFloat("BulletStrength", 2);
                break;
            case 2:
                PlayerPrefs.SetFloat("BulletStrength", 3);
                break;
            case 3:
                PlayerPrefs.SetFloat("BulletStrength", 4);
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
                PlayerPrefs.SetFloat("FireRateStrength", 0.8f);
                break;
            case 2:
                PlayerPrefs.SetFloat("FireRateStrength", 0.6f);
                break;
            case 3:
                PlayerPrefs.SetFloat("FireRateStrength", 0.4f);
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
        //lightCount -= specialCost; // Subtract the shoot cost from the light total
        // updateLightText(); // Update the UI's text
        
    }
    // Machine Gun, Duy and Connor
    void RapidFire(Vector3 spawn)
    {
        Instantiate(bulletPrefab, spawn, transform.rotation);
        bulletsound.Play();

        if (lightCount > 0)
        {
            lightCount -= specialCost; // Subtract the shoot cost from the light total
        }
        if (lightCount < 0)
        {
            lightCount = 0;
        }
        
        updateLightText(); // Update the UI's text
        // nextLeftFire = 0;
    }
    // Shotgun, Duy and Connor
    void SpreadFire(Vector3 spawn)
    {
        for (int a = 0; a < spreadNum; a++)
        {
            spread = Random.Range(-spreadAngle, spreadAngle);
            GameObject bullet = Instantiate(bulletPrefab, spawn, transform.rotation);
            bullet.GetComponent<Rigidbody2D>().rotation += spread;
        }
        bulletsound.Play();
        //lightCount -= specialCost * spreadNum; // Subtract the shoot cost from the light total
        //updateLightText(); // Update the UI's text
        //nextCharge = 0;
    }
    void ExplosiveFire(Vector3 spawn) //scale bullet up, in bullet code: if above size 15, explode
    {
        GameObject bullet = Instantiate(bulletPrefab, spawn, transform.rotation);
        bullet.gameObject.transform.localScale += bullet.gameObject.transform.localScale;
        bulletsound.Play();
        //lightCount -= explosiveCost; // Subtract the shoot cost from the light total
        //updateLightText(); // Update the UI's text
        //nextCharge = 0;
    }

    // Inactive
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
        //lightCount -= maxLight;
        updateLightText(); // Update the UI's text
        nextCharge = 0;
    }
}
