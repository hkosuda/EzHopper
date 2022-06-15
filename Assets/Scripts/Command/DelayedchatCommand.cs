using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedchatCommand : Command
{
    public DelayedchatCommand()
    {
        commandName = "delayedchat";
        description = "遅延を入れてチャットを送信します．";
        detail = "メッセージを送信するには，delayedchat 0.5 1.5 \"hello\"' のように，1番目の値に遅延させる時間の最小値，" +
            "2番目の値に遅延させる時間の最大値を指定します．3番目の値に送信するメッセージを指定します．\n" +
            "メッセージは，マップが切り替わると破棄されます．";
    }

    // ex) delayedchat(0) 0.8(1) 2.0(2) "noob"(3)
    public override void CommandMethod(Tracer tracer, List<string> values, List<string> options)
    {
        if (values == null || values.Count == 0) { return; }

        else if (values.Count < 3)
        {
            AddMessage("遅延させる時間の最小値，最大値を指定してください．", Tracer.MessageLevel.error, tracer, options);
        }

        else if (values.Count == 3)
        {
            AddMessage("送信するメッセージを指定してください．", Tracer.MessageLevel.error, tracer, options);
        }

        else if (values.Count == 4)
        {
            var minString = values[1];
            var maxString = values[2];

            if (!float.TryParse(minString, out var min)) { AddMessage(minString + "を数値に変換できません．", Tracer.MessageLevel.error, tracer, options); return; }
            if (!float.TryParse(maxString, out var max)) { AddMessage(maxString + "を数値に変換できません．", Tracer.MessageLevel.error, tracer, options); return; }

            if (min < 0 || max < 0)
            {
                AddMessage("負の値を指定することはできません．", Tracer.MessageLevel.error, tracer, options);
                return;
            }

            if (min > max)
            {
                AddMessage("2番目の値には，1番目の値以上の値を指定してください．", Tracer.MessageLevel.error, tracer, options);
                return;
            }

            var group = values[3];
            var message = ChatCommand.UnpackGroup(group);

            ToxicSystem.SendDelayedToxicChat(message, min, max);
        }

        else
        {
            AddMessage(ERROR_OverValues(3), Tracer.MessageLevel.error, tracer, options);
        }
    }
}
