using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSystem : MonoBehaviour
{
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

    public int level;
    public float currentExperience;
    public float requiredExperience;

    private float lerpTimer;
    private float delayTimer;

    // Start is called before the first frame update
    void Start()
    {
        frontExperienceBar.fillAmount = currentExperience / requiredExperience;
        backExperienceBar.fillAmount = currentExperience / requiredExperience;
        requiredExperience = CalculateRequiredExperience();
        levelText.text = level.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateExperienceUI();
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            GainExperience(20);
        }

        if (currentExperience >= requiredExperience)
        {
            LevelUp();
        }
    }

    public void UpdateExperienceUI()
    {
        float experienceFraction = currentExperience / requiredExperience;
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

        experienceText.text = currentExperience + "/" + requiredExperience;
    }

    public void GainExperience(float experienceGained)
    {
        currentExperience += experienceGained;
        lerpTimer = 0f;
        delayTimer = 0f;
    }

    public void LevelUp()
    {
        level++;
        frontExperienceBar.fillAmount = 0;
        backExperienceBar.fillAmount = 0;
        currentExperience = Mathf.RoundToInt(currentExperience - requiredExperience);
        GetComponent<Player>().IncreaseHealth(level);
        requiredExperience = CalculateRequiredExperience();
        levelText.text = level.ToString();
    }

    private int CalculateRequiredExperience()
    {
        int solveForRequiredExperience = 0;
        for (int levelCycle = 1; levelCycle <= level; levelCycle++)
        {
            solveForRequiredExperience += (int)Mathf.Floor(levelCycle + additionMultiplier * Mathf.Pow(powerMultiplier, levelCycle / divisionMultiplier));
        }
        return solveForRequiredExperience / 4;
    }
}