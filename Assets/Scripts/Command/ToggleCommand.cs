using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleCommand : Command
{
    static public EventHandler<bool> ToggleUpdated { get; set; }
    static public List<ToggleGroup> ToggleGroupList = new List<ToggleGroup>();

    public ToggleCommand()
    {
        commandName = "toggle";
        description = "ふたつのコマンドを，トグルで実行する機能を提供します．";
        detail = "使用方法としては，'toggle r \"recorder start\" \"recorder end\"' のように 'toggle' の後にキーの名前，" +
            "そのあとにトグルで実行するコードをふたつ指定します．\n" +
            "上記を実行することで，Rキーを押すことで 'recorder start' と 'recorder end' を交互に実行することができます．\n" +
            "トグルの設定を削除するには，'toggle remove 0' のように 'toggle remove' の後に削除したい設定の番号を指定します．" +
            "番号およびトグルの設定を確認するには，'toggle' を実行してください．";

        SetEvent(1);
    }

    static void SetEvent(int indicator)
    {
        if (indicator > 0)
        {
            InGameTimer.Updated += UpdateMethod;
        }

        else
        {
            InGameTimer.Updated -= UpdateMethod;
        }
    }

    static void UpdateMethod(object obj, float dt)
    {
        if (ToggleGroupList == null) { return; }
        if (!Input.anyKeyDown && Input.mouseScrollDelta.y == 0.0f) { return; }

        foreach (var group in ToggleGroupList)
        {
            // key
            if (InputSystem.CheckInput(group.key, true))
            {
                group.Exec();
            }
        }
    }

    public override List<string> AvailableValues(List<string> values)
    {
        if (values == null || values.Count == 0) { return new List<string>(); }

        if (values.Count < 3)
        {
            var available = new List<string>();

            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                if (keyCode == KeyCode.None) { continue; }
                available.Add(keyCode.ToString().ToLower());
            }

            available.Add("1");
            available.Add("-1");
            available.Add("remove");

            return available;
        }

        else if (values.Count < 4)
        {
            if (values[1] == "remove")
            {
                var list = new List<string>();

                for(var n = 0; n < ToggleGroupList.Count - 1; n++)
                {
                    list.Add(n.ToString());
                }

                list.Add("all");
                return list;
            }

            else
            {
                return new List<string>();
            }
        }

        else
        {
            return new List<string>();
        }
    }

    // ex) toggle(0) t(1) /observer/start/(2) /observer/end/(3)
    // ex) toggle(0) remove(1) 3(2)
    public override void CommandMethod(Tracer tracer, List<string> values, List<string> options)
    {
        if (values == null || values.Count == 0) { return; }

        if (values.Count == 1)
        {
            CurrentBindingMessage(tracer, options);
        }

        else if (values.Count == 2)
        {
            AddMessage(ERROR_InvalidValues(), Tracer.MessageLevel.error, tracer, options);
        }

        else if (values.Count == 3)
        {
            var action = values[1];

            if (action == "remove")
            {
                var indexString = values[2];

                if (indexString == "all")
                {
                    ToggleGroupList = new List<ToggleGroup>();
                    AddMessage("トグルの設定をすべて削除しました．", Tracer.MessageLevel.normal, tracer, options);
                }

                else if (int.TryParse(indexString, out var index))
                {
                    TryRemove(index, tracer, options);
                }

                else
                {
                    AddMessage(ERROR_NotInteger(indexString), Tracer.MessageLevel.error, tracer, options);
                }
            }

            else
            {
                AddMessage(ERROR_InvalidValues(), Tracer.MessageLevel.error, tracer, options);
            }
        }

        else if (values.Count == 4)
        {
            var keyString = values[1];
            var key = Keyconfig.StringToKey(keyString);

            if (key == null)
            {
                AddMessage(ERROR_InvalidKey(keyString), Tracer.MessageLevel.error, tracer, options);
                AddMessage(ERROR_InvalidKeyAlert(), Tracer.MessageLevel.warning, tracer, options);
                return;
            }

            var command1 = CommandReceiver.UnpackGrouping(values[2]);
            var command2 = CommandReceiver.UnpackGrouping(values[3]);

            TryAddToggleGroup(key, command1, command2, tracer, options);
        }

        else
        {
            AddMessage(ERROR_OverValues(3), Tracer.MessageLevel.error, tracer, options);
        }

        ToggleUpdated?.Invoke(null, false);

        // - inner function
        static string ERROR_InvalidValues()
        {
            return "キーのあとにトグルで実行するコマンドを二つ指定するか，'remove'のあとに削除するトグル設定のインデックスもしくは'all'を指定してください．";
        }
    }

    static void CurrentBindingMessage(Tracer tracer, List<string> options)
    {
        var info = "";

        foreach(var group in ToggleGroupList)
        {
            info += "\t" + group.key.GetKeyString() + "\t| " + group.command1 + "\t| " + group.command2 + "\n";
        }

        if (info == "")
        {
            AddMessage("現在，トグル設定は存在しません．", Tracer.MessageLevel.normal, tracer, options);
        }

        else
        {
            info = "現在のトグル設定は以下の通りです．\n" + info;
            AddMessage(info, Tracer.MessageLevel.normal, tracer, options);
        }
    }

    static void TryRemove(int index, Tracer tracer, List<string> options)
    {
        var indexLim = ToggleGroupList.Count - 1;

        if (index < 0 || index > indexLim)
        {
            AddMessage(ERROR_OutOfRange(index, indexLim), Tracer.MessageLevel.error, tracer, options);
        }

        else
        {
            var group = ToggleGroupList[index];

            ToggleGroupList.RemoveAt(index);
            AddMessage("トグルの設定を削除しました．", Tracer.MessageLevel.normal, tracer, options);
            AddMessage("削除したトグル設定：" + group.Info(), Tracer.MessageLevel.normal, tracer, options);
        }
    }

    static void TryAddToggleGroup(Keyconfig.Key key, string command1, string command2, Tracer tracer, List<string> options)
    {
        var keyString = key.GetKeyString();

        foreach(var group in ToggleGroupList)
        {
            if (group.key.GetKeyString() == keyString)
            {
                AddMessage("同じキーに，複数のトグル設定を割り当てることはできません．", Tracer.MessageLevel.error, tracer, options);
                return;
            }
        }

        var g = new ToggleGroup(key, command1, command2);

        ToggleGroupList.Add(g);
        AddMessage("トグル設定を作成しました．", Tracer.MessageLevel.normal, tracer, options);
        AddMessage("作成されたトグル設定：" + g.Info(), Tracer.MessageLevel.normal, tracer, options);
    }

    public class ToggleGroup
    {
        public Keyconfig.Key key;

        public string command1;
        public string command2;

        bool toggleSwitch;

        public ToggleGroup(Keyconfig.Key key, string command1, string command2)
        {
            this.key = key;
            this.command1 = command1;
            this.command2 = command2;

            toggleSwitch = true;
        }

        public void Exec()
        {
            if (toggleSwitch)
            {
                CommandReceiver.RequestCommand(command1);
            }

            else
            {
                CommandReceiver.RequestCommand(command2);
            }

            toggleSwitch = !toggleSwitch;
        }

        public string Info()
        {
            return key.GetKeyString() + "\t| " + command1 + "\t| " + command2;
        }
    }
}
