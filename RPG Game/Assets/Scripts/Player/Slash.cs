using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    public GameObject hitEffect;

    void Update()
    {
        //Destroy(gameObject, 0.5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        //Destroy(effect, 0.3f);
        //Destroy(gameObject);
    }

    public void DestroyAfterAnimation()
    {
        Destroy(gameObject);
    }
}
