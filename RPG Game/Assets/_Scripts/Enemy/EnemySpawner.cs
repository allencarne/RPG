using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject enemySpawner;
    [SerializeField] int maxEnemyCount;
    [SerializeField] Vector3 spawnBox;
    int enemyCount;
    float xPos;
    float yPos;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnemySpawn());
    }

    IEnumerator EnemySpawn()
    {
        while (enemyCount < maxEnemyCount)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.01f);
            enemyCount += 1;
        }
    }

    public void SpawnEnemy()
    {
        xPos = Random.Range(-spawnBox.x, spawnBox.x);
        yPos = Random.Range(-spawnBox.y, spawnBox.y);

        Instantiate(enemyPrefab, new Vector3 (xPos, yPos, 0) + enemySpawner.transform.position, Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(enemySpawner.transform.position, spawnBox);
    }
}
