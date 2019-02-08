using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldScript : MonoBehaviour
{
    public GameObject player;
    public GameObject partner;
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
    private float distance = 0;
    private int maxHits = 10;
    private int hitsTaken = 0;
    private bool shieldActive;
    private bool isHit = false;
    private float timeRegen;

    void Start() {
        shield = Instantiate(shieldPrefab, player.transform.position, Quaternion.identity);        
        shield.transform.parent = player.transform;
        shield.transform.position = player.transform.position;
        shieldActive = true;
        shield.GetComponent<CircleCollider2D>().isTrigger = true;
        distance = Vector2.Distance(player.transform.position,partner.transform.position);
        okShield = shield.GetComponent<SpriteRenderer>().sprite;
    }

    void Update() {
        distance = Vector2.Distance(player.transform.position,partner.transform.position);
        float shieldRadius = (distance > 20) ? (maxShieldDistance/distance) : maxShieldRadius;
        shield.GetComponent<CircleCollider2D>().radius = shieldRadius*2.5f/(shield.transform.localScale.x);
        shield.transform.localScale = Vector3.one * shieldRadius;
        if (isHit) {
            currentAnimated += Time.deltaTime;
            if (currentAnimated >= shieldAnimated) {
                isHit = false;
                shield.GetComponent<SpriteRenderer>().sprite = okShield;
            }
        }
        if (!shieldActive) {
            timeRegen += Time.deltaTime;
            if (timeRegen >= distance) {
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
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (shieldActive && other.gameObject.CompareTag("Enemy")) {
            other.gameObject.SetActive(false);
            shield.GetComponent<AudioSource>().PlayOneShot(shieldHit, 0.2F);
            hitsTaken += 1;
            isHit = true;
            currentAnimated = 0f;
            shield.GetComponent<SpriteRenderer>().sprite = damageShield;
            if (hitsTaken > maxHits) {
                shieldActive = false;
                hitsTaken = 0;
                shield.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }
}
