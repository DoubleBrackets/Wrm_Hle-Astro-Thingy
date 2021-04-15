using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultAmmo : MonoBehaviour
{
    public float range = 20f;
    public float width = 0.2f;
    private float knockback = 30f;

    public AmmoData ammo;

    public GameObject particleEffect;

    private void OnEnable()
    {
        Vector2 dir = transform.rotation * Vector2.right;
        float hitrange = ShootPhysics(dir);
        StartCoroutine(ShootAnim(dir,hitrange));
    }

    float ShootPhysics(Vector2 dir)
    {
        //raycast
        RaycastHit2D hit = Physics2D.BoxCast((Vector2)transform.position-(dir.normalized*0.1f), new Vector2(width, 0.01f), dir.Angle(), dir, range+0.1f, ammo.hitMask);

        if (hit.collider == null)
            return range;
        LayerMask layer = 1<<hit.collider.gameObject.layer;
        GameObject g = hit.collider.gameObject;
        if (layer.Equals(ammo.enemyMask))
        {
            GameObject particles = Instantiate(particleEffect, hit.collider.transform.position, transform.rotation);
            Destroy(particles, 0.7f);
            g.GetComponent<EntityScript>().TakeDamage(2, dir, knockback);
        }
        else
        {
/*            GameObject particles = Instantiate(particleEffect, hit.point, transform.rotation);
            Destroy(particles, 0.7f);*/
            ProcGenTileScript.instance.DestroyTile(hit.point+dir.normalized*0.01f);
        }

        return ((Vector2)transform.position - hit.point).magnitude;
    }
    
    IEnumerator ShootAnim(Vector2 dir,float range)
    {
        LineRenderer lineRen = GetComponent<LineRenderer>();

        lineRen.positionCount = 2;
        lineRen.SetPosition(0,transform.position);
        lineRen.SetPosition(1, (Vector2)transform.position + dir * (range+0.2f));

        float widthMult = lineRen.widthMultiplier;
        for(int x = 0;x <= 10;x++)
        {
            lineRen.widthMultiplier = widthMult * (1 - x / 10f);
            yield return new WaitForSeconds(0.03f);
        }

        Destroy(gameObject);
    }
}
