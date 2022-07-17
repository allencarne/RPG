using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    //Variables
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float attackMoveDistance;
    [SerializeField] float slashForce;
    //[SerializeField] float attackRange;
    public bool isAttacking;

    //Components
    Vector2 movement;
    [SerializeField] Animator animator;
    [SerializeField] GameObject player;
    public Rigidbody2D rb;
    public GameObject slashPrefab;
    public Transform firePoint;

    enum PlayerState
    {
        idle,
        move,
        attack,
    }

    PlayerState state = PlayerState.idle;

    private void Awake()
    {
        animator = player.GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
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
        }
    }

    public void PlayerIdleState()
    {
        //State Transition - Move\\
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            state = PlayerState.move;
        }

        //State Transition - Attack\\
        if (Input.GetMouseButtonDown(0))
        {
            state = PlayerState.attack;
        }

        ////////State Logic
        //Set isMoving Bool to False
        animator.SetBool("isMoving", false);
    }

    public void PlayerMoveState()
    {
        //State Transition - Idle\\
        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
        {
            state = PlayerState.idle;
        }

        //State Transition - Attack\\
        if (Input.GetMouseButtonDown(0))
        {
            state = PlayerState.attack;
        }

        ////////State Logic
        //Set isMoving Bool to true
        animator.SetBool("isMoving", true);

        //Input
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        movement = moveInput.normalized * moveSpeed;

        //Movement
        rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);

        //Animations
        if (movement != Vector2.zero)
        {
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
        }
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    public void PlayerAttackState()
    {
        //State Transition\\
        //AttackAnimationEnd Method Is Handling The Transition

        ////////State Logic
        //Trigger Attack Animation
        animator.SetTrigger("Attack");

        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        // Set Attack Animation Depending on Mouse Position
        animator.SetFloat("Aim Horizontal", difference.x);
        animator.SetFloat("Aim Vertical", difference.y);

        // Set Idle to last attack position
        animator.SetFloat("Horizontal", difference.x);
        animator.SetFloat("Vertical", difference.y);

        //// Slide Forward When Attacking
        // Create a Vector from Camera position subtracted by player position
        //Vector3 differencee = Camera.main.ScreenToWorldPoint(Input.mousePosition) - playerTransform.position;

        // Normalize movement vector and times it by attack move distance
        difference = difference.normalized * attackMoveDistance;

        // Add force in Attack Direction
        rb.AddForce(difference, ForceMode2D.Impulse);

        if (isAttacking)
        {
            // Instantiate Slash prefab
            GameObject slash = Instantiate(slashPrefab, firePoint.position, firePoint.rotation);

            // Get the Rigid Body of the Slash prefab
            Rigidbody2D slashRB = slash.GetComponent<Rigidbody2D>();

            // Add Force to Slash prefab
            slashRB.AddForce(firePoint.up * slashForce, ForceMode2D.Impulse);

            //Reset Animator Trigger
            animator.ResetTrigger("Attack");
            isAttacking = false;
        }
    }

    public void AttackAnimationEnd()
    {
        animator.ResetTrigger("Attack");
        state = PlayerState.idle;
    }

    public void Attack()
    {
        isAttacking = true;
    }

    public void PauseAttackAngleMethod()
    {

    }
}
