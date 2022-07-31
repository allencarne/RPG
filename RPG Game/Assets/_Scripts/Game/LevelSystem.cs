using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSystem : MonoBehaviour
{
    public PlayerScriptableObject playerScriptableObject;

    [Header("UI")]
    public Image frontExperienceBar;
    public Image backExperienceBar;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI experienceText;
    [Header("Multipliers")]
    [Range(1f,300f)]
    public float additionMultiplier = 300;
    [Range(2f, 4f)]
    public float powerMultiplier = 2;
    [Range(7f, 14f)]
    public float divisionMultiplier = 7;

    //public int level;
    //public float currentExperience;
    //public float requiredExperience;

    private float lerpTimer;
    private float delayTimer;

    // Start is called before the first frame update
    void Start()
    {
        frontExperienceBar.fillAmount = playerScriptableObject.currentExperience / playerScriptableObject.requiredExperience;
        backExperienceBar.fillAmount = playerScriptableObject.currentExperience / playerScriptableObject.requiredExperience;
        playerScriptableObject.requiredExperience = CalculateRequiredExperience();
        levelText.text = playerScriptableObject.level.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateExperienceUI();
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            GainExperience(20);
        }

        if (playerScriptableObject.currentExperience >= playerScriptableObject.requiredExperience)
        {
            LevelUp();
        }
    }

    public void UpdateExperienceUI()
    {
        float experienceFraction = playerScriptableObject.currentExperience / playerScriptableObject.requiredExperience;
        float frontExperienceBarFill = frontExperienceBar.fillAmount;
        if (frontExperienceBarFill < experienceFraction)
        {
            delayTimer += Time.deltaTime;
            backExperienceBar.fillAmount = experienceFraction;
            if (delayTimer > 0.5)
            {
                lerpTimer += Time.deltaTime;
                float percentComplete = lerpTimer / 4f;
                frontExperienceBar.fillAmount = Mathf.Lerp(frontExperienceBarFill, backExperienceBar.fillAmount, percentComplete);
            }
        }

        experienceText.text = playerScriptableObject.currentExperience + "/" + playerScriptableObject.requiredExperience;
    }

    public void GainExperience(float experienceGained)
    {
        playerScriptableObject.currentExperience += experienceGained;
        lerpTimer = 0f;
        delayTimer = 0f;
    }

    public void LevelUp()
    {
        playerScriptableObject.level++;
        frontExperienceBar.fillAmount = 0;
        backExperienceBar.fillAmount = 0;
        playerScriptableObject.currentExperience = Mathf.RoundToInt(playerScriptableObject.currentExperience - playerScriptableObject.requiredExperience);
        GetComponent<Player>().IncreaseHealth(playerScriptableObject.level);
        playerScriptableObject.requiredExperience = CalculateRequiredExperience();
        levelText.text = playerScriptableObject.level.ToString();
    }

    private int CalculateRequiredExperience()
    {
        int solveForRequiredExperience = 0;
        for (int levelCycle = 1; levelCycle <= playerScriptableObject.level; levelCycle++)
        {
            solveForRequiredExperience += (int)Mathf.Floor(levelCycle + additionMultiplier * Mathf.Pow(powerMultiplier, levelCycle / divisionMultiplier));
        }
        return solveForRequiredExperience / 4;
    }
}