using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShieldScript : MonoBehaviour
{
    public GameObject player;
    public GameObject shieldPrefab;
    public Sprite damageShield;
    public AudioClip shieldHit;
    public AudioClip shieldRegen;

    private Sprite okShield;
    private GameObject shield;
    private float maxShieldDistance = 300;
    private float maxShieldRadius = 15;
    private float shieldAnimated = 0.5f;
    private float currentAnimated = 0f;
    private float maxHits = 10;
    private float hitsTaken = 0;
    private bool shieldActive;
    private bool isHit = false;
    private float timeRegen;
    private float timeToRegen = 10;
    private float shieldRadius;

    void Start() {
        shield = Instantiate(shieldPrefab, player.transform.position, Quaternion.identity);        
        shield.transform.parent = player.transform;
        shield.transform.position = player.transform.position;
        shieldActive = true;
        shield.GetComponent<CircleCollider2D>().isTrigger = true;
        okShield = shield.GetComponent<SpriteRenderer>().sprite;
    }

    void Update() {
        if(shieldActive){
            maxHits = (int)Math.Log(player.GetComponent<PlayerCtrl>().lightCount);
            shieldRadius = maxHits-hitsTaken;
            shield.GetComponent<CircleCollider2D>().radius = shieldRadius*2.5f/(shield.transform.localScale.x + 1);
            shield.transform.localScale = Vector3.one * shieldRadius;
        }

        if (isHit) {
            currentAnimated += Time.deltaTime;
            if (currentAnimated >= 5) {
                isHit = false;
                shield.GetComponent<SpriteRenderer>().sprite = okShield;
            }
        }
        if (!shieldActive) {
            timeRegen += Time.deltaTime;
            if (timeRegen >= timeToRegen && player.GetComponent<PlayerCtrl>().lightCount > 0) {
                shield.GetComponent<AudioSource>().PlayOneShot(shieldRegen, 0.7F);
                shieldActive = true;
                timeRegen = 0;
                //AOE on shield regeneration
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                for (int i=0; i<enemies.Length; i++) {
                    if( Vector2.Distance(player.transform.position, enemies[i].transform.position ) <= shieldRadius*3f) {
                        enemies[i].SetActive(false);
                    }
                }
                shield.GetComponent<SpriteRenderer>().enabled = true;     
                shield.GetComponent<CircleCollider2D>().enabled = true;         
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (shieldActive && other.gameObject.CompareTag("Enemy")) {
            tutorialCtrl.instance.asteroidZapped();
            other.gameObject.SetActive(false);
            shield.GetComponent<AudioSource>().PlayOneShot(shieldHit, 0.2F);
            hitsTaken += 1;
            isHit = true;
            currentAnimated = 0f;
            player.GetComponent<PlayerCtrl>().lightCount += 10;
            shield.GetComponent<SpriteRenderer>().sprite = damageShield;
            if (hitsTaken > maxHits) {
                shieldActive = false;
                hitsTaken = 0;
                shield.GetComponent<SpriteRenderer>().enabled = false;
                shield.GetComponent<CircleCollider2D>().enabled = false;
            }
        }
    }

    // ShieldActive status getter for calling from player ctrl --Trevor--
    public bool IsShieldActive()
    {
        Debug.Log(shieldActive);
        return shieldActive;
    }
}
