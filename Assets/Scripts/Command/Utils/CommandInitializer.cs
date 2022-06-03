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
            new BeginCommand(),
            new ImNoobCommand(),
            new AnchorCommand(),
            new BindCommand(),
            new NextCommand(),
            new PrevCommand(),
        };

        foreach(var command in commandList)
        {
            CommandReceiver.AddCommand(command);
        }

        CommandReceiver.RequestCommand("bind p anchor set", true);
        CommandReceiver.RequestCommand("bind v anchor back", true);
        CommandReceiver.RequestCommand("bind 1 recorder begin", true);
        CommandReceiver.RequestCommand("bind -1 recorder end", true);
        CommandReceiver.RequestCommand("bind m drecorder save", true);
    }

    public void Shutdown()
    {

    }

    public void Reset()
    {

    }
}
