using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float speed = 6;

    private Rigidbody2D rb2d;
    public GameObject pureLightPrefab;
    public GameObject player;
    public float rotateSpeed = 400f;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        //rb2d.velocity = -transform.up * speed;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (gameObject.name == "Homer(Clone)")
        {
            Transform target = FindClosestEnemy().transform;
            Vector2 direction = (Vector2)target.position - rb2d.position;
            direction.Normalize();
            float rotateAmount = Vector3.Cross(direction, -transform.up).z;
            rb2d.angularVelocity = -rotateAmount * rotateSpeed;
        }
        if (gameObject.name == "Boomer")
        {
            //speed = ExplosiveTypeSpeed; for reduced explosive speed? may omit
        }
        rb2d.velocity = -transform.up * speed;
    }
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            //PlayerCtrl player.GetComponent<PlayerCtrl>().lightCount += 10;
            other.gameObject.SetActive(false);//or destroy whichever works better
            Instantiate(pureLightPrefab, other.gameObject.transform.position, Quaternion.identity); // Spawn a light where the enemy dies
            if (gameObject.name == "Boomer")
            {
                //Instantiate(gameObject);
            }
            gameObject.SetActive(false); //turn off the bullet
        }
    }
    public GameObject FindClosestEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }
}
