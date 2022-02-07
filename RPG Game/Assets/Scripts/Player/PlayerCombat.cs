using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Transform firePoint;
    public GameObject slashPrefab;
    public Animator animator;

    public float slashForce;

    //private float attackCoolDown;
    //public float startAttackCoolDown;

    void Update()
    {

    }

    public void Attack()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        animator.SetFloat("Aim Horizontal", difference.x);
        animator.SetFloat("Aim Vertical", difference.y);
        GameObject slash = Instantiate(slashPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = slash.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * slashForce, ForceMode2D.Impulse);
    }
}
