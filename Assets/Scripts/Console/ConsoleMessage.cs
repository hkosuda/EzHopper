using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleMessage : MonoBehaviour
{
    static public EventHandler<bool> LogUpdated { get; set; }

    static string consoleLog = "wwww";
    static Text consoleLogText;

    private void Awake()
    {
        consoleLogText = gameObject.GetComponent<Text>();
        UpdateLogText();
    }

    static public void Initialize()
    {
        SetEvent(1);
    }

    static public void Shutdown()
    {
        SetEvent(-1);
    }

    static void SetEvent(int indicator)
    {
        if (indicator > 0)
        {
            CommandReceiver.CommandRequestBegin += WriteSentence;
            CommandReceiver.CommandRequestEnd += WriteTracerMessages;
        }

        else
        {
            CommandReceiver.CommandRequestBegin -= WriteSentence;
            CommandReceiver.CommandRequestEnd -= WriteTracerMessages;
        }
    }

    static void WriteSentence(object obj, string sentence)
    {
        TrimEndReturn();
        consoleLog += "\n";
        consoleLog += "> " + sentence;

        UpdateLogText();
    }

    static void WriteTracerMessages(object obj, Tracer tracer)
    {
        TrimEndReturn();
        consoleLog += "\n";
        consoleLog += tracer.GetFullMessage();
        TrimEndReturn();

        UpdateLogText();
    }

    static void TrimEndReturn()
    {
        consoleLog.TrimEnd(new char[] { '\n' });
    }

    static void UpdateLogText()
    {
        if (consoleLogText == null) { return; }
        consoleLogText.text = consoleLog;

        LogUpdated?.Invoke(null, false);
    }

    static public void WriteLog(string log)
    {
        TrimEndReturn();
        consoleLog += "\n";
        consoleLog += log;

        if (consoleLogText != null) { UpdateLogText(); }
    }
}
