using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateOnce : MonoBehaviour
{
    // Start is called before the first frame update
    public int obstaclePoolSize = 25;
    public GameObject obstaclePrefab;
    public GameObject player;

    private GameObject[] obstacles;
    private float obstacleDist = 100f;
    private Vector2 objectPoolPosition;

    void Start()
    {
        obstacles = new GameObject[obstaclePoolSize];
        for (int i = 0; i < obstaclePoolSize; i++) {
            float spawnXPosition = Random.Range(player.transform.position.x-obstacleDist,player.transform.position.x+obstacleDist);
            float spawnYPosition = Random.Range(player.transform.position.y-obstacleDist,player.transform.position.y+obstacleDist);
            objectPoolPosition = new Vector2(spawnXPosition,spawnYPosition);
            obstacles[i] = (GameObject)Instantiate (obstaclePrefab, objectPoolPosition, Quaternion.identity);
            obstacles[i].SetActive(true);
        }
    }
}
