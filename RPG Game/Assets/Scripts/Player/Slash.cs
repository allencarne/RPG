using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    public GameObject hitEffect;

    public int attackDamage = 1;

    void Update()
    {
        //Destroy(gameObject, 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("Hit");
            other.GetComponent<Enemy>().TakeDamage(attackDamage);

        }
    }

    public void DestroyAfterAnimation()
    {
        Destroy(gameObject);
    }
}
