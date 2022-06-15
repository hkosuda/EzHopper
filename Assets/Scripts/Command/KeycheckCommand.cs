using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeycheckCommand : Command
{
    static public bool Active { get; private set; } = false;

    public KeycheckCommand()
    {
        commandName = "keycheck";
        description = "入力したキーが，ゲーム内でどのような文字列として扱われるかを確認する機能を提供します．";
        detail = "使い方としては，'keycheck' を実行したあとで任意のキーを押します．ゲームが入力を受け付けると，" +
            "コンソールに押したキーに対応する文字列が表示されます．'bind' などを使用したいもののキーの名称がわからないときに使用しましょう．\n" +
            "なお，キーの名称の多くはUnity（このゲームの作成に使用したゲームエンジンの名称）のKeyCodeをすべて小文字になおしたものとなっています．" +
            "そのため，keycheckコマンドによる確認以外にもUnityのスクリプトリファレンスが役に立つかもしれません．";
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
