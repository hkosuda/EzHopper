using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitCommand : Command
{
    public ExitCommand()
    {
        commandName = "exit";
        description = "コンソールを閉じます．";
    }

    public override void CommandMethod(Tracer tracer, List<string> values)
    {
        Console.CloseConsole();
    }
}
