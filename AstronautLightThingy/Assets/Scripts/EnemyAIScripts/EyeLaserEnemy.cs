using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeLaserEnemy : MonoBehaviour
{
    LineRenderer laserLineRen;

    public float range;
    public int damage;
    public float chargeTime;
    public float cooldown;
    public float moveForce;
    public float maxVel;
    public float width;
    public float knockbackForce;

    public Gradient shootColor;
    public Gradient indicatorColor;

    private GenericEnemy enemyBase;
    private float timer = 0;
    private ParticleSystem psys;

    public bool isRush;

    private void Awake()
    {
        enemyBase = GetComponentInParent<GenericEnemy>();
        enemyBase.enemyAIUpdate += LaserEnemyUpdate;
        laserLineRen = GetComponent<LineRenderer>();
        psys = GetComponent<ParticleSystem>();
        range += Random.Range(-1f, 1f);
        damage += Random.Range(-5, 5);
        chargeTime += Random.Range(-0.15f, 0.15f);
        cooldown += Random.Range(-2f, 1.5f);
        moveForce += Random.Range(-0.5f, 0.5f);
        maxVel += Random.Range(-0.5f, 0.5f);
        knockbackForce += Random.Range(-10f, 10f);

    }

    void LaserEnemyUpdate(Vector2 dir)
    {
        MoveTowardsPlayer(dir);
        if (timer > 0)
        {
            timer -= Time.fixedDeltaTime * 5f;
            return;
        }
        if (dir.magnitude > range)
            return;
        StartCoroutine(ShootLaser(dir));
        timer = cooldown;
    }

    void MoveTowardsPlayer(Vector2 dir)
    {
        if (isRush)
            return;
        Rigidbody2D rb = enemyBase.RigidBody();
        if (dir.magnitude <= range - 1f)
        {
            rb.velocity = rb.velocity / 2f;
        }
        rb.AddForce(dir.normalized * moveForce);
        if (rb.velocity.magnitude > maxVel)
            rb.velocity = rb.velocity.normalized * maxVel;
    }

    IEnumerator ShootLaser(Vector2 dir)
    {
        laserLineRen.positionCount = 2;
        laserLineRen.SetPosition(0, transform.position);
        Vector2 newDir = dir.normalized * range * 2f;
        psys.Play();
        laserLineRen.colorGradient = indicatorColor;
        for (int x = 0;x <= 10;x++)
        {
            laserLineRen.widthMultiplier = width*0.1f * (1-(x / 10f));
            laserLineRen.SetPosition(0, transform.position);
            laserLineRen.SetPosition(1, (Vector2)transform.position + newDir);
            yield return new WaitForSeconds(chargeTime / 10f);
        }
        laserLineRen.widthMultiplier = width * 2;
        laserLineRen.colorGradient = shootColor;
        float temprange = LaserPhysics(dir);
        newDir = dir.normalized * temprange;
        for (int x = 0; x <= 10; x++)
        {
            laserLineRen.widthMultiplier = 0.8f*width * (1 - (x / 10f));
            laserLineRen.SetPosition(0, transform.position);
            laserLineRen.SetPosition(1, (Vector2)transform.position + newDir);
            yield return new WaitForSeconds(0.03f);
        }
    }

    float LaserPhysics(Vector2 dir)
    {
        Audio.instance.playLazer();
        float angle = dir.Angle();
        Vector2 norm = dir.normalized;
        float tempRange = range*2;
        //Casts for tiles

        for(int x = -1;x <= 1;x++)
        {
            var tileHits = ProcGenTileScript.instance.TileRayCastAll((Vector2)transform.position+dir.normalized.PerpendicularVector(x)*width*0.3f, dir, range * 2f);
            if (tileHits.Count > 0)
            {
                float hitRange = ((Vector2)transform.position - ProcGenTileScript.instance.TileToPosition(tileHits[0])).magnitude + 0.4f;
                if (hitRange < tempRange)
                {
                    tempRange = hitRange;
                }
                foreach (Vector3Int hit in tileHits)
                {
                    if (((Vector2)transform.position - ProcGenTileScript.instance.TileToPosition(hit)).magnitude > hitRange)
                        break;
                    ProcGenTileScript.instance.DestroyTile(hit);
                }
            }
        }

        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, new Vector2(width, 0.01f), angle, dir, tempRange, enemyBase.playerMask);
  
        foreach (RaycastHit2D hit in hits)
        {
            LayerMask layer = 1 << hit.collider.gameObject.layer;
            GameObject g = hit.collider.gameObject;
            if (layer.Equals(enemyBase.playerMask))
            {
                g.GetComponent<EntityScript>().TakeDamage(damage, dir, knockbackForce);
            }
        }

        return tempRange;
    }

}
