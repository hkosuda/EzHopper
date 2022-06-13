using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using UnityEngine;

public class CopyCommand : Command
{
    public CopyCommand()
    {
        commandName = "copy";
        description = "現在の設定を再現するためのコマンドをクリップボードにコピーします．";
    }

    public override void CommandMethod(Tracer tracer, List<string> values, List<string> options)
    {
        if (values == null || values.Count == 0) { return; }

        if (values.Count == 1)
        {
            var command = GetCommand();
            GUIUtility.systemCopyBuffer = command;

            AddMessage("現在の設定を復元するためのコマンドの一覧をクリップボードにコピーしました．", Tracer.MessageLevel.normal, tracer, options);
        }

        else
        {
            AddMessage(ERROR_OverValues(0), Tracer.MessageLevel.error, tracer, options);
        }
    }

    static string GetCommand()
    {
        var command = "";

        command += "// initialize\n";
        command += "default";
        command += "unbind all";
        command += "invoke remove_all all";


        command += GetSettingCommand();
        command += "\n";
        command += GetKeybindCommand();
        command += "\n";
        command += GetIgniteCommand();

        return command;
    }

    static string GetSettingCommand()
    {
        var command = "// values\n";

        foreach(var setting in Bools.Settings)
        {
            if (setting.Value.CurrentValue == setting.Value.DefaultValue) { continue; }

            if (setting.Value.CurrentValue)
            {
                command += setting.Key.ToString() + " 1\n"; 
            }

            else
            {
                command += setting.Key.ToString() + " 0\n";
            }
        }

        foreach(var setting in Floats.Settings)
        {
            if (setting.Value.CurrentValue == setting.Value.DefaultValue) { continue; }

            command += setting.Key.ToString() + " " + setting.Value.CurrentValue.ToString() + "\n";
        }

        return command;
    }

    static string GetIgniteCommand()
    {
        var command = "// invoke\n";

        foreach(var pair in InvokeCommand.BindingListList)
        {
            var commandList = pair.Value;

            foreach(var c in commandList)
            {
                command += "invoke add " + pair.Key.ToString() + " \"" + c + "\"\n";
            }
        }

        return command;
    }

    static string GetKeybindCommand()
    {
        var command = "// bind\n";

        foreach(var keybind in BindCommand.KeyBindingList)
        {
            if (keybind.key.keyCode == KeyCode.None)
            {
                if (keybind.key.wheelDelta > 0)
                {
                    command += "bind 1 \"" + keybind.command + "\"\n";
                }

                else
                {
                    command += "bind -1 \"" + keybind.command + "\"\n";
                }
            }

            else
            {
                command += "bind " + keybind.key.GetKeyString().ToLower() + " \"" + keybind.command + "\"\n";
            }
        }

        return command;
    }
}
