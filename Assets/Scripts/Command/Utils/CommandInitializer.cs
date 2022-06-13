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
            new RecorderCommand(),
            new ExitCommand(),
            new DemoCommand(),
            new BeginCommand(),
            new AnchorCommand(),
            new BindCommand(),
            new NextCommand(),
            new PrevCommand(),
            new UnbindCommand(),
            new ObserverCommand(),
            new CrosshairCommand(),
            new ReplayCommand(),
            new GhostCommand(),
            new CommandCommand(),
            new SettingsCommand(),
            new BackCommand(),
            new OverrideCommand(),
            new InvokeCommand(),
            new TimerCommand(),
            new CopyCommand(),
            new PasteCommand(),
            new DefaultCommand(),
            new KeyCommand(),
            new ToggleCommand(),
            new KeycheckCommand(),
            new ChatCommand(),
            new DelayedchatCommand(),
            new CounterCommand(),
        };

#if UNITY_EDITOR
        commandList.Add(new DRecorderCommand());
#endif

        foreach(var command in commandList)
        {
            CommandReceiver.AddCommand(command);
        }

        Bools.AddCommands();
        Floats.AddCommands();

        
    }

    public void Shutdown()
    {

    }

    public void Reset()
    {
        foreach(var command in commandList)
        {
            command.OnMapInitialized();
        }
    }
}
