using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Dashes/SwordWindDash", fileName = "SwordWindDash")]
public class SwordWindDash : ScriptableObject, IDashBehaviour
{
    [SerializeField] private float dashMoveDistance;

    public void BehaviourUpdate(PlayerStateMachine stateMachine)
    {
        //State Logic
        stateMachine.Animator.SetTrigger("Dash");

        Vector2 difference = stateMachine.GetPlayerMouseDifference();

        if (!stateMachine.AttackAnglePaused)
        {
            //Set Attack Animation Depending on Mouse Position
            stateMachine.Animator.SetFloat("Horizontal", difference.x);
            stateMachine.Animator.SetFloat("Vertical", difference.y);

            //Set Idle to last attack position
            stateMachine.Animator.SetFloat("Horizontal", difference.x);
            stateMachine.Animator.SetFloat("Vertical", difference.y);

            ////Slide Forward When Attacking
            //Normalize movement vector and times it by attack move distance
            difference = difference.normalized * dashMoveDistance;

            //Add force in Attack Direction
            stateMachine.Rigidbody.AddForce(difference, ForceMode2D.Impulse);

            stateMachine.AttackAnglePaused = true;
        }

        if (stateMachine.IsAttacking)
        {
            stateMachine.CreateSlash();
            //Reset Animator Trigger
            stateMachine.Animator.ResetTrigger("Dash");

            //Reset isAttacking Bool;
            stateMachine.IsAttacking = false;
        }
    }
}
