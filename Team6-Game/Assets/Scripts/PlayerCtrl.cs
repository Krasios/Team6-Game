using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{
    public float speed;
    public string name;
    public GameObject bulletPrefab;

    private Light playerLight;
    private Rigidbody2D rigidB;


    void Start() {
        rigidB = GetComponent<Rigidbody2D>();
        playerLight = GetComponent<Light>();
    }

    void FixedUpdate() {
        float moveHorizontal = Input.GetAxis("Horizontal_"+name);
        float moveVertical = Input.GetAxis("Vertical_"+name);

        rigidB.AddForce(new Vector2(moveHorizontal,moveVertical)*speed);
        if (playerLight.range > 5){
            playerLight.range -= 0.01f;
        }
        if (Input.GetButton("Jump")) {
            Instantiate(bulletPrefab, transform.position, transform.rotation);
        }
    }
    void OnTriggerEnter2D(Collider2D other) {

    }
}
