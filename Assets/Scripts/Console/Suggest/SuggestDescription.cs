using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuggestDescription : MonoBehaviour
{
    static GameObject frame;
    static Text descriptionText;

    private void Awake()
    {
        frame = gameObject.transform.GetChild(0).gameObject;
        descriptionText = gameObject.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<Text>();

        frame.SetActive(false);
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
            ConsoleInputField.ValueUpdated += ShowDescription;
        }

        else
        {
            ConsoleInputField.ValueUpdated += ShowDescription;
        }
    }

    static void ShowDescription(object obj, string value)
    {
        var command = GetCommand(value);
        if (command == null) { frame.SetActive(false); return; }

        frame.SetActive(true);
        descriptionText.text = GetDescription(command, value);

        // - inner function
        static Command GetCommand(string  value)
        {
            value = value.TrimStart();

            foreach (var command in CommandReceiver.CommandList.Values)
            {
                if (value.StartsWith(command.commandName + " "))
                {
                    return command;
                }
            }

            return null;
        }
        
        // - inner function
        static string GetDescription(Command command, string value)
        {
            var description = "";
            var values = CommandReceiver.GetValues(value);

            description = AddLine(description, "[" + command.commandName + "]", true);
            description = AddLine(description, "-----------------------------------------------------", false);


            if (command.description != "")
            {
                description = AddLine(description, "äTóv", true);
                description = AddLine(description, command.description);
            }

            if (command.detail != "")
            {
                description = AddLine(description, "è⁄ç◊", true);
                description = AddLine(description, command.detail);
            }

            return description;

            static string AddLine(string description, string content, bool color = false)
            {
                if (color) { return description + "<color=lime>" + content + "</color>\n"; }
                return description + content + "\n";
            }
        }
    }
}
