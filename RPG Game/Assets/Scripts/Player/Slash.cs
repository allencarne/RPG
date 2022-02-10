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

    private void OnTriggerEnter2D(Collider2D other)
    {
        //GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        //Destroy(effect, 0.3f);
        //Destroy(gameObject);
        Debug.Log("Hit");
    }

    public void DestroyAfterAnimation()
    {
        Destroy(gameObject);
    }
}
