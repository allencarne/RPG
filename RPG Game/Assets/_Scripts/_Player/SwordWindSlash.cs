using UnityEngine;

[CreateAssetMenu(fileName = "SwordWindSlash", menuName = "Scriptable Objects/Attacks/SwordWindSlash", order = 0)]
public class SwordWindSlash : ScriptableObject, IAttackBehaviour
{
    [SerializeField] private float attackMoveDistance;

    public void BehaviourUpdate(PlayerStateMachine stateMachine)
    {
        //Trigger Attack Animation
        stateMachine.Animator.SetTrigger("Attack");

        Vector2 difference = stateMachine.GetPlayerMouseDifference();

        if (!stateMachine.AttackAnglePaused)
        {
            //Set Attack Animation Depending on Mouse Position
            stateMachine.Animator.SetFloat("Aim Horizontal", difference.x);
            stateMachine.Animator.SetFloat("Aim Vertical", difference.y);

            //Set Idle to last attack position
            stateMachine.Animator.SetFloat("Horizontal", difference.x);
            stateMachine.Animator.SetFloat("Vertical", difference.y);

            //Normalize movement vector and times it by attack move distance
            difference = difference.normalized * attackMoveDistance;

            //Add force in Attack Direction
            stateMachine.Rigidbody.AddForce(difference, ForceMode2D.Impulse);

            stateMachine.AttackAnglePaused = true;
        }

        if (stateMachine.IsAttacking)
        {
            stateMachine.CreateSlash();
            //Reset Animator Trigger
            stateMachine.Animator.ResetTrigger("Attack");

            //Reset isAttacking Bool;
            stateMachine.IsAttacking = false;
        }
    }
}
