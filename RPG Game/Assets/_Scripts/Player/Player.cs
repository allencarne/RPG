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
    Vector2 movement;
    bool isAttacking;
    bool isDashing;
    bool isAbilityActive;
    bool canAttack2 = false;
    bool canAttack3 = false;
    float lastAttack; // Variable to help with Attack Cooldown
    float lastDash; // Variable to help with Dash Cooldown

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

    enum PlayerState
    {
        idle,
        move,
        attack,
        attack2,
        attack3,
        dash,
        ability,
        hit,
        death
    }

    PlayerState state = PlayerState.idle;

    private void Awake()
    {
        abilityCooldownUI = abilitiesUI.GetComponent<AbilityCooldownUI>();
        cam = Camera.main;
        playerHealthbar = GetComponent<PlayerHealthbar>();
        //animator.speed = 1.5f;
    }

     void Update()
    {
        Debug.Log(state);

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
            case PlayerState.dash:
                PlayerDashState();
                break;
            case PlayerState.ability:
                PlayerAbilityState();
                break;
            case PlayerState.hit:
                PlayerHitState(damage);
                break;
            case PlayerState.death:
                PlayerDeathState();
                break;
        }

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

        AbilityKeyPressed();
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

        // Input
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        movement = moveInput.normalized * playerScriptableObject.speed;

        // Movement
        rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);

        NoMoveKeyPressed();

        DashKeyPressed();

        AttackKeyPressed();
    }

    public void PlayerAttackState()
    {
        switch (playerScriptableObject.weapon.weaponIndex)
        {
            case 0:
                BasicAttackAbility();
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
                BasicAttackAbility2();
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
                BasicAttackAbility3();
                abilityCooldownUI.UseBasicAttack3Ability();
                break;
            case 1:
                break;
        }
    }

    public void PlayerDashState()
    {
        abilityCooldownUI.UseDashAbility();

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

    public void PlayerAbilityState()
    {
        //Animate
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

            // Set AttackAnglePause Bool to True
            attackAnglePaused = true;

            if (!isAbilityActive)
            {
                // Instantiate Slash prefab
                GameObject slash = Instantiate(playerScriptableObject.weapon.abilityPrefab, firePointRanged.position, firePointRanged.rotation);

                // Get the Rigid Body of the Slash prefab
                Rigidbody2D slashRB = slash.GetComponent<Rigidbody2D>();

                // Add Force to Slash prefab
                slashRB.AddForce(-firePoint.up * playerScriptableObject.weapon.abilityProjectileForce, ForceMode2D.Impulse);

                // Reset isAttacking Bool;
                //isAbilityActive = false;
            }
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
    public void BasicAttackAbility()
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

    public void BasicAttackAbility2()
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

    public void BasicAttackAbility3()
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

    //===== Animation Events =====\\
    public void AE_Attack()
    {
        isAttacking = true;
    }

    public void AE_Attack2()
    {
        canAttack2 = true;
    }

    public void AE_Attack3()
    {
        canAttack3 = true;
    }

    public void AE_AttackAnimationEnd()
    {
        attackAnglePaused = false;
        canAttack2 = false;
        canAttack3 = false;
        state = PlayerState.idle;
    }

    public void AE_HitAnimationEnd()
    {
        state = PlayerState.idle;
    }

    public void AE_DashParticle()
    {
        isDashing = true;
    }

    //===== Health =====\\
    public void RestoreHealth (float healAmount)
    {
        playerScriptableObject.health += healAmount;
        playerHealthbar.lerpTimer = 0f;
    }

    public void IncreaseHealth(int level)
    {
        playerScriptableObject.maxHealth += (playerScriptableObject.health * 0.01f) * ((100 - level) * 0.01f);
        playerScriptableObject.health = playerScriptableObject.maxHealth;
    }

    //===== Input =====\\
    public void MoveKeyPressed()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            state = PlayerState.move;
        }
    }

    public void NoMoveKeyPressed()
    {
        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
        {
            state = PlayerState.idle;
        }
    }

    public void AttackKeyPressed()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time - lastAttack < playerScriptableObject.weapon.basicAttackCoolDown)
            {
                return;
            }
            lastAttack = Time.time;
            state = PlayerState.attack;
        }
    }

    public void DashKeyPressed()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (Time.time - lastDash < playerScriptableObject.weapon.dashCoolDown)
            {
                return;
            }
            lastDash = Time.time;
            state = PlayerState.dash;
        }
    }

    public void AbilityKeyPressed()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (Time.time - lastAttack < playerScriptableObject.weapon.basicAttackCoolDown)
            {
                return;
            }
            lastAttack = Time.time;
            state = PlayerState.ability;
        }
    }
}
