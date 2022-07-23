using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public EnemySpawner enemySpawner;
    public GameObject expObject;
    public Animator animator;
    public Image frontHealthBar;
    public Image backHealthBar;
    public Canvas enemyUI;
    
    [SerializeField] float maxHealth = 100;
    [SerializeField] float chipSpeed = 2f;

    private float lerpTimer;
    private float currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        // Set Current Health to Max Health at the start of the game
        currentHealth = maxHealth;

        // Locate Enemy spawner Class
        enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    private void Update()
    {
        // Prevents healthbar from being below or 0 or above max health
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpateHealthUI();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        lerpTimer = 0f;

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
        animator.SetTrigger("Dead");

        // Instantiate Exp Object
        Instantiate(expObject, transform.position, Quaternion.identity);

        // Turn off Healthbar
        enemyUI.gameObject.SetActive(false);

        // Turn off Enemy Collider
        GetComponent<Collider2D>().enabled = false;

        // Destroy Enemy after a delay
        Destroy(gameObject, 5f);

        // Spawn another enemy
        enemySpawner.SpawnEnemy();
    }

    void UpateHealthUI()
    {
        float fillFront = frontHealthBar.fillAmount;
        float fillBack = backHealthBar.fillAmount;
        float healthFraction = currentHealth / maxHealth;

        if (fillBack > healthFraction)
        {
            frontHealthBar.fillAmount = healthFraction;
            backHealthBar.color = Color.white;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(fillBack, healthFraction, percentComplete);
        }

        if (fillFront < healthFraction)
        {
            backHealthBar.color = Color.green;
            backHealthBar.fillAmount = healthFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillFront, backHealthBar.fillAmount, percentComplete);
        }
    }
}
