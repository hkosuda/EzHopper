using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandIndex : MonoBehaviour
{
    static Text paragraph1;
    static Text paragraph2;
    static Text paragraph3;

    private void Awake()
    {
        paragraph1 = GetText(1);
        paragraph2 = GetText(3);
        paragraph3 = GetText(5);

        // - inner function
        Text GetText(int n)
        {
            return gameObject.transform.GetChild(n).gameObject.GetComponent<Text>();
        }
    }

    void Start()
    {
        UpdateContent();
    }

    void UpdateContent()
    {
        var p1 = "";

        foreach(var commandPair in CommandReceiver.CommandList)
        {
            if (commandPair.Value.commandType == Command.CommandType.values) { continue; }

            p1 += "<color=lime>" + commandPair.Key + "</color>\n";
            p1 += "�T�v�F" + CommandCommand.Paragraph(commandPair.Value.description) + "\n";
            p1 += "�ڍׁF" + CommandCommand.Paragraph(commandPair.Value.detail) + "\n\n";
        }

        paragraph1.text = p1.TrimEnd(new char[] { '\n' });

        var p2 = "";

        foreach (var setting in Bools.Settings)
        {
            p2 += "<color=lime>" + setting.Key.ToString().ToLower() + "</color>\n";
            p2 += "�T�v�F" + CommandCommand.Paragraph(setting.Value.Description) + "\n\n";
        }

        paragraph2.text = p2.TrimEnd(new char[] { '\n' });

        var p3 = "";

        foreach(var setting in Floats.Settings)
        {
            p3 += "<color=lime>" + setting.Key.ToString().ToLower() + "</color>\n";
            p3 += "�T�v�F" + CommandCommand.Paragraph(setting.Value.Description) + "\n\n";
        }

        paragraph3.text = p3.TrimEnd(new char[] { '\n' });
    }
}
