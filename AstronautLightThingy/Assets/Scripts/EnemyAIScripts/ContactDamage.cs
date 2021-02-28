using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamage : MonoBehaviour
{ 
    public int damage;
    public float knockbackForce;

    private void Awake()
    {
        damage += Random.Range(-5, 5);
        knockbackForce += Random.Range(-5f, 5f);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerEntityScript p = collision.collider.GetComponent<PlayerEntityScript>();
        if (p != null)
        {
            p.TakeDamage(damage, p.transform.position - transform.position, knockbackForce);
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }
}
