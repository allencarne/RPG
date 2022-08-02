using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitIndicator : MonoBehaviour
{
    public int enemyDamage = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Player>().PlayerHitState(enemyDamage);
        }
    }

    public void DestroyAfterAnimation()
    {
        Destroy(gameObject);
    }
}
