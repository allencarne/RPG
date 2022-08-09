using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject enemySpawner;
    [SerializeField] int maxEnemyCount;
    [SerializeField] Vector3 spawnBox;
    public int enemyCount;
    float xPos;
    float yPos;


    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemy();
    }

    private void Update()
    {
        //if (enemyCount < maxEnemyCount)
        //{
        //    SpawnEnemy();
        //}
    }

    public void SpawnEnemy()
    {
        //enemyCount += 1;
        xPos = Random.Range(-spawnBox.x, spawnBox.x);
        yPos = Random.Range(-spawnBox.y, spawnBox.y);

        Instantiate(enemyPrefab, new Vector3(xPos, yPos, 0) + enemySpawner.transform.position, Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(enemySpawner.transform.position, spawnBox);
    }
}
