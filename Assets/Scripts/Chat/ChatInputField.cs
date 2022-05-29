using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatInputField : MonoBehaviour
{
    static InputField inputField;

    private void Awake()
    {
        inputField = gameObject.GetComponent<InputField>();
    }

    private void Update()
    {
        inputField.ActivateInputField();

        if (Input.GetKeyDown(KeyCode.Return))
        {
            SendChat(inputField.text);
        }
    }

    static void SendChat(string value)
    {
        if (inputField.text.Trim() == "") { return; }

        ChatMessages.SendChat(value, ChatMessages.Sender.player);
        inputField.text = "";
    }

    static public void Activate()
    {
        inputField.ActivateInputField();
    }
}
