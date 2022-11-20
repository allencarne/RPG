using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    public PlayerScriptableObject playerScriptableObject;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            // Hit Effect
            GameObject effect = Instantiate(playerScriptableObject.weapon.basicAttackHitEffect, other.transform.position, Quaternion.identity);
            Destroy(effect, .3f);

            // Deal Damage
            other.GetComponent<Enemy>().EnemyHitState(playerScriptableObject.weapon.attackDamage);

            // Apply Knockback
            Rigidbody2D enemy = other.GetComponent<Rigidbody2D>();
            Vector2 difference = enemy.transform.position - transform.position;
            difference = difference.normalized * playerScriptableObject.weapon.basicAttackKnockBackForce;
            enemy.AddForce(difference, ForceMode2D.Impulse);
        }

        if (other.tag == "TrainingDummy")
        {
            // Hit Effect
            GameObject effect = Instantiate(playerScriptableObject.weapon.basicAttackHitEffect, other.transform.position, Quaternion.identity);
            Destroy(effect, .3f);

            // Deal Damage
            other.GetComponent<TrainingDummy>().TrainingDummyHitState();

            // Apply Knockback
            Rigidbody2D enemy = other.GetComponent<Rigidbody2D>();
            Vector2 difference = enemy.transform.position - transform.position;
            difference = difference.normalized * playerScriptableObject.weapon.basicAttackKnockBackForce;
            enemy.AddForce(difference, ForceMode2D.Impulse);
        }
    }

    public void DestroyAfterAnimation()
    {
        Destroy(gameObject);
    }
}
