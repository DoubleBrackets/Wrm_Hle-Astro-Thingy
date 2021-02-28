using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntityScript : EntityScript
{
    public static PlayerEntityScript instance;
    // Start is called before the first frame update

    public override void Awake()
    {
        base.Awake();
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }


    //events
    public event Action OnPlayerDamagedEvent;
    public event Action OnPlayerHealthChangedEvent;
    public override void TakeDamage(int damage, Vector2 kbDir, float kbVel)
    {
        StartCoroutine(TakeDamageCoroutine(damage, kbDir, kbVel));
    }

    IEnumerator TakeDamageCoroutine(int damage, Vector2 kbDir, float kbVel)
    {
        yield return new WaitForSeconds(0.05f);
        CineMachineImpulseManager.instance.Impulse(kbDir.normalized * 7);
        Audio.instance.playHurt();
        base.TakeDamage(damage, kbDir, kbVel);
        if (damage > 0)
            OnPlayerDamagedEvent?.Invoke();
        if(damage != 0)
            OnPlayerHealthChangedEvent?.Invoke();
        if (health > maxHealth)
            health = maxHealth;
    }

    protected override void Death()
    {
        Instantiate(deathParticles, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
        GameManagerScript.instance.GameOver();
    }

}
