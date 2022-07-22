using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    //Variables
    [SerializeField] float moveSpeed;
    [SerializeField] float slashForce;

    //Components
    [SerializeField] GameObject player;
    [SerializeField] GameObject slashPrefab;
    [SerializeField] Transform firePoint;

    [HideInInspector] Animator animator;
    [HideInInspector] Rigidbody2D rb;

    //private PlayerAbilities abilities
    //public Animator Animator => animator;
    //public float MoveSpeed => moveSpeed;
    //public Rigidbody2D Rigidbody => rb;

    //turn into getter and setter
    public bool attackAnglePaused = false;
    public bool isAttacking;

    private Camera cam;
    //private PlayerState state;

    //State/Ability Specific Variables
    [SerializeField] float attackMoveDistance;
    [SerializeField] float dashMoveDistance;

    //Move State Variable
    Vector2 movement;

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
        animator = player.GetComponent<Animator>();
        rb = player.GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
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

        //State Transition - Dash\\
        if (Input.GetKey(KeyCode.Space))
        {
            state = PlayerState.dash;
        }

        //__State Logic__\\
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

        //State Transition - Dash\\
        if (Input.GetKey(KeyCode.Space))
        {
            state = PlayerState.dash;
        }

        //__State Logic__\\
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
        //__State Logic__\\
        //Trigger Attack Animation
        animator.SetTrigger("Attack");

        //Calculate the difference between mouse position and player position
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        if (!attackAnglePaused)
        {
            //Set Attack Animation Depending on Mouse Position
            animator.SetFloat("Aim Horizontal", difference.x);
            animator.SetFloat("Aim Vertical", difference.y);

            //Set Idle to last attack position
            animator.SetFloat("Horizontal", difference.x);
            animator.SetFloat("Vertical", difference.y);

            ////Slide Forward When Attacking
            //Normalize movement vector and times it by attack move distance
            difference = difference.normalized * attackMoveDistance;

            //Add force in Attack Direction
            rb.AddForce(difference, ForceMode2D.Impulse);

            //Set AttackAnglePause Bool to True
            attackAnglePaused = true;
        }

        if (isAttacking)
        {
            CreateSlash();

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
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

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
            difference = difference.normalized * dashMoveDistance;

            //Add force in Attack Direction
            rb.AddForce(difference, ForceMode2D.Impulse);

            //Set AttackAnglePause Bool to True
            attackAnglePaused = true;
        }

        if (isAttacking)
        {
            CreateSlash();

            //Reset Animator Trigger
            animator.ResetTrigger("Dash");

            //Reset isAttacking Bool;
            isAttacking = false;
        }
    }

    //Used in Animation Event
    public void AttackAnimationEnd()
    {
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("Dash");
        attackAnglePaused = false;
        state = PlayerState.idle;
    }

    //Used in Animation Event
    public void Attack()
    {
        isAttacking = true;
    }

    public void CreateSlash()
    {
        //Instantiate Slash prefab
        GameObject slash = Instantiate(slashPrefab, firePoint.position, firePoint.rotation);

        //Get the Rigid Body of the Slash prefab
        Rigidbody2D slashRB = slash.GetComponent<Rigidbody2D>();

        //Add Force to Slash prefab
        slashRB.AddForce(firePoint.up * slashForce, ForceMode2D.Impulse);
    }
}
