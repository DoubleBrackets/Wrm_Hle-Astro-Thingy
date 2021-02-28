using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimScript : MonoBehaviour
{
    private Animation animator;
    private MoveScript mScript;
    private SpriteRenderer spriteRen;

    public ParticleSystem thrustParticles;

    private void Start()
    {
        mScript = MoveScript.instance;
        animator = GetComponent<Animation>();
        spriteRen = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        Vector2 movement = mScript.GetInput();

        Vector2 mousePos = CameraScript.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 facing = mousePos - (Vector2)transform.position ;
        //Particles
        if (facing.x < 0)
        {
            spriteRen.flipX = true;
        }
        else
            spriteRen.flipX = false;

        if(movement.magnitude == 0)
        {
            if (!thrustParticles.isStopped)
                thrustParticles.Stop();
        }
        else
        {
            if (thrustParticles.isStopped)
                thrustParticles.Play();
        }


        var partRot = thrustParticles.shape.rotation;
        partRot.z = Mathf.Lerp(partRot.z,movement.Angle() - 90,0.1f);
        var shape = thrustParticles.shape;
        shape.rotation = partRot;
    }
}
