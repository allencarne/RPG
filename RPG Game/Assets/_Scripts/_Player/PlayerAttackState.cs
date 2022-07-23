using UnityEngine;

public class PlayerAttackState : PlayerState
{
    private IAttackBehaviour behaviour;

    public PlayerAttackState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
        behaviour = stateMachine.Abilities.AttackBehaviour;
    }

    public override void Update()
    {
        behaviour.BehaviourUpdate(stateMachine);
    }
}
