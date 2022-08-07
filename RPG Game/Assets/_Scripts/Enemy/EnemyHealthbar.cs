using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthbar : MonoBehaviour
{
    //[SerializeField] EnemyScriptableObject enemyScriptableObject;
    Enemy enemy;

    [Header("HealthBar")]
    [SerializeField] Image frontHealthBar;
    [SerializeField] Image backHealthBar;
    [SerializeField] public Canvas enemyUI;
    [HideInInspector] public float chipSpeed = 2f;
    [HideInInspector] public float lerpTimer;

    public void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    public void Start()
    {
        // Set Current Health to Max Health at the start of the game
        enemy.health = enemy.maxHealth;
    }

    public void Update()
    {

        // Prevents healthbar from being below or 0 or above max health
        enemy.health = Mathf.Clamp(enemy.health, 0, enemy.maxHealth);

        UpateHealthUI();
    }

    void UpateHealthUI()
    {
        float fillFront = frontHealthBar.fillAmount;
        float fillBack = backHealthBar.fillAmount;
        float healthFraction = enemy.health / enemy.maxHealth;

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
