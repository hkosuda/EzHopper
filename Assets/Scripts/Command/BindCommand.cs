using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindCommand : Command
{
    static public List<Binding> KeyBindingList { get; private set; }

    public BindCommand()
    {
        commandName = "bind";
        description = "特定のキーにコマンドを割り当てる機能を提供します．\n" +
            "たとえば，'bind c anchor back'とコンソールで入力すると，Cキーを押した際に'begin ez_athletic'が実行されるようになります．" +
            "これにより，記録された座標に素早く戻ることができるようになります．\n" +
            "バインドを解除するには，unbindコマンドを使用します．\n" +
            "値を指定せずに，ただ単に'bind'と入力すると現在のバインドの設定をみることができます．" +
            "unbindコマンドでバインドを削除するまえに確認するようにしましょう．";

        // initialize method
        KeyBindingList = new List<Binding>();
        Timer.Updated += UpdateMethod;
    }

    public override List<string> AvailableValues(List<string> values)
    {
        if (values == null || values.Count == 0) { return new List<string>(); }

        if (values.Count < 3)
        {
            var available = new List<string>();

            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                available.Add(keyCode.ToString().ToLower());
            }

            available.Add("1");
            available.Add("-1");

            return available;
        }

        return new List<string>();
    }

    public override void CommandMethod(Tracer tracer, List<string> values)
    {
        if (values == null || values.Count == 0) { return; }

        if (values.Count == 1)
        {
            tracer.AddMessage(CurrentBindings(), Tracer.MessageLevel.normal);
            return;
        }

        else if (values.Count == 2)
        {
            tracer.AddMessage("キーのあとに，バインドするコマンドを指定してください．", Tracer.MessageLevel.error);
            return;
        }

        else if (values.Count > 2)
        {
            if (KeyBindingList == null) { KeyBindingList = new List<Binding>(); }

            var keyString = values[1];
            var key = GetKey(keyString, tracer);

            if (key == null) { return; }

            var command = GetCommand(values);
            KeyBindingList.Add(new Binding(key.keyCode, key.wheelDelta, command));

            tracer.AddMessage("バインドを追加しました：" + BindingInfo(KeyBindingList.Last()), Tracer.MessageLevel.normal);
        }

        // - inner function
        static string CurrentBindings()
        {
            if (KeyBindingList == null || KeyBindingList.Count == 0)
            {
                return "現在バインドされているコマンドはありません．";
            }

            var message = "現在バインドされているコマンドは以下の通りです．\n";

            for(var n = 0; n < KeyBindingList.Count; n++)
            {
                var binding = KeyBindingList[n];

                message += "\t\t[" + n.ToString() + "] " + binding.key.GetKeyString() + " : " + binding.command + "\n";
            }

            return message.TrimEnd(new char[1] { '\n' });
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

        // - inner function
        static Keyconfig.Key GetKey(string keyString, Tracer tracer)
        {
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                if (keyCode.ToString().ToLower() == keyString)
                {
                    return new Keyconfig.Key(keyCode);
                }
            }

            if (float.TryParse(keyString, out var num))
            {
                if (num > 0.0f)
                {
                    return new Keyconfig.Key(KeyCode.None, 1.0f);
                }

                else if (num < 0.0f)
                {
                    return new Keyconfig.Key(KeyCode.None, -1.0f);
                }

                else
                {
                    tracer.AddMessage("マウスホイールの上下にコマンドを割り当てたい場合は，0でない値を指定してください．", Tracer.MessageLevel.error);
                    return null;
                }
            }

            tracer.AddMessage(keyString + "を有効なキーに変換できません．", Tracer.MessageLevel.error);
            return null;
        }
    }

    // update method
    static void UpdateMethod(object obj, float dt)
    {
        if(KeyBindingList == null) { return; }
        if (!Input.anyKeyDown && Input.mouseScrollDelta.y == 0.0f) { return; }

        foreach(var keybind in KeyBindingList)
        {
            // key
            if (InputSystem.CheckInput(keybind.key, true))
            {
                CommandReceiver.RequestCommand(keybind.command, false);
            }
        }
    }

    static public void RemoveKeybind(int n, Tracer tracer)
    {
        if (KeyBindingList == null || KeyBindingList.Count == 0)
        {
            tracer.AddMessage("現在バインドは作成されていません．", Tracer.MessageLevel.error);
            return;
        }

        if (0 <= n && n < KeyBindingList.Count)
        {
            var bind = KeyBindingList[n];
            
            KeyBindingList.RemoveAt(n);
            tracer.AddMessage("バインドを削除しました：" + BindingInfo(bind), Tracer.MessageLevel.normal);
        }

        else
        {
            var message = n.ToString() + "は有効な値の範囲外です．有効な値の範囲は0から" + (KeyBindingList.Count - 1).ToString() + "までです．";
            tracer.AddMessage(message, Tracer.MessageLevel.error);
        }
    }

    static string BindingInfo(BindCommand.Binding binding)
    {
        return "key : " + binding.key.GetKeyString() + ", command : " + binding.command;
    }

    public class Binding
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
