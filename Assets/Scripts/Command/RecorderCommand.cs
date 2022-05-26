using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecorderCommand : Command
{
    static readonly List<string> availableValues = new List<string>()
    {
        "begin", "end"
    };

    public RecorderCommand()
    {
        commandName = "recorder";
        description = "プレイヤーの現在位置を記録していく機能を提供します．";
    }

    public override bool CheckValues(Tracer tracer, List<string> values)
    {
        if (values.Count < 2)
        {
            NoValues(tracer);
            return false;
        }

        var value = values[1];

        if (!availableValues.Contains(value))
        {
            NotAvailable(tracer, value);
            return false;
        }

        return true;

        // - inner function
        static void NoValues(Tracer tracer)
        {
            tracer.AddMessage("'" + availableValues[0] + "' もしくは '" + availableValues[1] + "' を値として指定してください．", Tracer.MessageLevel.error);
        }

        // - inner function
        static void NotAvailable(Tracer tracer, string value)
        {
            tracer.AddMessage("'" + value + "' は値として不適切です．", Tracer.MessageLevel.error);
            tracer.AddMessage("'" + availableValues[0] + "' もしくは '" + availableValues[1] + "' を値として指定してください．", Tracer.MessageLevel.error);
        }
    }

    public override void CommandMethod(Tracer tracer, List<string> values)
    {
        if (values.Count < 2) { return; }

        var value = values[1];

        if (value == "begin")
        {
            PlayerRecorder.BeginRecording();
            return;
        }

        if (value == "end")
        {
            PlayerRecorder.FinishRecording();
            return;
        }
    }
}
