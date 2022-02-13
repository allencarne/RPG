using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject enemySpawner;
    public Vector3 spawnBox;
    private float xPos;
    private float yPos;


    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        xPos = Random.Range(-spawnBox.x, spawnBox.x);
        yPos = Random.Range(-spawnBox.y, spawnBox.y);

        Instantiate(enemyPrefab, new Vector3(xPos, yPos, 0) + transform.position, Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, spawnBox);
    }
}
