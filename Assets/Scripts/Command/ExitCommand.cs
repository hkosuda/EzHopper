using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitCommand : Command
{
    public ExitCommand()
    {
        commandName = "exit";
        description = "�R���\�[������܂��D";
    }

    public override void CommandMethod(Tracer tracer, List<string> values)
    {
        Console.CloseConsole();
    }
}
