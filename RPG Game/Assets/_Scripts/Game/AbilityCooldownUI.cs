using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityCooldownUI : MonoBehaviour
{
    [SerializeField] Image abilityImageCooldown;
    [SerializeField] TMP_Text abilityTextCooldown;
    [SerializeField] PlayerScriptableObject playerScriptableObject;

    [HideInInspector] bool isCooldown = false;
    [HideInInspector] float cooldownTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        abilityTextCooldown.gameObject.SetActive(false);
        abilityImageCooldown.fillAmount = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isCooldown)
        {
            ApplyCoolDown();
        }
    }

    void ApplyCoolDown()
    {
        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer < 0.0f)
        {
            isCooldown = false;
            abilityTextCooldown.gameObject.SetActive(false);
            abilityImageCooldown.fillAmount = 0.0f;
        }
        else
        {
            //abilityTextCooldown.text = Mathf.RoundToInt(cooldownTimer).ToString();
            abilityTextCooldown.text = cooldownTimer.ToString();
            abilityImageCooldown.fillAmount = cooldownTimer / playerScriptableObject.weapon.leftMouse1CoolDown;
        }
    }

    public void UseAbility()
    {
        if (isCooldown)
        {
            // User has click ability while in use
        }
        else
        {
            isCooldown = true;
            abilityTextCooldown.gameObject.SetActive(true);
            cooldownTimer = playerScriptableObject.weapon.leftMouse1CoolDown;
        }
    }
}
