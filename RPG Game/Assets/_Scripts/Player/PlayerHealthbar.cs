using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthbar : MonoBehaviour
{
    [SerializeField] PlayerScriptableObject playerScriptableObject;

    [Header("HealthBar")]
    [SerializeField] Image frontHealthBar;
    [SerializeField] Image backHealthbar;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI nameText;
    [HideInInspector] public float chipSpeed = 2f;
    [HideInInspector] public float lerpTimer;

    // Start is called before the first frame update
    void Start()
    {
        // Set Current Player Health to Max Health
        playerScriptableObject.health = playerScriptableObject.maxHealth;
        nameText.text = playerScriptableObject.name.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        // Healthbar UI Update
        playerScriptableObject.health = Mathf.Clamp(playerScriptableObject.health, 0, playerScriptableObject.maxHealth);
        UpdateHealthUI();
    }

    public void UpdateHealthUI()
    {
        float fillFront = frontHealthBar.fillAmount;
        float fillBack = backHealthbar.fillAmount;
        float healthFraction = playerScriptableObject.health / playerScriptableObject.maxHealth;

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

        healthText.text = Mathf.Round(playerScriptableObject.health) + "/" + Mathf.Round(playerScriptableObject.maxHealth);
    }
}
