using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbOfRestoreScript : MonoBehaviour
{
    public float fuelRestore;
    public int energyRestore;
    public int healthRestore;

    Transform target;
    public Rigidbody2D rb;
    private void Awake()
    {
        target = MoveScript.instance.gameObject.transform;
        rb.velocity = Random.Range(0, 360f).UnitVector() * 2f;
    }

    private void FixedUpdate()
    {
        Vector2 dir = target.transform.position - transform.position;
        Vector2 cVec = rb.velocity;
        float angle = Vector2.SignedAngle(cVec, dir);
        rb.velocity = Quaternion.Euler(0, 0, 3 * 0.08f * angle) * rb.velocity;
        if(dir.magnitude < 0.1f)
        {
            Audio.instance.playRestore();
            PlayerWeaponScript.instance.AddEnergy(energyRestore);
            PlayerParticleManager.playerParticleManager.PlayParticle("RestoreParticles");
            PlayerEntityScript.instance.TakeDamage(-healthRestore, Vector2.zero, 0);
            MoveScript.instance.AddFuel(fuelRestore);
            Destroy(gameObject);
        }
    }


}
