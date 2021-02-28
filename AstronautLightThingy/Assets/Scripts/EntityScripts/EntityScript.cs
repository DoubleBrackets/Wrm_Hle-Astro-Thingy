using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof (Rigidbody2D))]
public class EntityScript : MonoBehaviour
{
    public int health = 0;
    public int maxHealth = 0;

    public GameObject damagedParticles;
    public GameObject deathParticles;

    public GameObject drop;

    private Rigidbody2D entityRb;

    private float kbTime = 0.2f;
    private float kbTimer = 0f;

    public virtual void Awake()
    {
        entityRb = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (kbTimer >= 0)
            kbTimer -= Time.deltaTime;
    }

    public virtual void TakeDamage(int damage,Vector2 kbDir, float kbVel)
    {
        entityRb.AddForce(kbDir.normalized * kbVel);
        kbTimer = kbTime;
        if (damage == 0)
            return;
        Audio.instance.playEnemy();
        health -= damage;
        if(damagedParticles != null)
        {
            GameObject particles = Instantiate(damagedParticles, transform.position, Quaternion.Euler(0, 0, kbDir.Angle()),transform);
            Destroy(particles, 0.5f);
        }
        if (health <= 0)
            Death();
    }


    protected virtual void Death()
    {
        Instantiate(deathParticles, transform.position, Quaternion.identity);
        if(drop != null)
        {
            Instantiate(drop, transform.position, Quaternion.identity);
        }
        CineMachineImpulseManager.instance.Impulse((CameraScript.main.transform.position - transform.position).normalized*3);
        Destroy(gameObject);
    }

    public bool IsInKb()
    {
        if (kbTimer <= 0)
            return false;
        return true;
    }
}
