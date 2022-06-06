using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensitivitySetting : MonoBehaviour
{
    static readonly Floats.Item item = Floats.Item.mouse_sens;

    static Slider slider;
    static InputField inputField;

    private void Awake()
    {
        slider = gameObject.GetComponent<Slider>();
        inputField = gameObject.transform.GetChild(0).gameObject.GetComponent<InputField>();

        slider.minValue = 0.1f;
        slider.maxValue = 10.0f;

        slider.onValueChanged.AddListener(UpdateInputField);
        inputField.onEndEdit.AddListener(UpdateSlider);

        slider.value = Floats.Get(item);
        inputField.text = Floats.Get(Floats.Item.mouse_sens).ToString("f2");
    }

    void UpdateInputField(float value)
    {
        ChageSensi(value);
        inputField.text = Floats.Get(Floats.Item.mouse_sens).ToString("f2");
    }

    void UpdateSlider(string value)
    {
        if (float.TryParse(value, out var num))
        {
            ChageSensi(num);
            slider.value = Floats.Get(Floats.Item.mouse_sens);
        }
    }

    void ChageSensi(float value)
    {
        var setting = Floats.Settings[item];

        if (setting.ValidationCheck(value))
        {
            setting.SetValue(value);
        }
    }
}
