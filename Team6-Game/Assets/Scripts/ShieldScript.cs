using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldScript : MonoBehaviour
{
    public GameObject player;
    public GameObject partner;
    public GameObject shieldPrefab;

    private GameObject shield;
    private float maxShieldDistance = 300;
    private float maxShieldRadius = 15;
    private float distance = 0;
    private int maxHits = 10;
    private int hitsTaken = 0;
    private bool shieldActive;
    private float timeRegen;

    void Start()
    {
        shield = Instantiate(shieldPrefab, player.transform.position, Quaternion.identity);        
        shield.transform.parent = player.transform;
        shield.transform.position = player.transform.position;
        shieldActive = true;
        shield.GetComponent<CircleCollider2D>().isTrigger = true;
        distance = Vector2.Distance(player.transform.position,partner.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(player.transform.position,partner.transform.position);
        float shieldRadius = (distance > 20) ? (maxShieldDistance/distance) : maxShieldRadius;
        shield.GetComponent<CircleCollider2D>().radius = shieldRadius*2.5f/(shield.transform.localScale.x);
        shield.transform.localScale = Vector3.one * shieldRadius;
        if (!shieldActive) {
            timeRegen += Time.deltaTime;
            if (timeRegen >= distance) {
                shieldActive = true;
                timeRegen = 0;
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other) {
        if (shieldActive && other.gameObject.CompareTag("Enemy")) {
            Destroy(other.gameObject);
            hitsTaken += 1;
            if (hitsTaken > maxHits) {
                shieldActive = false;
                hitsTaken = 0;
            }
        }

    }
}
