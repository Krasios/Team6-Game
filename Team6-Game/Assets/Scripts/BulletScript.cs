using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float speed;

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

            // Connor and Duy, if using the charge gun the bullet splinters with large bullets
            if (PlayerPrefs.GetInt("SpecialGun") == 2 && (transform.localScale.x >= 10f) )
            {
                for (int a = 0; a < 8; a++)
                {
                    // 
                    GameObject bullet = Instantiate(this.gameObject, transform.position, transform.rotation);
                    bullet.transform.localScale = new Vector3(bullet.transform.localScale.x - 3f , bullet.transform.localScale.y - 3f, bullet.transform.localScale.z);
                    bullet.GetComponent<Rigidbody2D>().rotation += a * (360 / 8);
                }
            }

            Instantiate(pureLightPrefab, other.gameObject.transform.position, Quaternion.identity); // Spawn a light where the enemy dies
            gameObject.SetActive(false); //turn off the bullet
        }
    }
}
