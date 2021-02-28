using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelBar : MonoBehaviour
{
    Slider slider;
    public static FuelBar instance;

    private float defaultValue;

    public void Start()
    {
        slider = GetComponent<Slider>();
        instance = this;
        defaultValue = slider.maxValue;
    }

    public void SetValue(float val)
    {
        slider.value = val * defaultValue;
    }
}
