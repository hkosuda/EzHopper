using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitCommand : Command
{
    public ExitCommand()
    {
        commandName = "exit";
        description = "コンソールを閉じます．";
        detail = "コンソールを閉じます．";
    }

    public override void CommandMethod(Tracer tracer, List<string> values, List<string> options)
    {
        if (values == null || values.Count == 0) { return; }

        if (values.Count == 1)
        {
            Console.CloseConsole();
        }

        else
        {
            AddMessage(ERROR_OverValues(0), Tracer.MessageLevel.error, tracer, options);
        }
    }
}
