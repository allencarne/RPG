using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    public GameObject enemyHitIndicator;
    public Rigidbody2D rb;
    Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player Aimer").transform;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Attack()
    {
        Instantiate(enemyHitIndicator, player.transform.position, Quaternion.identity);
    }
}
