using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float maxHealth;
    private float currentHealth;
    public float chipSpeed = 2f;
    private float lerpTimer;
    public Image frontHealthBar;
    public Image backHealthbar;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }
     void Update()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();

        if (Input.GetKeyDown(KeyCode.X))
        {
            PlayerTakeDamage(5f);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            RestoreHealth(5f);
        }
    }

    public void UpdateHealthUI()
    {
        float fillFront = frontHealthBar.fillAmount;
        float fillBack = backHealthbar.fillAmount;
        float healthFraction = currentHealth / maxHealth;

        if (fillBack > healthFraction)
        {
            frontHealthBar.fillAmount = healthFraction;
            backHealthbar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            backHealthbar.fillAmount = Mathf.Lerp(fillBack, healthFraction, percentComplete);
        }

        if (fillFront < healthFraction)
        {
            backHealthbar.color = Color.green;
            backHealthbar.fillAmount = healthFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillFront, backHealthbar.fillAmount, percentComplete);
        }
    }

    public void RestoreHealth (float healAmount)
    {
        currentHealth += healAmount;
        lerpTimer = 0f;
    }

    public void PlayerTakeDamage(float damage)
    {
        currentHealth -= damage;
        lerpTimer = 0f;

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
