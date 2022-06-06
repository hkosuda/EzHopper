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
            new AnchorCommand(),
            new BindCommand(),
            new NextCommand(),
            new PrevCommand(),
            new UnbindCommand(),
            new GodCommand(),
            new CrosshairCommand(),
        };

        foreach(var command in commandList)
        {
            CommandReceiver.AddCommand(command);
        }

        Bools.AddCommands();
        Floats.AddCommands();

#if UNITY_EDITOR
        CommandReceiver.RequestCommand("bind p anchor set", true);
        CommandReceiver.RequestCommand("bind v anchor back", true);
        CommandReceiver.RequestCommand("bind 1 recorder begin", true);
        CommandReceiver.RequestCommand("bind -1 recorder end", true);
        CommandReceiver.RequestCommand("bind r drecorder save", true);
#endif
    }

    public void Shutdown()
    {

    }

    public void Reset()
    {

    }
}
