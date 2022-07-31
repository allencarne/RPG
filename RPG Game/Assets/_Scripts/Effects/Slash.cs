using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    public WeaponScriptableObject weaponScriptableObject;

    //public GameObject hitEffect;

    //public int attackDamage = 1;
    //[SerializeField] private float knockBackForce;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            // Hit Effect
            GameObject effect = Instantiate(weaponScriptableObject.leftMouse1HitEffect, other.transform.position, Quaternion.identity);
            Destroy(effect, .3f);

            // Deal Damage
            other.GetComponent<Enemy>().TakeDamage(weaponScriptableObject.attackDamage);

            // Apply Knockback
            Rigidbody2D enemy = other.GetComponent<Rigidbody2D>();
            Vector2 difference = enemy.transform.position - transform.position;
            difference = difference.normalized * weaponScriptableObject.leftMouse1KnockBackForce;
            enemy.AddForce(difference, ForceMode2D.Impulse);
        }
    }

    public void DestroyAfterAnimation()
    {
        Destroy(gameObject);
    }
}
