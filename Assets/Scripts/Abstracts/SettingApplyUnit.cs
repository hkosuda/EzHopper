using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;

public abstract class SettingApplyUnit : MonoBehaviour
{
    static protected readonly List<string> initializer = new List<string>()
    {
        "unbind all",
        "toggle remove all",
        "invoke remove_all all",
    };

    protected GameObject applyButtonBody;

    private void Start()
    {
        SetEvent(1);
    }

    private void OnDestroy()
    {
        SetEvent(-1);
    }

    void SetEvent(int indicator)
    {
        if (indicator > 0)
        {
            DE_Shooter.ShootingHit += RunCommands;
        }

        else
        {
            DE_Shooter.ShootingHit -= RunCommands;
        }
    }

    protected virtual void RunCommands(object obj, RaycastHit hit)
    {
        ConsoleMessage.WriteLog("<color=lime>コマンドの自動実行を開始しました．</color>");

        foreach (var initCommand in initializer)
        {
            CommandReceiver.RequestCommand(initCommand);
        }
    }

    protected void Initialize(string description, List<string> commandList)
    {
        applyButtonBody = gameObject.transform.GetChild(0).GetChild(0).gameObject;

        var descriptionText = GetText(1);
        descriptionText.text = description;

        var commandText = GetText(2);
        commandText.text = CommandText(commandList);

        // - inner function
        TextMeshProUGUI GetText(int n)
        {
            return gameObject.transform.GetChild(n).GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        }

        // - inner function
        string CommandText(List<string> commandList)
        {
            var text = "// init\n";

            foreach(var initCommand in initializer)
            {
                text += "> " + initCommand + "\n";
            }

            if (commandList == null) { return text; }

            text += "\n// settings\n";

            foreach(var command in commandList)
            {
                text += "> " + command + "\n";
            }

            return text;
        }
    }

    static protected string RunCommandText(List<string> commandList)
    {
        var text = "";

        foreach(var initCommand in initializer)
        {
            text += "> " + initCommand + "\n";
        }

        foreach(var command in commandList)
        {
            text += "> " + command + "\n";
        }

        return text.TrimEnd(new char[] {'\n'});
    }
}
