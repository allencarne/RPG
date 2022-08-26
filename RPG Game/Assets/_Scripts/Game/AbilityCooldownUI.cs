using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityCooldownUI : MonoBehaviour
{
    [SerializeField] PlayerScriptableObject playerScriptableObject;

    [Header("Basic Attack")]
    [SerializeField] Button basicAttackAbilityImage;
    [SerializeField] Image basicAttackAbilityImageCooldown;
    [SerializeField] TMP_Text basicAttackAbilityTextCooldown;
    [HideInInspector] bool basicAttackIsCooldown = false;
    [HideInInspector] float basicAttackCooldownTimer = 0.0f;

    [Header("Basic Attack 2")]
    [SerializeField] Button basicAttack2AbilityImage;
    [SerializeField] Image basicAttack2AbilityImageCooldown;
    [SerializeField] TMP_Text basicAttack2AbilityTextCooldown;
    [HideInInspector] bool basicAttack2IsCooldown = false;
    [HideInInspector] float basicAttack2CooldownTimer = 0.0f;

    [Header("Basic Attack 3")]
    [SerializeField] Button basicAttack3AbilityImage;
    [SerializeField] Image basicAttack3AbilityImageCooldown;
    [SerializeField] TMP_Text basicAttack3AbilityTextCooldown;
    [HideInInspector] bool basicAttack3IsCooldown = false;
    [HideInInspector] float basicAttack3CooldownTimer = 0.0f;

    [Header("Dash")]
    [SerializeField] Image dashAbilityImageCooldown;
    [SerializeField] TMP_Text dashAbilityTextCooldown;
    [HideInInspector] bool dashIsCooldown = false;
    [HideInInspector] float dashCooldownTimer = 0.0f;

    [Header("Ability")]
    [SerializeField] Image abilityImageCooldown;
    [SerializeField] TMP_Text abilityTextCooldown;
    [HideInInspector] bool abilityIsCooldown = false;
    [HideInInspector] float abilityCooldownTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        // Basic Attack cooldown false
        basicAttackAbilityTextCooldown.gameObject.SetActive(false);
        basicAttackAbilityImageCooldown.fillAmount = 0.0f;

        // Basic Attack cooldown false
        basicAttack2AbilityTextCooldown.gameObject.SetActive(false);
        basicAttack2AbilityImageCooldown.fillAmount = 0.0f;

        // Basic Attack cooldown false
        basicAttack3AbilityTextCooldown.gameObject.SetActive(false);
        basicAttack3AbilityImageCooldown.fillAmount = 0.0f;

        // Dash cooldown false
        dashAbilityTextCooldown.gameObject.SetActive(false);
        dashAbilityImageCooldown.fillAmount = 0.0f;

        // Ability cooldown false
        abilityTextCooldown.gameObject.SetActive(false);
        abilityImageCooldown.fillAmount = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (basicAttackIsCooldown)
        {
            ApplyBasicAttackCoolDown();
        }

        if (basicAttack2IsCooldown)
        {
            ApplyBasicAttack2CoolDown();
        }

        if (basicAttack3IsCooldown)
        {
            ApplyBasicAttack3CoolDown();
        }

        if (dashIsCooldown)
        {
            ApplyDashCoolDown();
        }

        if (abilityIsCooldown)
        {
            ApplyAbilityCoolDown();
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

    void ApplyBasicAttack2CoolDown()
    {
        basicAttack2CooldownTimer -= Time.deltaTime;

        if (basicAttack2CooldownTimer < 0.0f)
        {
            basicAttack2IsCooldown = false;
            basicAttack2AbilityTextCooldown.gameObject.SetActive(false);
            basicAttack2AbilityImageCooldown.fillAmount = 0.0f;
        }
        else
        {
            //abilityTextCooldown.text = Mathf.RoundToInt(cooldownTimer).ToString();
            basicAttack2AbilityTextCooldown.text = basicAttack2CooldownTimer.ToString();
            basicAttack2AbilityImageCooldown.fillAmount = basicAttack2CooldownTimer / playerScriptableObject.weapon.basicAttackCoolDown;
        }
    }

    void ApplyBasicAttack3CoolDown()
    {
        basicAttack3CooldownTimer -= Time.deltaTime;

        if (basicAttack3CooldownTimer < 0.0f)
        {
            basicAttack3IsCooldown = false;
            basicAttack3AbilityTextCooldown.gameObject.SetActive(false);
            basicAttack3AbilityImageCooldown.fillAmount = 0.0f;
            basicAttack3AbilityImage.gameObject.SetActive(false);
            basicAttackAbilityImage.gameObject.SetActive(true);
        }
        else
        {
            //abilityTextCooldown.text = Mathf.RoundToInt(cooldownTimer).ToString();
            basicAttack3AbilityTextCooldown.text = basicAttack3CooldownTimer.ToString();
            basicAttack3AbilityImageCooldown.fillAmount = basicAttack3CooldownTimer / playerScriptableObject.weapon.basicAttackCoolDown;
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

    void ApplyAbilityCoolDown()
    {
        abilityCooldownTimer -= Time.deltaTime;

        if (abilityCooldownTimer < 0.0f)
        {
            abilityIsCooldown = false;
            abilityTextCooldown.gameObject.SetActive(false);
            abilityImageCooldown.fillAmount = 0.0f;
        }
        else
        {
            abilityTextCooldown.text = abilityCooldownTimer.ToString();
            abilityImageCooldown.fillAmount = abilityCooldownTimer / playerScriptableObject.weapon.abilityCoolDown;
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
            basicAttackAbilityImage.gameObject.SetActive(true);
            basicAttack2AbilityImage.gameObject.SetActive(false);
            basicAttackAbilityTextCooldown.gameObject.SetActive(true);
            basicAttackCooldownTimer = playerScriptableObject.weapon.basicAttackCoolDown;
        }
    }

    public void UseBasicAttack2Ability()
    {

        if (basicAttack2IsCooldown)
        {
            // User has click ability while in use
        }
        else
        {
            basicAttack2IsCooldown = true;
            basicAttackAbilityImage.gameObject.SetActive(false);
            basicAttack2AbilityImage.gameObject.SetActive(true);
            basicAttack2AbilityTextCooldown.gameObject.SetActive(true);
            basicAttack2CooldownTimer = playerScriptableObject.weapon.basicAttackCoolDown;
        }
    }

    public void UseBasicAttack3Ability()
    {

        if (basicAttack3IsCooldown)
        {
            // User has click ability while in use
        }
        else
        {
            basicAttack3IsCooldown = true;
            basicAttack2AbilityImage.gameObject.SetActive(false);
            basicAttack3AbilityImage.gameObject.SetActive(true);
            basicAttack3AbilityTextCooldown.gameObject.SetActive(true);
            basicAttack3CooldownTimer = playerScriptableObject.weapon.basicAttackCoolDown;
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

    public void UseAbility()
    {
        if (abilityIsCooldown)
        {
            // User has click ability while in use
        }
        else
        {
            abilityIsCooldown = true;
            abilityTextCooldown.gameObject.SetActive(true);
            abilityCooldownTimer = playerScriptableObject.weapon.abilityCoolDown;
        }
    }
}
