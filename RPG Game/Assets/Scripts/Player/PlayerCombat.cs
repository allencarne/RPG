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
    public float attackMoveDistance;

    //private float attackCoolDown;
    //public float startAttackCoolDown;

    void Update()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        // Attack
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
            animator.SetFloat("Aim Horizontal", difference.x);
            animator.SetFloat("Aim Vertical", difference.y);

            // Move in attack direction
            difference = difference.normalized * attackMoveDistance;
            rb.AddForce(difference, ForceMode2D.Impulse);
        }

        // If mouse is inside attack range - attack - else - move player in attack direction

    }

    public void Attack()
    {
        GameObject slash = Instantiate(slashPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = slash.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * slashForce, ForceMode2D.Impulse);
        animator.ResetTrigger("Attack");
    }
}
