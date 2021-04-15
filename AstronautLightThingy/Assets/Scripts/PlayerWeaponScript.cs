using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponScript : MonoBehaviour
{
    public static PlayerWeaponScript instance;

    public static int ammoTypeCount;

    private SpriteRenderer gunSpriteRen;


    private float shootCooldown = 0;

    public float energy;
    public float maxEnergy;

    public AmmoType[] ammoTypes;
    public Vector2 barrelOffset;
    [System.Serializable]
    public struct AmmoType
    {
        public GameObject projectilePrefab;
        public float cooldown;
        public Color iconColor;
        public float recoilForce;
        public float energyCost;
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            return;
        gunSpriteRen = GetComponent<SpriteRenderer>();
        ammoTypeCount = ammoTypes.Length;    }

    // Update is called once per frame
    void Update()
    {
        if (shootCooldown > 0)
            shootCooldown -= Time.deltaTime;
        //rotate gun to face cursor
        Vector2 mousePos = CameraScript.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 shootVec = (mousePos - (Vector2)transform.position);
        float angle = shootVec.Angle();

        RotateSprite(angle);

        if(Input.GetMouseButton(0) && shootCooldown <= 0 && energy > ammoTypes[0].energyCost)
        {
            Shoot(shootVec,0);
        }  
        else if(Input.GetKeyDown(KeyCode.Space) && shootCooldown <= 0 && energy >= ammoTypes[1].energyCost)
        {
            Shoot(shootVec, 1);
        }
        else if(Input.GetMouseButton(1) && shootCooldown <= 0 && energy > ammoTypes[2].energyCost)
        {
            Shoot(shootVec, 2);
        }
    }

    void Shoot(Vector2 shootVec,int index)
    {
        Audio.instance.playShoot();
        shootVec = shootVec.normalized;
        PlayerParticleManager.playerParticleManager.PlayParticle("ShootParticles");
        //Gets ammotype and isntantiates it
        AmmoType type = ammoTypes[index];
        AddEnergy( -type.energyCost);
        Quaternion shootAngle = Quaternion.Euler(0, 0, shootVec.Angle());
        Instantiate(type.projectilePrefab, transform.position + shootAngle*barrelOffset, shootAngle); 
        shootCooldown = type.cooldown;
        //recoil
        MoveScript.rb.AddForce(-shootVec * type.recoilForce);
        CineMachineImpulseManager.instance.Impulse(-shootVec * type.recoilForce/8f);
    }

    void RotateSprite(float angle)
    {
        if (angle > 90 || angle < -90)
            gunSpriteRen.flipY = true;
        else
            gunSpriteRen.flipY = false;

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void AddEnergy(float val)
    {
        energy += val;
        if (energy > maxEnergy)
            energy = maxEnergy;
        EnergyBar.instance.SetValue(energy / (float)maxEnergy);
    }
}
