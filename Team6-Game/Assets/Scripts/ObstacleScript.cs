using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    // Start is called before the first frame update
    public int obstaclePoolSize = 25;
    public GameObject obstaclePrefab;
    public float spawnRate = 0.5f;
    public GameObject player;

    private GameObject[] obstacles;
    private Vector2 objectPoolPosition = new Vector2(-400f,-200f);
    private float timeSinceLastSpawned;
    private int currentObstacle = 0;
    private float obstacleDist = 50f;

    void Start()
    {
        obstacles = new GameObject[obstaclePoolSize];
        for (int i = 0; i < obstaclePoolSize; i++) {
            obstacles[i] = (GameObject)Instantiate (obstaclePrefab, objectPoolPosition, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastSpawned += Time.deltaTime;
        if (timeSinceLastSpawned >= spawnRate) {
            timeSinceLastSpawned = 0;
            float spawnXPosition = Random.Range(player.transform.position.x-obstacleDist,player.transform.position.x+obstacleDist);
            float spawnYPosition = Random.Range(player.transform.position.y-obstacleDist,player.transform.position.y+obstacleDist);
            obstacles[currentObstacle].transform.position = new Vector2(spawnXPosition,spawnYPosition);
            obstacles[currentObstacle].SetActive(true);
            currentObstacle++;

            if (currentObstacle >= obstaclePoolSize) {
                currentObstacle = 0;
            }
        }
    }
}
