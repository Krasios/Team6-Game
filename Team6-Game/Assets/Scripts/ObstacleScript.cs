using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    // Start is called before the first frame update
    public int obstaclePoolSize = 100;
    public GameObject obstaclePrefab;
    public float spawnRate = 0.5f;
    public GameObject player;

    private GameObject[] obstacles;
    private float timeSinceLastSpawned;
    private int currentObstacle = 0;
    private float obstacleDist = 500f;
    private float offsetDist = 475f/2f;
    private float spawnXPosition;
    private float spawnYPosition;

    void Start()
    {
        obstacles = new GameObject[obstaclePoolSize];
        for (int i = 0; i < obstaclePoolSize; i++)
        {
            spawnXPosition = Random.Range(player.transform.position.x - obstacleDist, player.transform.position.x + obstacleDist);
            spawnYPosition = Random.Range(player.transform.position.y - obstacleDist, player.transform.position.y + obstacleDist);

            if (spawnXPosition <= 0)
            {
                spawnXPosition -= offsetDist;
            }
            else
            {
                spawnXPosition += offsetDist;
            }

            if (spawnYPosition <= 0)
            {
                spawnYPosition -= offsetDist;
            }
            else
            {
                spawnYPosition += offsetDist;
            }

            obstacles[i] = (GameObject)Instantiate(obstaclePrefab, new Vector2(spawnXPosition, spawnYPosition), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastSpawned += Time.deltaTime;
        if (timeSinceLastSpawned >= spawnRate)
        {
            timeSinceLastSpawned = 0;
            spawnXPosition = Random.Range(player.transform.position.x - obstacleDist, player.transform.position.x + obstacleDist);
            spawnYPosition = Random.Range(player.transform.position.y - obstacleDist, player.transform.position.y + obstacleDist);
            obstacles[currentObstacle].transform.position = new Vector2(spawnXPosition, spawnYPosition);
            obstacles[currentObstacle].SetActive(true);
            currentObstacle++;

            if (currentObstacle >= obstaclePoolSize)
            {
                currentObstacle = 0;
            }
        }
    }
}
