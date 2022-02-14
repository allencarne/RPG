using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void PlayerTakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            PlayerDie();
        }
    }

    public void PlayerDie()
    {
        Destroy(gameObject);
    }
}
