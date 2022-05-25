using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleInputField : MonoBehaviour
{
    static public EventHandler<string> ValueUpdated { get; private set; }

    static InputField inputField;

    private void Awake()
    {
        inputField = gameObject.GetComponent<InputField>();
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
    }
}
