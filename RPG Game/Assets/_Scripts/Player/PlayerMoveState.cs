using UnityEngine;

public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {

    }

    public override void Update()
    {
        //State Transition - Idle\\
        if (!stateMachine.AnyMoveKeyPressed)
            stateMachine.SetState(new PlayerIdleState(stateMachine));

        //State Transition - Attack\\
        if (stateMachine.AttackKeyPressed)
            stateMachine.SetState(new PlayerAttackState(stateMachine));
                /*

        //State Transition - Dash\\
        if (stateMachine.DashKeyPressed)
            stateMachine.SetState(new DashState(stateMachine));
        */

        //__State Logic__\\
        stateMachine.Animator.SetBool("isMoving", true);

        //Input
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        var movement = moveInput.normalized * stateMachine.MoveSpeed;

        //Movement
        stateMachine.Rigidbody.MovePosition(stateMachine.Rigidbody.position + movement * Time.fixedDeltaTime);

        //Animations
        if (movement != Vector2.zero)
        {
            stateMachine.Animator.SetFloat("Horizontal", movement.x);
            stateMachine.Animator.SetFloat("Vertical", movement.y);
        }
        stateMachine.Animator.SetFloat("Speed", movement.sqrMagnitude);
    }
}
