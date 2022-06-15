using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;

public abstract class SettingApplyUnit : MonoBehaviour
{
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
            DE_Shooter.ShootingHit += OnShot;
        }

        else
        {
            DE_Shooter.ShootingHit -= OnShot;
        }
    }

    protected abstract void OnShot(object obj, RaycastHit hit);

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
            var text = "";
            if (commandList == null) { return text; }

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

        foreach(var command in commandList)
        {
            text += "> " + command + "\n";
        }

        return text.TrimEnd(new char[] {'\n'});
    }
}
