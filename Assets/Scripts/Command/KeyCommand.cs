using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCommand : Command
{
    public KeyCommand()
    {
        commandName = "key";
        description = "基本的な動作に関するキーの割り当てを変更，確認する機能を提供します．";
    }

    public override List<string> AvailableValues(List<string> values)
    {
        if (values == null || values.Count == 0)
        {
            return new List<string>();
        }

        else if (values.Count < 3)
        {
            var list = new List<string>();

            foreach(Keyconfig.KeyAction action in Enum.GetValues(typeof(Keyconfig.KeyAction)))
            {
                if (action == Keyconfig.KeyAction.none) { continue; }
                list.Add(action.ToString().ToLower());
            }

            return list;
        }

        else if (values.Count < 4)
        {
            var available = new List<string>();

            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                if (keyCode == KeyCode.None) { continue; }
                available.Add(keyCode.ToString().ToLower());
            }

            available.Add("1");
            available.Add("-1");

            return available;
        }

        else
        {
            return new List<string>();
        }
    }

    public override void CommandMethod(Tracer tracer, List<string> values, List<string> options)
    {
        if (values == null || values.Count == 0) { return; }

        if (values.Count == 1)
        {
            CurrentKeyInfo(tracer, options);
        }

        else if (values.Count == 2)
        {
            AddMessage("対象となるアクションと，キーの2つの値が必要です．", Tracer.MessageLevel.error, tracer, options);
        }

        else if (values.Count == 3)
        {
            var actionString = values[1];
            var keyAction = GetKeyAction(actionString);

            if (keyAction == Keyconfig.KeyAction.none)
            {
                AddMessage(keyAction + "に対応するアクションはありません．", Tracer.MessageLevel.error, tracer, options);
                return;
            }

            var keyString = values[2];
            var key = GetKey(keyString);

            if (key == null)
            {
                AddMessage(keyString + "に対応するキーは存在しません．", Tracer.MessageLevel.error, tracer, options);
                return;
            }

            Keyconfig.SetKey(keyAction, key.keyCode, key.wheelDelta);
            AddMessage("キーを変更しました．", Tracer.MessageLevel.normal, tracer, options);
        }

        else
        {
            AddMessage(ERROR_OverValues(2), Tracer.MessageLevel.error, tracer, options);
        }
    }

    static void CurrentKeyInfo(Tracer tracer, List<string> options)
    {
        AddMessage("現在のキー設定は以下の通りです．", Tracer.MessageLevel.normal, tracer, options);

        foreach(var key in Keyconfig.KeybindList)
        {
            var info = key.Key.ToString() + "\t| " + key.Value.GetKeyString();
            AddMessage(info, Tracer.MessageLevel.normal, tracer, options, 2);
        }
    }

    static Keyconfig.KeyAction GetKeyAction(string str)
    {
        foreach(Keyconfig.KeyAction action in Enum.GetValues(typeof(Keyconfig.KeyAction)))
        {
            if (action == Keyconfig.KeyAction.none) { continue; }

            if (action.ToString().ToLower() == str)
            {
                return action;
            }
        }

        return Keyconfig.KeyAction.none;
    }

    static Keyconfig.Key GetKey(string str)
    {
        if (int.TryParse(str, out var num))
        {
            if (num > 0)
            {
                return new Keyconfig.Key(KeyCode.None, 1);
            }

            else if (num < 0)
            {
                return new Keyconfig.Key(KeyCode.None, -1);
            }

            else
            {
                return null;
            }
        }

        else
        {
            foreach(KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                if (keyCode.ToString().ToLower() == str)
                {
                    return new Keyconfig.Key(keyCode);
                }
            }
        }

        return null;
    }
}
