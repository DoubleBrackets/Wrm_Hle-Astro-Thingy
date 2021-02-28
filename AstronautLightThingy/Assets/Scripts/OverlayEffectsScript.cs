using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayEffectsScript : MonoBehaviour
{
    public static OverlayEffectsScript instance;

    public Sprite bloodOverlay;
    public float bloodDuration;

    private SpriteRenderer ren;
    private void Awake()
    {
        instance = this;
        ren = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        PlayerEntityScript.instance.OnPlayerDamagedEvent += OnPlayerDamagedEffect;
    }

    private void OnPlayerDamagedEffect()
    {
        StartCoroutine(BasicFade(bloodOverlay, bloodDuration));
    }


    private IEnumerator BasicFade(Sprite sprite,float time)
    {
        ren.sprite = sprite;
        Color c = ren.color;

        float timer = 0;
        while(timer < time)
        {
            timer += Time.deltaTime;
            c.a = 1 - timer / time;
            ren.color = c;
            yield return new WaitForEndOfFrame();
        }
        ren.sprite = null;
    }

    private void OnDestroy()
    {
        PlayerEntityScript.instance.OnPlayerDamagedEvent -= OnPlayerDamagedEffect;
    }
}
