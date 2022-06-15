using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatCommand : Command
{
    public ChatCommand()
    {
        commandName = "chat";
        description = "チャットでメッセージを送信します．";
        detail = "たとえば'chat \"hello\"を実行すると，画面の左下に'hello'が表示されます．\n" +
            "シンボルと組み合わせることで，特定のイベントが発生したときにカスタムメッセージを表示できます．" +
            "たとえば，'invoke add on_enter_next_checkpoint \"chat 経過時間：%time%\"' を実行すると，" +
            "次のチェックポイントに到達したときに経過時間を表示できます．";
    }

    public override void CommandMethod(Tracer tracer, List<string> values, List<string> options)
    {
        if (values == null || values.Count == 0) { return; }

        if (values.Count == 1)
        {
            AddMessage("チャットで送信するメッセージを指定してください．", Tracer.MessageLevel.error, tracer, options);
        }

        else if (values.Count == 2)
        {
            var group = values[1];
            var message = UnpackGroup(group);

            ChatMessages.SendChat(message, ChatMessages.Sender.system);
        }

        else
        {
            AddMessage(ERROR_OverValues(1), Tracer.MessageLevel.error, tracer, options);
        }
    }

    static public string UnpackGroup(string group)
    {
        var message = CommandReceiver.UnpackGrouping(group);
        message = CommandReceiver.ReplaceSymbol(message);

        return message;
    }
}
