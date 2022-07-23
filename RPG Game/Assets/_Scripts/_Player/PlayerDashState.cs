using UnityEngine;

public class PlayerDashState : PlayerState
{
    private IDashBehaviour behaviour;

    public PlayerDashState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
        behaviour = stateMachine.Abilities.DashBehaviour;
    }

    public override void Update()
    {
        behaviour.BehaviourUpdate(stateMachine);
    }
}
