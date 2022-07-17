using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Transform firePoint;
    public GameObject slashPrefab;
    public Animator animator;
    public Rigidbody2D rb;

    public float slashForce;
    public float attackRange;

    //private float attackCoolDown;
    //public float startAttackCoolDown;

    void Update()
    {
        // Create a Vector from Camera position subtracted by player position
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        // If mouse button is pressed, Attack
        if (Input.GetMouseButtonDown(0))
        {
            // Activate Attack State
            animator.SetTrigger("Attack");

            // Set Attack in Current Attack Direction
            animator.SetFloat("Aim Horizontal", difference.x);
            animator.SetFloat("Aim Vertical", difference.y);

            // Set Idle to last attack position
            animator.SetFloat("Horizontal", difference.x);
            animator.SetFloat("Vertical", difference.y);
        }
    }

    /*public void Attack()
    {
        // Instantiate Slash prefab
        GameObject slash = Instantiate(slashPrefab, firePoint.position, firePoint.rotation);

        // Get the Rigid Body of the Slash prefab
        Rigidbody2D rb = slash.GetComponent<Rigidbody2D>();

        // Add Force to Slash prefab
        rb.AddForce(firePoint.up * slashForce, ForceMode2D.Impulse);

        //Reset Animator Trigger
        animator.ResetTrigger("Attack");
    }*/
}
