using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public static HealthBar instance;
    public Slider slider;
    PlayerEntityScript playerscript;

    private float defaultValue;

    private void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        playerscript = PlayerEntityScript.instance;
        defaultValue = slider.maxValue;
        playerscript.OnPlayerHealthChangedEvent += SetHealth;
        SetHealth();
    }

    public void SetHealth()
    {
        slider.value = playerscript.health / (float)playerscript.maxHealth * defaultValue;
    }

    private void OnDestroy()
    {
        playerscript.OnPlayerHealthChangedEvent -= SetHealth;
    }
}
