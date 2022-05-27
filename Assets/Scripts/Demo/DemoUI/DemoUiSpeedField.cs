using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoUiSpeedField : MonoBehaviour
{
    static InputField field;

    void Start()
    {
        field = gameObject.GetComponent<InputField>();
        field.onEndEdit.AddListener(ChangeSpeed);

        field.text = DemoManager.PlaySpeed.ToString("f2");
    }

    static void ChangeSpeed(string text)
    {
        if (float.TryParse(text, out var num))
        {
            DemoManager.ChangeSpeed(num);
        }

        field.text = DemoManager.PlaySpeed.ToString("f2");
    }
}
