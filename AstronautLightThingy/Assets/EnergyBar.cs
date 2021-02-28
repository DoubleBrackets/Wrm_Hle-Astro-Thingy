using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    Slider slider;
    public static EnergyBar instance;

    private float defaultValue;

    public void Start()
    {
        slider = GetComponent<Slider>();
        instance = this;
        defaultValue = slider.maxValue;
        PlayerWeaponScript.instance.AddEnergy(0);
    }

    public void SetValue(float val)
    {
        slider.value = val * defaultValue;
    }

}
