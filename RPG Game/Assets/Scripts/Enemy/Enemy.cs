using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;
    public int maxHealth = 100;
    private int currentHealth;
    public HealthBar healthBar;
    public EnemySpawner enemySpawner;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);

        // Player hurt animation
        animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Die animation
        animator.SetBool("isDead", true);

        // Disable the enemy
        healthBar.gameObject.SetActive(false);
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, 5f);
        //this.enabled = false;

        // Spawn another enemy
        enemySpawner.SpawnEnemy();
    }
}
