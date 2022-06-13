using System;
using System.Linq;
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

        var enterButton = gameObject.transform.GetChild(1).gameObject.GetComponent<Button>();
        enterButton.onClick.AddListener(RequestCommand);
    }

    static void OnValueUpdatedMethod(string value)
    {
        ValueUpdated?.Invoke(null, value);
    }

    static void RequestCommand()
    {
        var value = inputField.text;
        if (value.Trim() == "") { return; }

        CommandReceiver.RequestCommand(value);
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
            CommandReceiver.UnknownCommandRequest += UpdateInputFieldOnUnknownCommand;
            CommandReceiver.CommandRequestEnd += UpdateInputFieldOnRequestEnd;
        }

        else
        {
            CommandReceiver.UnknownCommandRequest -= UpdateInputFieldOnUnknownCommand;
            CommandReceiver.CommandRequestEnd -= UpdateInputFieldOnRequestEnd;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            RequestCommand();
        }
    }

    static void UpdateInputFieldOnUnknownCommand(object obj, string sentence)
    {
        UpdateInputField(false);
    }

    static void UpdateInputFieldOnRequestEnd(object obj, Tracer tracer)
    {
        UpdateInputField(tracer.NoError);
    }

    static void UpdateInputField(bool noError)
    {
        if (noError)
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

    static public void AddValue(string value)
    {
        var currentValue = inputField.text;

        var values = CommandReceiver.GetValues(currentValue);
        var options = CommandReceiver.GetOptions(currentValue);

        if (values == null || values.Count == 0) { inputField.text = ""; return; }

        string corrected;

        // pre processing
        if (currentValue.EndsWith(" "))
        {
            corrected = CorrectValues(values);
        }

        else
        {
            corrected = CorrectValues(values, true);
        }

        // add
        if (values.Last().StartsWith("\""))
        {
            corrected += "\"" + value;
        }

        else
        {
            corrected += value;
        }

        corrected = AddOptions(corrected, options);
        inputField.text = corrected;
        inputField.caretPosition = inputField.text.Length;

        // - inner function
        static string CorrectValues(List<string> values, bool offset = false)
        {
            if (values == null || values.Count == 0) { return ""; }

            var text = "";

            if (offset)
            {
                for (var n = 0; n < values.Count - 1; n++)
                {
                    text += values[n] + " ";
                }
            }

            else
            {
                for (var n = 0; n < values.Count; n++)
                {
                    text += values[n] + " ";
                }
            }

            return text.TrimEnd() + " ";
        }

        static string AddOptions(string corrected, List<string> options)
        {
            corrected = corrected.TrimEnd() + " ";

            if (options == null || options.Count == 0)
            {
                return corrected;
            }

            else
            {
                return corrected + options.Last() + " ";
            }
        }
    }
}
