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

    [Header("Ability 1")]
    [SerializeField] Image ability1ImageCooldown;
    [SerializeField] TMP_Text ability1TextCooldown;
    [HideInInspector] bool ability1IsCooldown = false;
    [HideInInspector] float ability1CooldownTimer = 0.0f;

    [Header("Dash")]
    [SerializeField] Image dashAbilityImageCooldown;
    [SerializeField] TMP_Text dashAbilityTextCooldown;
    [HideInInspector] bool dashIsCooldown = false;
    [HideInInspector] float dashCooldownTimer = 0.0f;

    [Header("Ability 2")]
    [SerializeField] Image ability2ImageCooldown;
    [SerializeField] TMP_Text ability2TextCooldown;
    [HideInInspector] bool ability2IsCooldown = false;
    [HideInInspector] float ability2CooldownTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        // Basic Attack 1 cooldown false
        basicAttackAbilityTextCooldown.gameObject.SetActive(false);
        basicAttackAbilityImageCooldown.fillAmount = 0.0f;

        // Basic Attack 2 cooldown false
        basicAttack2AbilityTextCooldown.gameObject.SetActive(false);
        basicAttack2AbilityImageCooldown.fillAmount = 0.0f;

        // Basic Attack 3 cooldown false
        basicAttack3AbilityTextCooldown.gameObject.SetActive(false);
        basicAttack3AbilityImageCooldown.fillAmount = 0.0f;

        // Ability1 cooldown false
        ability1TextCooldown.gameObject.SetActive(false);
        ability1ImageCooldown.fillAmount = 0.0f;

        // Dash cooldown false
        dashAbilityTextCooldown.gameObject.SetActive(false);
        dashAbilityImageCooldown.fillAmount = 0.0f;

        // Ability2 cooldown false
        ability2TextCooldown.gameObject.SetActive(false);
        ability2ImageCooldown.fillAmount = 0.0f;
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

        if (ability1IsCooldown)
        {
            ApplyAbility1CoolDown();
        }

        if (dashIsCooldown)
        {
            ApplyDashCoolDown();
        }

        if (ability2IsCooldown)
        {
            ApplyAbility2CoolDown();
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

    void ApplyAbility1CoolDown()
    {
        ability1CooldownTimer -= Time.deltaTime;

        if (ability1CooldownTimer < 0.0f)
        {
            ability1IsCooldown = false;
            ability1TextCooldown.gameObject.SetActive(false);
            ability1ImageCooldown.fillAmount = 0.0f;
        }
        else
        {
            ability1TextCooldown.text = ability1CooldownTimer.ToString();
            ability1ImageCooldown.fillAmount = ability1CooldownTimer / playerScriptableObject.weapon.ability1CoolDown;
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

    void ApplyAbility2CoolDown()
    {
        ability2CooldownTimer -= Time.deltaTime;

        if (ability2CooldownTimer < 0.0f)
        {
            ability2IsCooldown = false;
            ability2TextCooldown.gameObject.SetActive(false);
            ability2ImageCooldown.fillAmount = 0.0f;
        }
        else
        {
            ability2TextCooldown.text = ability1CooldownTimer.ToString();
            ability2ImageCooldown.fillAmount = ability2CooldownTimer / playerScriptableObject.weapon.ability2CoolDown;
        }
    }

    // Use

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

    public void UseAbility1()
    {
        if (ability1IsCooldown)
        {
            // User has click ability while in use
        }
        else
        {
            ability1IsCooldown = true;
            ability1TextCooldown.gameObject.SetActive(true);
            ability1CooldownTimer = playerScriptableObject.weapon.ability1CoolDown;
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

    public void UseAbility2()
    {
        if (ability2IsCooldown)
        {
            // User has click ability while in use
        }
        else
        {
            ability2IsCooldown = true;
            ability2TextCooldown.gameObject.SetActive(true);
            ability2CooldownTimer = playerScriptableObject.weapon.ability2CoolDown;
        }
    }
}
