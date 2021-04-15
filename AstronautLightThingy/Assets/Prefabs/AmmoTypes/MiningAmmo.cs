using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningAmmo : MonoBehaviour
{
    public float range = 0.15f;
    public GameObject particleEffect;

    private void OnEnable()
    {
        Vector2 dir = transform.rotation * Vector2.right;
        ShootPhysics(dir);
    }

    void ShootPhysics(Vector2 dir)
    {
        //Raycasts for tiles
        var hits = ProcGenTileScript.instance.TileRayCastAll(transform.position, dir, range);
        foreach(Vector3Int hit in hits)
        {
            ProcGenTileScript.instance.DestroyTile(hit);
        }
        GameObject particles = Instantiate(particleEffect, transform.position, transform.rotation);
        Destroy(particles,1f);
        Destroy(gameObject,1.1f);
    }
    
}
