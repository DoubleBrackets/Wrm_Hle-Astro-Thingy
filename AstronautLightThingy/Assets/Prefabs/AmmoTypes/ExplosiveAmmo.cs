using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveAmmo : MonoBehaviour
{
    public float lifeTime;
    public float homingRadius;
    public float homingForce;
    public float blastRadius;
    public float startVel;
    public int damage;
    public float kbForce;

    private bool detonated = false;

    public Rigidbody2D rb;
    public ParticleSystem psys;

    public AmmoData data;

    private float timer = 0f;

    Transform lockedOn = null;

    private void Awake()
    {
        rb.velocity = transform.rotation * Vector2.right*startVel;
        timer = lifeTime;
    }

    private void FixedUpdate()
    {
        if (detonated)
            return;
        timer -= Time.fixedDeltaTime;
        if (timer <= 0)
            StartCoroutine(Detonate());
        if(lockedOn == null)
        {
            Collider2D hits = Physics2D.OverlapCircle(transform.position, homingRadius, data.enemyMask);
            if (hits != null)
            {
                lockedOn = hits.transform;
            }
        }
        else
        {
            Vector2 dir = lockedOn.transform.position - transform.position;
            Vector2 cVec = rb.velocity;
            float angle = Vector2.SignedAngle(cVec, dir);
            rb.velocity = Quaternion.Euler(0,0,homingForce*0.08f*angle)*rb.velocity;
        }
 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!detonated)
            StartCoroutine(Detonate());
    }

    IEnumerator Detonate()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        detonated = true;
        psys.Play();
        Audio.instance.playExplosion();
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, blastRadius,data.enemyMask | data.playerMask);
        GetComponent<SpriteRenderer>().enabled = false;
        foreach(Collider2D hit in hits)
        {
            EntityScript ent = hit.GetComponent<EntityScript>();
            if(ent != null)
            {
                ent.TakeDamage(damage, hit.transform.position - gameObject.transform.position, kbForce);
            }
        }
        yield return new WaitForSeconds(0.75f);
        Destroy(gameObject);
    }
}
