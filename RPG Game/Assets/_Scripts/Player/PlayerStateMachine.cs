using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    //Variables
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float attackMoveDistance;

    //Components
    public Animator animator;
    public Rigidbody2D rb;
    public Vector2 movement;
    public Transform transform;

    enum PlayerState
    {
        idle,
        move,
        attack,
    }

    PlayerState state = PlayerState.idle;

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
        }
    }

    public void PlayerIdleState()
    {
        //State Transition - Move
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            state = PlayerState.move;
        }

        ////State Logic
        //Set isMoving Bool to False
        animator.SetBool("isMoving", false);
    }

    public void PlayerMoveState()
    {
        //State Transition - Idle
        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
        {
            state = PlayerState.idle;
        }

        ////State Logic
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

    }
}
