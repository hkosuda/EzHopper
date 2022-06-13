using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeycheckCommand : Command
{
    static public bool Active { get; private set; } = false;

    public KeycheckCommand()
    {
        commandName = "keycheck";
        description = "入力したキーが，ゲーム内でどんな文字列として扱われるかを確認する機能を提供します．";
    }

    public override void CommandMethod(Tracer tracer, List<string> values, List<string> options)
    {
        if (values == null || values.Count == 0) { return; }

        if (values.Count == 1)
        {
            Active = true;
            AddMessage("入力待機状態になりました．調べたいキーを押してください．", Tracer.MessageLevel.emphasis, tracer, options);
        }

        else
        {
            AddMessage(ERROR_OverValues(0), Tracer.MessageLevel.error, tracer, options);
        }
    }

    static public void EchoInputKey(string keyString)
    {
        Active = false;
        ConsoleMessage.WriteLog("<color=lime>\t入力されたキーの名称：" + keyString.ToLower() + "</color>");
    }
}
