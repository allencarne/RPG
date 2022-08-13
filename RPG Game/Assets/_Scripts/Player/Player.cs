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
    bool canAttack2 = false;
    float lastAttack; // Variable to help with Attack Cooldown
    float lastDash; // Variable to help with Dash Cooldown

    [Header("Components")]
    [SerializeField] PlayerScriptableObject playerScriptableObject;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject abilities;
    PlayerHealthbar playerHealthbar;
    AbilityCooldownUI abilityCooldownUI;
    Camera cam;

    enum PlayerState
    {
        idle,
        move,
        attack,
        attack2,
        dash,
        hit,
        death
    }

    PlayerState state = PlayerState.idle;

    private void Awake()
    {
        abilityCooldownUI = abilities.GetComponent<AbilityCooldownUI>();
        cam = Camera.main;
        playerHealthbar = GetComponent<PlayerHealthbar>();
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
            case PlayerState.dash:
                PlayerDashState();
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

    public void PlayerIdleState()
    {
        // Animate
        animator.Play("Idle");

        MoveKeyPressed();

        DashKeyPressed();

        AttackKeyPressed();
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
                //abilityCooldownUI.UseBasicAttackAbility();
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
            difference = difference.normalized * playerScriptableObject.weapon.spaceVelocity;
            // Slide in Attack Direction
            rb.AddForce(difference, ForceMode2D.Impulse);

            // Set AttackAnglePause Bool to True
            attackAnglePaused = true;
        }

        if (isAttacking)
        {
            // Instantiate Slash prefab
            GameObject slash = Instantiate(playerScriptableObject.weapon.spacePrefab, firePoint.position, firePoint.rotation);

            // Get the Rigid Body of the Slash prefab
            Rigidbody2D slashRB = slash.GetComponent<Rigidbody2D>();

            // Add Force to Slash prefab
            slashRB.AddForce(firePoint.up * playerScriptableObject.weapon.spaceprojectileForce, ForceMode2D.Impulse);

            // Reset isAttacking Bool;
            isAttacking = false;
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

            // Normalize movement vector and times it by attack move distance
            difference = difference.normalized * playerScriptableObject.weapon.leftMouse1SlideVelocity;
            // Slide in Attack Direction
            rb.AddForce(difference, ForceMode2D.Impulse);

            // Set AttackAnglePause Bool to True
            attackAnglePaused = true;
        }

        if (isAttacking)
        {
            // Instantiate Slash prefab
            GameObject slash = Instantiate(playerScriptableObject.weapon.leftMouse1Prefab, firePoint.position, firePoint.rotation);

            // Get the Rigid Body of the Slash prefab
            Rigidbody2D slashRB = slash.GetComponent<Rigidbody2D>();

            // Add Force to Slash prefab
            slashRB.AddForce(firePoint.up * playerScriptableObject.weapon.leftMouse1projectileForce, ForceMode2D.Impulse);

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

            // Normalize movement vector and times it by attack move distance
            difference = difference.normalized * playerScriptableObject.weapon.leftMouse1SlideVelocity;
            // Slide in Attack Direction
            rb.AddForce(difference, ForceMode2D.Impulse);

            // Set AttackAnglePause Bool to True
            attackAnglePaused = true;
        }

        if (isAttacking)
        {
            // Instantiate Slash prefab
            GameObject slash = Instantiate(playerScriptableObject.weapon.leftMouse1Prefab, firePoint.position, firePoint.rotation);

            // Get the Rigid Body of the Slash prefab
            Rigidbody2D slashRB = slash.GetComponent<Rigidbody2D>();

            // Add Force to Slash prefab
            slashRB.AddForce(firePoint.up * playerScriptableObject.weapon.leftMouse1projectileForce, ForceMode2D.Impulse);

            // Reset isAttacking Bool;
            isAttacking = false;
        }
    }

    public void Attack() // Animation Event
    {
        isAttacking = true;
    }

    public void AE_Attack2()
    {
        canAttack2 = true;
    }

    public void AttackAnimationEnd() // Animation Event
    {
        attackAnglePaused = false;
        canAttack2 = false;
        state = PlayerState.idle;
    }

    public void HitAnimationEnd()
    {
        state = PlayerState.idle;
    }

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

    // Input
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
            if (Time.time - lastAttack < playerScriptableObject.weapon.leftMouse1CoolDown)
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
}
