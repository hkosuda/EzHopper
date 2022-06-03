using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindCommand : Command
{
    static List<Binding> keyBindingList;

    public BindCommand()
    {
        commandName = "bind";
        keyBindingList = new List<Binding>();

        Timer.Updated += UpdateMethod;
    }

    public override void CommandMethod(Tracer tracer, List<string> values)
    {
        if (values == null || values.Count < 3) { return; }
        if (keyBindingList == null) { keyBindingList = new List<Binding>(); }

        var keyDeltaValue = values[1];
        var command = GetCommand(values);

        foreach(KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
        {
            if (keyCode.ToString().ToLower() == keyDeltaValue)
            {
                keyBindingList.Add(new Binding(keyCode, 0.0f, command));
                return;
            }
        }

        if (float.TryParse(keyDeltaValue, out var num))
        {
            if (num > 0.0f)
            {
                keyBindingList.Add(new Binding(KeyCode.None, 1.0f, command));
            }

            else if (num < 0.0f)
            {
                keyBindingList.Add(new Binding(KeyCode.None, -1.0f, command));
                return;
            }

            else
            {
                tracer.AddMessage("マウスホイールの上下にコマンドを割り当てたい場合は，0でない値を指定してください．", Tracer.MessageLevel.error);
            }
        }

        // - inner function
        static string GetCommand(List<string> values)
        {
            var command = "";

            for(var n = 2; n < values.Count; n++)
            {
                command += values[n] + " ";
            }

            return command.TrimEnd();
        }
    }

    static void UpdateMethod(object obj, float dt)
    {
        if(keyBindingList == null) { return; }
        if (!Input.anyKeyDown && Input.mouseScrollDelta.y == 0.0f) { return; }

        foreach(var keybind in keyBindingList)
        {
            // key
            if (InputSystem.CheckInput(keybind.key, true))
            {
                CommandReceiver.RequestCommand(keybind.command, true);
            }
        }
    }

    class Binding
    {
        public Keyconfig.Key key;
        public string command;

        public Binding(KeyCode keyCode, float wheelDelta, string command)
        {
            key = new Keyconfig.Key(keyCode, wheelDelta);
            this.command = command;
        }
    }
}
