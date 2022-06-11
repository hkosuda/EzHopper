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

    public override void CommandMethod(Tracer tracer, List<string> values, List<string> options)
    {
        if (values == null || values.Count == 0) { return; }

        if (values.Count == 1)
        {
            AddMessage(CurrentBindings(), Tracer.MessageLevel.normal, tracer, options);
        }

        else if (values.Count == 2)
        {
            AddMessage("キーのあとに，バインドするコマンドを指定してください．", Tracer.MessageLevel.error, tracer, options);
        }

        // ex) bind(0) i(1) /timer/stop/(2)
        else if (values.Count == 3)
        {
            if (KeyBindingList == null) { KeyBindingList = new List<Binding>(); }

            var keyString = values[1];
            var key = GetKey(keyString, tracer, options);

            if (key == null) 
            {
                AddMessage(keyString + "を有効なキーに変換できません．", Tracer.MessageLevel.error, tracer, options);
                return;
            }

            var command = CommandReceiver.UnpackGrouping(values[2]);

            if (CheckDuplication(key, command))
            {
                AddMessage("すでに同じ内容のバインドが存在します．", Tracer.MessageLevel.error, tracer, options);
            }

            else
            {
                KeyBindingList.Add(new Binding(key.keyCode, key.wheelDelta, command));
                AddMessage("バインドを追加しました：" + BindingInfo(KeyBindingList.Last()), Tracer.MessageLevel.normal, tracer, options);
            }
        }

        else
        {
            AddMessage(ERROR_OverValues(2), Tracer.MessageLevel.error, tracer, options);
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
        static Keyconfig.Key GetKey(string keyString, Tracer tracer, List<string> options)
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
                    AddMessage("マウスホイールの上下にコマンドを割り当てたい場合は，0でない値を指定してください．", Tracer.MessageLevel.error, tracer, options);
                    return null;
                }
            }

            AddMessage(keyString + "を有効なキーに変換できません．", Tracer.MessageLevel.error, tracer, options);
            return null;
        }

        // - inner function
        static bool CheckDuplication(Keyconfig.Key key, string command)
        {
            if (KeyBindingList == null) { KeyBindingList = new List<Binding>(); }

            foreach(var keybind in KeyBindingList)
            {
                if (keybind.key.GetKeyString() == key.GetKeyString() && keybind.command == command)
                {
                    return true;
                }
            }

            return false;
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
                CommandReceiver.RequestCommand(keybind.command);
            }
        }
    }

    static public void RemoveKeybind(List<int> indexes, Tracer tracer, List<string> options)
    {
        if (KeyBindingList == null || KeyBindingList.Count == 0)
        {
            AddMessage("現在バインドは作成されていません．", Tracer.MessageLevel.error, tracer, options);
            return;
        }

        var indexLim = KeyBindingList.Count - 1;

        foreach(var index in indexes)
        {
            if (index < 0 || index > indexLim)
            {
                AddMessage(ERROR_OutOfRange(index, indexLim), Tracer.MessageLevel.error, tracer, options);
            }
        }

        if (!tracer.NoError) { return; }
        
        foreach(var index in indexes)
        {
            var bind = KeyBindingList[index];

            KeyBindingList.RemoveAt(index);
            AddMessage("バインドを削除しました：" + BindingInfo(bind), Tracer.MessageLevel.normal, tracer, options);
        }
    }

    static public void RemoveAll(Tracer tracer, List<string> options)
    {
        KeyBindingList = new List<Binding>();
        AddMessage("バインドをすべて削除しました．", Tracer.MessageLevel.normal, tracer, options);
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
