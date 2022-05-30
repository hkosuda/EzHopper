using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleInputField : MonoBehaviour
{
    static public EventHandler<string> ValueUpdated { get; set; }

    static InputField inputField;

    private void Awake()
    {
        inputField = gameObject.GetComponent<InputField>();
        ActivateInputField();

        inputField.onValueChanged.AddListener(OnValueUpdatedMethod);
        inputField.onEndEdit.AddListener(OnEndEditMethod);
    }

    static void OnValueUpdatedMethod(string value)
    {
        ValueUpdated?.Invoke(null, value);
    }

    static void OnEndEditMethod(string value)
    {
        CommandReceiver.RequestCommand(value, true);
    }

    void Start()
    {
        SetEvent(1);
    }

    private void OnDestroy()
    {
        SetEvent(-1);
    }

    static void SetEvent(int indicator)
    {
        if (indicator > 0)
        {
            CommandReceiver.CommandRequestEnd += UpdateInputFieldText;
        }

        else
        {
            CommandReceiver.CommandRequestEnd -= UpdateInputFieldText;
        }
    }

    static void UpdateInputFieldText(object obj, Tracer tracer)
    {
        if (tracer.NoError)
        {
            inputField.text = "";
        }

        ActivateInputField();
    }

    static public void ActivateInputField()
    {
        if (inputField == null) { return; }
        inputField.ActivateInputField();
    }

    static public void ChangeValue(string value)
    {
        inputField.text = value;
        inputField.caretPosition = inputField.text.Length;
    }
}
