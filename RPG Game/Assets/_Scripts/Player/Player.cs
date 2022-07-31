using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    [Header("Variables")]
    Vector2 movement;
    bool isAttacking;
    bool attackAnglePaused = false;

    [Header("Components")]
    public PlayerScriptableObject playerScriptableObject;
    public Animator animator;
    public Rigidbody2D rb;
    public Transform firePoint;
    private Camera cam;

    [Header("HealthBar")]
    public Image frontHealthBar;
    public Image backHealthbar;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI nameText;
    private float chipSpeed = 2f;
    private float lerpTimer;

    enum PlayerState
    {
        idle,
        move,
        attack,
        dash
    }

    PlayerState state = PlayerState.idle;

    private void Awake()
    {
        cam = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Set Current Player Health to Max Health
        playerScriptableObject.health = playerScriptableObject.maxHealth;
        nameText.text = playerScriptableObject.name.ToString();
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
            case PlayerState.dash:
                PlayerDashState();
                break;
        }

        // Healthbar UI Update
        playerScriptableObject.health = Mathf.Clamp(playerScriptableObject.health, 0, playerScriptableObject.maxHealth);
        UpdateHealthUI();

        if (Input.GetKeyDown(KeyCode.X))
        {
            PlayerTakeDamage(5f);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            RestoreHealth(5f);
        }
    }

    public void PlayerIdleState()
    {
        // Animate
        animator.SetBool("isMoving", false);

        // State Transition - Move
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            state = PlayerState.move;
        }

        // State Transition - Attack
        if (Input.GetMouseButtonDown(0))
        {
            state = PlayerState.attack;
        }

        // State Transition - Dash
        if (Input.GetKey(KeyCode.Space))
        {
            state = PlayerState.dash;
        }
    }

    public void PlayerMoveState()
    {
        // Animate
        animator.SetBool("isMoving", true);
        if (movement != Vector2.zero)
        {
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
        }
        animator.SetFloat("Speed", movement.sqrMagnitude);

        //Input
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        movement = moveInput.normalized * playerScriptableObject.speed;

        //Movement
        rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);

        //State Transition - Idle
        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
        {
            state = PlayerState.idle;
        }

        //State Transition - Attack
        if (Input.GetMouseButtonDown(0))
        {
            state = PlayerState.attack;
        }

        //State Transition - Dash
        if (Input.GetKey(KeyCode.Space))
        {
            state = PlayerState.dash;
        }
    }

    public void PlayerAttackState()
    {
        // Animate
        animator.SetTrigger("Attack");

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

            //Normalize movement vector and times it by attack move distance
            difference = difference.normalized * playerScriptableObject.weapon.leftMouse1Velocity;
            //Add force in Attack Direction
            rb.AddForce(difference, ForceMode2D.Impulse);

            //Set AttackAnglePause Bool to True
            attackAnglePaused = true;
        }

        if (isAttacking)
        {
            //Instantiate Slash prefab
            GameObject slash = Instantiate(playerScriptableObject.weapon.leftMouse1Prefab, firePoint.position, firePoint.rotation);

            //Get the Rigid Body of the Slash prefab
            Rigidbody2D slashRB = slash.GetComponent<Rigidbody2D>();

            //Add Force to Slash prefab
            slashRB.AddForce(firePoint.up * playerScriptableObject.weapon.projectileForce, ForceMode2D.Impulse);

            //Reset Animator Trigger
            animator.ResetTrigger("Attack");

            //Reset isAttacking Bool;
            isAttacking = false;
        }
    }

    public void PlayerDashState()
    {
        //State Logic
        animator.SetTrigger("Dash");

        //Calculate the difference between mouse position and player position
        Vector2 difference = cam.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        if (!attackAnglePaused)
        {
            //Set Attack Animation Depending on Mouse Position
            animator.SetFloat("Horizontal", difference.x);
            animator.SetFloat("Vertical", difference.y);

            //Set Idle to last attack position
            animator.SetFloat("Horizontal", difference.x);
            animator.SetFloat("Vertical", difference.y);

            ////Slide Forward When Attacking
            //Normalize movement vector and times it by attack move distance
            difference = difference.normalized * playerScriptableObject.weapon.spaceVelocity;

            //Add force in Attack Direction
            rb.AddForce(difference, ForceMode2D.Impulse);

            //Set AttackAnglePause Bool to True
            attackAnglePaused = true;
        }

        if (isAttacking)
        {
            //Instantiate Slash prefab
            GameObject slash = Instantiate(playerScriptableObject.weapon.spacePrefab, firePoint.position, firePoint.rotation);

            //Get the Rigid Body of the Slash prefab
            Rigidbody2D slashRB = slash.GetComponent<Rigidbody2D>();

            //Add Force to Slash prefab
            slashRB.AddForce(firePoint.up * playerScriptableObject.weapon.projectileForce, ForceMode2D.Impulse);

            //Reset Animator Trigger
            animator.ResetTrigger("Dash");

            //Reset isAttacking Bool;
            isAttacking = false;
        }
    }

    public void AttackAnimationEnd() // Animation Event
    {
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("Dash");
        attackAnglePaused = false;
        state = PlayerState.idle;
    }

    public void Attack() // Animation Event
    {
        isAttacking = true;
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

    public void RestoreHealth (float healAmount)
    {
        playerScriptableObject.health += healAmount;
        lerpTimer = 0f;
    }

    public void PlayerTakeDamage(float damage)
    {
        playerScriptableObject.health -= damage;
        lerpTimer = 0f;

        if (playerScriptableObject.health <= 0)
        {
            PlayerDie();
        }
    }

    public void IncreaseHealth(int level)
    {
        playerScriptableObject.maxHealth += (playerScriptableObject.health * 0.01f) * ((100 - level) * 0.01f);
        playerScriptableObject.health = playerScriptableObject.maxHealth;
    }

    public void PlayerDie()
    {
        Destroy(gameObject);
    }
}
