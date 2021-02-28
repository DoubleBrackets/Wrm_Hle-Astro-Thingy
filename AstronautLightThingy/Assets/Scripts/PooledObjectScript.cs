using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledObjectScript : MonoBehaviour
{
    private ParticleSystem psys;
    private void Awake()
    {
        psys = GetComponent<ParticleSystem>();
    }
    private void OnEnable()
    {
        psys.Play();
        StartCoroutine(Disable());
    }

    IEnumerator Disable()
    {
        yield return new WaitForSeconds(psys.main.startLifetime.constant);
        gameObject.SetActive(false);
    }
}
