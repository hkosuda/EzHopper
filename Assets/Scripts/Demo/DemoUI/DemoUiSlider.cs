using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoUiSlider : MonoBehaviour
{
    static Slider slider;

    void Start()
    {
        slider = gameObject.GetComponent<Slider>();

        slider.minValue = 0.0f;
        slider.maxValue = DemoManager.Duration;

        slider.onValueChanged.AddListener(ChangePastTimeDirectory);
    }

    private void Update()
    {
        slider.value = DemoManager.PastTime;
    }

    static void ChangePastTimeDirectory(float value)
    {
        DemoManager.ChangePastTime(value, true);
        slider.value = DemoManager.PastTime;
    }
}
