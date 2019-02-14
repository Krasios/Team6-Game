using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float speed = 6;

    private Rigidbody2D rb2d;
    public GameObject pureLightPrefab;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = -transform.up * speed;
    }

    // Update is called once per frame
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Enemy")) {
            //PlayerCtrl player.GetComponent<PlayerCtrl>().lightCount += 10;
            other.gameObject.SetActive(false);//or destroy whichever works better
            Instantiate(pureLightPrefab, other.gameObject.transform.position, Quaternion.identity); // Spawn a light where the enemy dies
            gameObject.SetActive(false); //turn off the bullet
        }
    }
}
