using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    [Header("Variables")]
    [HideInInspector] public bool attackAnglePaused = false;
    [HideInInspector] public float damage; // Temporary
    [HideInInspector] Vector2 movement;

    bool isAttacking;
    bool isDashing;
    bool isAbilityActive;
    bool isAbility2Active;
    bool isWindPullBaseActive;
    bool isUltimateActive;
    bool canAttack2 = false;
    bool canAttack3 = false;

    float lastAttack; // Variable to help with Attack Cooldown
    float lastDash; // Variable to help with Dash Cooldown
    float lastAbility; // Variable to help with Ability Cooldown
    float lastAbility2;
    float lastUltimate;

    [Header("Components")]
    [SerializeField] PlayerScriptableObject playerScriptableObject;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Transform firePoint;
    [SerializeField] Transform firePointRanged;
    [SerializeField] GameObject abilitiesUI;
    PlayerHealthbar playerHealthbar;
    AbilityCooldownUI abilityCooldownUI;
    Camera cam;

    //Buff
    [SerializeField] GameObject tempestsFuryBuff;

    [Header("Keys")]
    public KeyCode moveUpKey;
    public KeyCode moveDownKey;
    public KeyCode moveLeftKey;
    public KeyCode moveRightKey;

    public KeyCode basicAttackKey;
    public KeyCode ability1Key;
    public KeyCode dashKey;
    public KeyCode ability2Key;
    public KeyCode UltimateKey;
    

    enum PlayerState
    {
        idle,
        move,
        attack,
        attack2,
        attack3,
        ability1,
        dash,
        ability2,
        ultimate,
        hit,
        death
    }

    PlayerState state = PlayerState.idle;

    private void Awake()
    {
        abilityCooldownUI = abilitiesUI.GetComponent<AbilityCooldownUI>();
        cam = Camera.main;
        playerHealthbar = GetComponent<PlayerHealthbar>();
        //animator.speed = 1.3f;
    }

     void Update()
    {
        //Debug.Log(isAbilityActive);

        switch (state)
        {
            case PlayerState.idle:
                PlayerIdleState();
                break;
            case PlayerState.move:
                PlayerMoveState();
                break;
            case PlayerState.attack:
                PlayerAttackState();
                break;
            case PlayerState.attack2:
                PlayerAttack2State();
                break;
            case PlayerState.attack3:
                PlayerAttack3State();
                break;
            case PlayerState.ability1:
                PlayerAbility1State();
                break;
            case PlayerState.dash:
                PlayerDashState();
                break;
            case PlayerState.ability2:
                PlayerAbility2State();
                break;
            case PlayerState.ultimate:
                PlayerUltimateState();
                break;
            case PlayerState.hit:
                PlayerHitState(damage);
                break;
            case PlayerState.death:
                PlayerDeathState();
                break;
        }

        // Testing
        if (Input.GetKeyDown(KeyCode.X))
        {
            PlayerHitState(5f);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            RestoreHealth(5f);
        }
    }

    //===== States =====\\
    public void PlayerIdleState()
    {
        // Animate
        animator.Play("Idle");

        MoveKeyPressed();

        DashKeyPressed();

        AttackKeyPressed();

        Ability1KeyPressed();

        Ability2KeyPressed();

        UltimateKeyPressed();
    }

    public void PlayerMoveState()
    {

        // Animate
        animator.Play("Move");

        // Set idle Animation after move
        if (movement != Vector2.zero)
        {
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
        }
        animator.SetFloat("Speed", movement.sqrMagnitude);

        MovePlayer();

        // Transitions
        NoMoveKeyPressed();

        DashKeyPressed();

        AttackKeyPressed();

        Ability1KeyPressed();

        Ability2KeyPressed();

        UltimateKeyPressed();
    }

    public void PlayerAttackState()
    {
        switch (playerScriptableObject.weapon.weaponIndex)
        {
            case 0:
                WindSlash();
                abilityCooldownUI.UseBasicAttackAbility();
                break;
            case 1:
                break;
        }
    }
    
    public void PlayerAttack2State()
    {
        switch (playerScriptableObject.weapon.weaponIndex)
        {
            case 0:
                WindSlash2();
                abilityCooldownUI.UseBasicAttack2Ability();
                break;
            case 1:
                break;
        }
    }

    public void PlayerAttack3State()
    {
        switch (playerScriptableObject.weapon.weaponIndex)
        {
            case 0:
                WindSlash3();
                abilityCooldownUI.UseBasicAttack3Ability();
                break;
            case 1:
                break;
        }
    }

    public void PlayerAbility1State()
    {
        switch (playerScriptableObject.weapon.weaponIndex)
        {
            case 0:
                WindPull();
                abilityCooldownUI.UseAbility1();
                break;
            case 1:
                break;
        }
    }

    public void PlayerDashState()
    {
        switch (playerScriptableObject.weapon.weaponIndex)
        {
            case 0:
                WindDash();
                abilityCooldownUI.UseDashAbility();
                break;
            case 1:
                break;
        }
    }

    public void PlayerAbility2State()
    {
        switch (playerScriptableObject.weapon.weaponIndex)
        {
            case 0:
                Whirlwind();
                abilityCooldownUI.UseAbility2();
                break;
            case 1:
                break;
        }
    }

    public void PlayerUltimateState()
    {
        switch (playerScriptableObject.weapon.weaponIndex)
        {
            case 0:
                TempestsFury();
                //abilityCooldownUI.UseAbility2();
                break;
            case 1:
                break;
        }
    }

    public void PlayerHitState(float damage)
    {
        // Transition
        state = PlayerState.hit;

        // Animate
        animator.Play("Hit");

        // Prevents Bug - Makes sure that dash and attack always allows direction change and slide
        attackAnglePaused = false;

        // Logic
        playerScriptableObject.health -= damage;
        playerHealthbar.lerpTimer = 0f;

        if (playerScriptableObject.health <= 0)
        {
            PlayerDeathState();
        }
    }

    public void PlayerDeathState()
    {
        state = PlayerState.death;
        Destroy(gameObject);
    }

    //===== Abilities =====\\
    public void WindSlash()
    {
        // Animate
        animator.Play("Attack");

        // Calculate the difference between mouse position and player position
        Vector2 difference = cam.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        // if attack angle is not paused - Pause Attack Angle and Animate in that direction - And slide forward
        if (!attackAnglePaused)
        {
            // Set Attack Animation Depending on Mouse Position
            animator.SetFloat("Aim Horizontal", difference.x);
            animator.SetFloat("Aim Vertical", difference.y);
            // Set Idle to last attack position
            animator.SetFloat("Horizontal", difference.x);
            animator.SetFloat("Vertical", difference.y);

            // If mouse is outside attackrange - Slide Forward
            if (Vector3.Distance(rb.position, cam.ScreenToWorldPoint(Input.mousePosition)) > playerScriptableObject.weapon.attackRange)
            {
                // Normalize movement vector and times it by attack move distance
                difference = difference.normalized * playerScriptableObject.weapon.basicAttackSlideVelocity;
                // Slide in Attack Direction
                rb.AddForce(difference, ForceMode2D.Impulse);
            }

            // Set AttackAnglePause Bool to True
            attackAnglePaused = true;
        }

        if (isAttacking)
        {
            // Instantiate Slash prefab
            GameObject slash = Instantiate(playerScriptableObject.weapon.basicAttackPrefab, firePoint.position, firePoint.rotation);

            // Get the Rigid Body of the Slash prefab
            Rigidbody2D slashRB = slash.GetComponent<Rigidbody2D>();

            // Add Force to Slash prefab
            slashRB.AddForce(firePoint.up * playerScriptableObject.weapon.basicAttackProjectileForce, ForceMode2D.Impulse);

            // Reset isAttacking Bool;
            isAttacking = false;
        }

        // Transition
        if (canAttack2)
        {
            if (Input.GetMouseButtonDown(0))
            {
                attackAnglePaused = false;
                state = PlayerState.attack2;
            }
        }
    }

    public void WindSlash2()
    {
        animator.Play("Attack 2");

        // Calculate the difference between mouse position and player position
        Vector2 difference = cam.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        // if attack angle is not paused - Pause Attack Angle and Animate in that direction - And slide forward
        if (!attackAnglePaused)
        {
            // Set Attack Animation Depending on Mouse Position
            animator.SetFloat("Aim Horizontal", difference.x);
            animator.SetFloat("Aim Vertical", difference.y);
            // Set Idle to last attack position
            animator.SetFloat("Horizontal", difference.x);
            animator.SetFloat("Vertical", difference.y);

            // If mouse is outside attackrange - Slide Forward
            if (Vector3.Distance(rb.transform.position, cam.ScreenToWorldPoint(Input.mousePosition)) > playerScriptableObject.weapon.attackRange)
            {
                // Normalize movement vector and times it by attack move distance
                difference = difference.normalized * playerScriptableObject.weapon.basicAttackSlideVelocity;
                // Slide in Attack Direction
                rb.AddForce(difference, ForceMode2D.Impulse);
            }

            // Set AttackAnglePause Bool to True
            attackAnglePaused = true;
        }

        if (isAttacking)
        {
            // Instantiate Slash prefab
            GameObject slash = Instantiate(playerScriptableObject.weapon.basicAttack2Prefab, firePoint.position, firePoint.rotation);

            // Get the Rigid Body of the Slash prefab
            Rigidbody2D slashRB = slash.GetComponent<Rigidbody2D>();

            // Add Force to Slash prefab
            slashRB.AddForce(firePoint.up * playerScriptableObject.weapon.basicAttackProjectileForce, ForceMode2D.Impulse);

            // Reset isAttacking Bool;
            isAttacking = false;
        }

        // Transition
        if (canAttack3)
        {
            if (Input.GetMouseButtonDown(0))
            {
                attackAnglePaused = false;
                state = PlayerState.attack3;
            }
        }
    }

    public void WindSlash3()
    {
        animator.Play("Attack 3");

        // Calculate the difference between mouse position and player position
        Vector2 difference = cam.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        // if attack angle is not paused - Pause Attack Angle and Animate in that direction - And slide forward
        if (!attackAnglePaused)
        {
            // Set Attack Animation Depending on Mouse Position
            animator.SetFloat("Aim Horizontal", difference.x);
            animator.SetFloat("Aim Vertical", difference.y);
            // Set Idle to last attack position
            animator.SetFloat("Horizontal", difference.x);
            animator.SetFloat("Vertical", difference.y);

            // If mouse is outside attackrange - Slide Forward
            if (Vector3.Distance(rb.transform.position, cam.ScreenToWorldPoint(Input.mousePosition)) > playerScriptableObject.weapon.attackRange)
            {
                // Normalize movement vector and times it by attack move distance
                difference = difference.normalized * playerScriptableObject.weapon.basicAttackSlideVelocity;
                // Slide in Attack Direction
                rb.AddForce(difference, ForceMode2D.Impulse);
            }

            // Set AttackAnglePause Bool to True
            attackAnglePaused = true;
        }

        if (isAttacking)
        {
            // Instantiate Slash prefab
            GameObject slash = Instantiate(playerScriptableObject.weapon.basicAttack3Prefab, firePoint.position, firePoint.rotation);

            // Get the Rigid Body of the Slash prefab
            Rigidbody2D slashRB = slash.GetComponent<Rigidbody2D>();

            // Add Force to Slash prefab
            slashRB.AddForce(firePoint.up * playerScriptableObject.weapon.basicAttackProjectileForce, ForceMode2D.Impulse);

            // Reset isAttacking Bool;
            isAttacking = false;
        }
    }

    public void WindPull()
    {
        //Animate
        animator.Play("Wind Pull");

        if (isWindPullBaseActive)
        {
            Instantiate(playerScriptableObject.weapon.ability1BasePrefab, firePoint.position, firePoint.rotation);

            isWindPullBaseActive = false;
        }

        // Calculate the difference between mouse position and player position
        Vector2 difference = cam.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        // if attack angle is not paused - Pause Attack Angle and Animate in that direction - And slide forward
        if (!attackAnglePaused)
        {
            // Set Attack Animation Depending on Mouse Position
            animator.SetFloat("Aim Horizontal", difference.x);
            animator.SetFloat("Aim Vertical", difference.y);
            // Set Idle to last attack position
            animator.SetFloat("Horizontal", difference.x);
            animator.SetFloat("Vertical", difference.y);

            // Set AttackAnglePause Bool to True
            attackAnglePaused = true;
        }

        if (isAbilityActive)
        {
            // Instantiate Slash prefab
            GameObject slash = Instantiate(playerScriptableObject.weapon.ability1Prefab, firePointRanged.position, firePointRanged.rotation);

            // Get the Rigid Body of the Slash prefab
            Rigidbody2D slashRB = slash.GetComponent<Rigidbody2D>();

            // Add Force to Slash prefab
            slashRB.AddForce(-firePoint.up * playerScriptableObject.weapon.ability1ProjectileForce, ForceMode2D.Impulse);

            // Reset isAttacking Bool;
            isAbilityActive = false;
        }
    }

    public void WindDash()
    {
        // Animate
        animator.Play("Dash");

        if (isDashing)
        {
            // Instatiate Particle Effect
            Instantiate(playerScriptableObject.weapon.dashParticle, rb.transform.position, firePoint.rotation);

            // Reset
            isDashing = false;
        }

        // Calculate the difference between mouse position and player position
        Vector2 difference = cam.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        if (!attackAnglePaused)
        {
            //Set Attack Animation Depending on Mouse Position
            animator.SetFloat("Horizontal", difference.x);
            animator.SetFloat("Vertical", difference.y);
            //Set Idle to last attack position
            animator.SetFloat("Horizontal", difference.x);
            animator.SetFloat("Vertical", difference.y);

            //Normalize movement vector and times it by attack move distance
            difference = difference.normalized * playerScriptableObject.weapon.dashVelocity;
            // Slide in Attack Direction
            rb.AddForce(difference, ForceMode2D.Impulse);

            // Set AttackAnglePause Bool to True
            attackAnglePaused = true;
        }

        if (isAttacking)
        {
            // Instantiate Slash prefab
            GameObject slash = Instantiate(playerScriptableObject.weapon.dashPrefab, firePoint.position, firePoint.rotation);

            // Get the Rigid Body of the Slash prefab
            Rigidbody2D slashRB = slash.GetComponent<Rigidbody2D>();

            // Add Force to Slash prefab
            slashRB.AddForce(firePoint.up * playerScriptableObject.weapon.dashProjectileForce, ForceMode2D.Impulse);

            // Reset isAttacking Bool;
            isAttacking = false;
        }
    }

    public void Whirlwind()
    {
        //Animate
        animator.Play("Whirlwind");

        // Calculate the difference between mouse position and player position
        Vector2 difference = cam.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        // if attack angle is not paused - Pause Attack Angle and Animate in that direction - And slide forward
        if (!attackAnglePaused)
        {
            // Set Attack Animation Depending on Mouse Position
            animator.SetFloat("Aim Horizontal", difference.x);
            animator.SetFloat("Aim Vertical", difference.y);
            // Set Idle to last attack position
            animator.SetFloat("Horizontal", difference.x);
            animator.SetFloat("Vertical", difference.y);

            // If mouse is outside attackrange - Slide Forward
            if (Vector3.Distance(rb.position, cam.ScreenToWorldPoint(Input.mousePosition)) > playerScriptableObject.weapon.attackRange)
            {
                // Normalize movement vector and times it by attack move distance
                difference = difference.normalized * playerScriptableObject.weapon.ability2SlideVelocity;
                // Slide in Attack Direction
                rb.AddForce(difference, ForceMode2D.Impulse);
            }

            // Set AttackAnglePause Bool to True
            attackAnglePaused = true;
        }

        if (isAbility2Active)
        {
            Instantiate(playerScriptableObject.weapon.ability2Prefab, transform.position, Quaternion.identity);

            isAbility2Active = false;
        }
    }

    public void TempestsFury()
    {
        //Animate
        animator.Play("Tempest's Fury");

        if (isUltimateActive)
        {
            StartCoroutine(UltimateDuration());

            //tempestsFuryBuff.SetActive(true);

            //playerScriptableObject.speed += 5;
            //Instantiate(playerScriptableObject.weapon.ultimatePrefab, transform.position, transform.rotation);

            isUltimateActive = false;
        }
    }

    IEnumerator UltimateDuration()
    {
        tempestsFuryBuff.SetActive(true);
        playerScriptableObject.speed += 5;

        yield return new WaitForSeconds(5);

        tempestsFuryBuff.SetActive(false);
        playerScriptableObject.speed -= 5;
    }

    //===== Animation Events =====\\
    public void AE_Attack()
    {
        isAttacking = true;
    }

    public void AE_AttackAnimationEnd()
    {
        attackAnglePaused = false;
        canAttack2 = false;
        canAttack3 = false;
        state = PlayerState.idle;
    }

    public void AE_Attack2()
    {
        canAttack2 = true;
    }

    public void AE_Attack3()
    {
        canAttack3 = true;
    }

    public void AE_WindPull()
    {
        isAbilityActive = true;
    }

    public void AE_WindPullBase()
    {
        isWindPullBaseActive = true;
    }

    public void AE_DashParticle()
    {
        isDashing = true;
    }

    public void AE_Whirlwind()
    {
        isAbility2Active = true;
    }

    public void AE_WhirlwindAnimationEnd()
    {
        attackAnglePaused = false;
        state = PlayerState.idle;
    }

    public void AE_HitAnimationEnd()
    {
        state = PlayerState.idle;
    }

    public void AE_TempestsFury()
    {
        isUltimateActive = true;
    }

    public void AE_TempestsFuryAnimationEnd()
    {
        state = PlayerState.idle;
    }

    //===== Input =====\\
    public void MovePlayer()
    {
        // Input
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        movement = moveInput.normalized * playerScriptableObject.speed;

        // Movement
        rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);
    }

    public void MoveKeyPressed()
    {
        if (Input.GetKey(moveUpKey) || Input.GetKey(moveLeftKey) || Input.GetKey(moveDownKey) || Input.GetKey(moveRightKey))
        {
            state = PlayerState.move;
        }
    }

    public void NoMoveKeyPressed()
    {
        if (!Input.GetKey(moveUpKey) && !Input.GetKey(moveLeftKey) && !Input.GetKey(moveDownKey) && !Input.GetKey(moveRightKey))
        {
            state = PlayerState.idle;
        }
    }

    public void AttackKeyPressed()
    {
        if (Input.GetKey(basicAttackKey))
        {
            if (Time.time - lastAttack < playerScriptableObject.weapon.basicAttackCoolDown)
            {
                return;
            }
            lastAttack = Time.time;
            state = PlayerState.attack;
        }
    }

    public void Ability1KeyPressed()
    {
        if (Input.GetKey(ability1Key))
        {
            if (Time.time - lastAbility < playerScriptableObject.weapon.ability1CoolDown)
            {
                return;
            }
            lastAbility = Time.time;
            state = PlayerState.ability1;
        }
    }

    public void DashKeyPressed()
    {
        if (Input.GetKey(dashKey))
        {
            if (Time.time - lastDash < playerScriptableObject.weapon.dashCoolDown)
            {
                return;
            }
            lastDash = Time.time;
            state = PlayerState.dash;
        }
    }

    public void Ability2KeyPressed()
    {
        if (Input.GetKey(ability2Key))
        {
            if (Time.time - lastAbility2 < playerScriptableObject.weapon.ability2CoolDown)
            {
                return;
            }
            lastAbility2 = Time.time;
            state = PlayerState.ability2;
        }
    }

    public void UltimateKeyPressed()
    {
        if (Input.GetKey(UltimateKey))
        {
            if (Time.time - lastUltimate < playerScriptableObject.weapon.ultimateCoolDown)
            {
                return;
            }
            lastUltimate = Time.time;
            state = PlayerState.ultimate;
        }
    }

    //===== Health =====\\
    public void RestoreHealth(float healAmount)
    {
        playerScriptableObject.health += healAmount;
        playerHealthbar.lerpTimer = 0f;
    }

    public void IncreaseHealth(int level)
    {
        playerScriptableObject.maxHealth += (playerScriptableObject.health * 0.01f) * ((100 - level) * 0.01f);
        playerScriptableObject.health = playerScriptableObject.maxHealth;
    }
}
