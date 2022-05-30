using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensitivitySetting : MonoBehaviour
{
    static Slider slider;
    static InputField inputField;

    private void Awake()
    {
        slider = gameObject.GetComponent<Slider>();
        inputField = gameObject.transform.GetChild(0).gameObject.GetComponent<InputField>();

        slider.minValue = 0.1f;
        slider.maxValue = 5.0f;

        slider.onValueChanged.AddListener(UpdateInputField);
        inputField.onEndEdit.AddListener(UpdateSlider);

        slider.value = ClientParams.MouseSensi;
        inputField.text = ClientParams.MouseSensi.ToString("f2");
    }

    void UpdateInputField(float value)
    {
        ChageSensi(value);
        inputField.text = ClientParams.MouseSensi.ToString("f2");
    }

    void UpdateSlider(string value)
    {
        if (float.TryParse(value, out var num))
        {
            ChageSensi(num);
            slider.value = ClientParams.MouseSensi;
        }
    }

    void ChageSensi(float value)
    {
        ClientParams.SetSensi(value);
    }
}
