using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCommand : Command
{
    public ClearCommand()
    {
        commandName = "clear";
        description = "コンソールのメッセージをすべて削除します．";
        detail = "コンソールが表示できる文字数には制限があります．'invoke'に割り当てたコードを'-echo'やオプションなしで実行していると，" +
            "知らぬ間にメッセージの文字数がかなりの数になってしまいます．" +
            "文字数が限界まで到達するとメッセージが表示されなくなります．そうなった場合は，clearを使用してメッセージをすべて削除してください．";
    }

    public override void CommandMethod(Tracer tracer, List<string> values, List<string> options)
    {
        if (values == null || values.Count == 0) { return; }

        if (values.Count == 1)
        {
            ConsoleMessage.ClearLog();
        }

        else
        {
            AddMessage(ERROR_OverValues(0), Tracer.MessageLevel.error, tracer, options);
        }
    }
}
