using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandInitializer : IKernelManager
{
    static List<Command> commandList;

    public void Initialize()
    {
        commandList = new List<Command>()
        {
            new DRecorderCommand(),
            new RecorderCommand(),
            new ExitCommand(),
            new DemoCommand(),
        };

        foreach(var command in commandList)
        {
            CommandReceiver.AddCommand(command);
        }
    }

    public void Shutdown()
    {

    }

    public void Reset()
    {

    }
}
