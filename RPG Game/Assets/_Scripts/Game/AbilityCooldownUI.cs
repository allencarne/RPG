using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityCooldownUI : MonoBehaviour
{
    [SerializeField] PlayerScriptableObject playerScriptableObject;

    [Header("Basic Attack")]
    [SerializeField] Image basicAttackAbilityImageCooldown;
    [SerializeField] TMP_Text basicAttackAbilityTextCooldown;
    [HideInInspector] bool basicAttackIsCooldown = false;
    [HideInInspector] float basicAttackCooldownTimer = 0.0f;

    [Header("Dash")]
    [SerializeField] Image dashAbilityImageCooldown;
    [SerializeField] TMP_Text dashAbilityTextCooldown;
    [HideInInspector] bool dashIsCooldown = false;
    [HideInInspector] float dashCooldownTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        basicAttackAbilityTextCooldown.gameObject.SetActive(false);
        basicAttackAbilityImageCooldown.fillAmount = 0.0f;

        dashAbilityTextCooldown.gameObject.SetActive(false);
        dashAbilityImageCooldown.fillAmount = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (basicAttackIsCooldown)
        {
            ApplyBasicAttackCoolDown();
        }

        if (dashIsCooldown)
        {
            ApplyDashCoolDown();
        }
    }

    void ApplyBasicAttackCoolDown()
    {
        basicAttackCooldownTimer -= Time.deltaTime;

        if (basicAttackCooldownTimer < 0.0f)
        {
            basicAttackIsCooldown = false;
            basicAttackAbilityTextCooldown.gameObject.SetActive(false);
            basicAttackAbilityImageCooldown.fillAmount = 0.0f;
        }
        else
        {
            //abilityTextCooldown.text = Mathf.RoundToInt(cooldownTimer).ToString();
            basicAttackAbilityTextCooldown.text = basicAttackCooldownTimer.ToString();
            basicAttackAbilityImageCooldown.fillAmount = basicAttackCooldownTimer / playerScriptableObject.weapon.basicAttackCoolDown;
        }
    }

    void ApplyDashCoolDown()
    {
        dashCooldownTimer -= Time.deltaTime;

        if (dashCooldownTimer < 0.0f)
        {
            dashIsCooldown = false;
            dashAbilityTextCooldown.gameObject.SetActive(false);
            dashAbilityImageCooldown.fillAmount = 0.0f;
        }
        else
        {
            dashAbilityTextCooldown.text = dashCooldownTimer.ToString();
            dashAbilityImageCooldown.fillAmount = dashCooldownTimer / playerScriptableObject.weapon.dashCoolDown;
        }
    }

    public void UseBasicAttackAbility()
    {
        if (basicAttackIsCooldown)
        {
            // User has click ability while in use
        }
        else
        {
            basicAttackIsCooldown = true;
            basicAttackAbilityTextCooldown.gameObject.SetActive(true);
            basicAttackCooldownTimer = playerScriptableObject.weapon.basicAttackCoolDown;
        }
    }

    public void UseDashAbility()
    {
        if (dashIsCooldown)
        {
            // User has click ability while in use
        }
        else
        {
            dashIsCooldown = true;
            dashAbilityTextCooldown.gameObject.SetActive(true);
            dashCooldownTimer = playerScriptableObject.weapon.dashCoolDown;
        }
    }
}
