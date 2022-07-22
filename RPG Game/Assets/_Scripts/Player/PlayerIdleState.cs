
public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(PlayerStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Update()
    {
        
        //State Transition - Move\\
        if (stateMachine.AnyMoveKeyPressed)
            stateMachine.SetState(new PlayerMoveState(stateMachine));

        //State Transition - Attack\\
        if (stateMachine.AttackKeyPressed)
            stateMachine.SetState(new PlayerAttackState(stateMachine));

        /*/State Transition - Dash\\
        if (stateMachine.DashKeyPressed)
            stateMachine.SetState(new DashState(stateMachine));
        */

        //State Logic
        stateMachine.Animator.SetBool("isMoving", false);
        
    }
}
