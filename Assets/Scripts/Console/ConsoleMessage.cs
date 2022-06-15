using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleMessage : MonoBehaviour
{
    static public EventHandler<bool> LogUpdated { get; set; }

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
            CommandReceiver.UnknownCommandRequest += WriteUnkownCommandMessages;
            CommandReceiver.CommandRequestEnd += WriteTracerMessages;
        }

        else
        {
            CommandReceiver.CommandRequestBegin -= WriteSentence;
            CommandReceiver.UnknownCommandRequest -= WriteUnkownCommandMessages;
            CommandReceiver.CommandRequestEnd -= WriteTracerMessages;
        }
    }

    static void WriteSentence(object obj, string sentence)
    {
        TrimEndReturn();

        var message = "> " + sentence;
        ProcessMessages(message, sentence);

        UpdateLogText();
    }

    static void WriteUnkownCommandMessages(object obj, string sentence)
    {
        TrimEndReturn();

        var values = CommandReceiver.GetValues(sentence);
        if (values == null || values.Count == 0) { return; }

        var message = "<color=red>" + values[0] + "というコマンドは存在しません．" + "</color>";
        ProcessMessages(message, sentence);

        UpdateLogText();
    }

    static void WriteTracerMessages(object obj, Tracer tracer)
    {
        var tracerMessage = tracer.ConsoleMessage();
        if (tracerMessage.Trim() == "") { return; }

        TrimEndReturn();
        consoleLog += "\n";
        consoleLog += tracerMessage;
        TrimEndReturn();

        UpdateLogText();
    }

    static void ProcessMessages(string message, string sentence)
    {
        var options = CommandReceiver.GetOptions(sentence);

        if (Tracer.CheckOption(Tracer.Option.echo, options))
        {
            AddMessageToLog(message);
        }

        else if (!Tracer.CheckOption(Tracer.Option.mute, options) && !Tracer.CheckOption(Tracer.Option.flash, options))
        {
            AddMessageToLog(message);
        }

        // - inner function
        static void AddMessageToLog(string message)
        {
            consoleLog += "\n" + message;
        }
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

    static public void ClearLog()
    {
        consoleLog = "";

        UpdateLogText();
    }

    static string consoleLog = "<color=lime> - EzHopper ver 1.0 - </color>\n" +
        "<color=orange>" +
        "Tips：\n" +
        "\tコマンドの一覧を確認するには，'command'を実行してください．\n" +
        "\t設定に関するコマンドを確認するには'settings'を実行してください．\n" +
        "\t説明まで表示するには，どちらも'description'をつけて実行してください（'command description'など）．\n" +
        "\t'exit'でコンソールを閉じることができます．\n" +
        "</color>";
}
