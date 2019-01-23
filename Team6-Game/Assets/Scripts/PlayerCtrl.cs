using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{
    public float speed;
    public string name;

    private Rigidbody2D rigidB;

    void Start() {
        rigidB = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        float moveHorizontal = Input.GetAxis("Horizontal_"+name);
        float moveVertical = Input.GetAxis("Vertical_"+name);

        rigidB.AddForce(new Vector2(moveHorizontal,moveVertical)*speed);
    }
    void OnTriggerEnter2D(Collider2D other) {

    }
}
