using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthbar : MonoBehaviour
{
    [SerializeField] EnemyScriptableObject enemyScriptableObject;

    [Header("HealthBar")]
    public Image frontHealthBar;
    public Image backHealthBar;
    public Canvas enemyUI;
    [HideInInspector] public float chipSpeed = 2f;
    [HideInInspector] public float lerpTimer;

    public void Update()
    {
        // Set Current Health to Max Health at the start of the game
        enemyScriptableObject.health = enemyScriptableObject.maxHealth;

        // Prevents healthbar from being below or 0 or above max health
        enemyScriptableObject.health = Mathf.Clamp(enemyScriptableObject.health, 0, enemyScriptableObject.maxHealth);

        UpateHealthUI();
    }

    void UpateHealthUI()
    {
        float fillFront = frontHealthBar.fillAmount;
        float fillBack = backHealthBar.fillAmount;
        float healthFraction = enemyScriptableObject.health / enemyScriptableObject.maxHealth;

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
