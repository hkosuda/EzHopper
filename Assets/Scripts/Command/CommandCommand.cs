using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandCommand : Command
{
    public CommandCommand()
    {
        commandName = "command";
        description = "コマンドの一覧を表示します．詳細付きで一覧を表示するときは，'command description'を実行してください．";
    }

    public override List<string> AvailableValues(List<string> values)
    {
        if (values == null || values.Count == 0) { return new List<string>(); }

        if (values.Count < 3)
        {
            return new List<string>() { "description" };
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
            AddMessage(GetHelpText(false), Tracer.MessageLevel.normal, tracer, options);
            return;
        }

        if (values.Count == 2)
        {
            var value = values[1];

            if (value == "description")
            {
                AddMessage(GetHelpText(true), Tracer.MessageLevel.normal, tracer, options);
                return;
            }

            else
            {
                AddMessage("詳細つきで一覧を表示するときは'command description'，詳細を省いて一覧を表示するときは'command'を実行してください．", Tracer.MessageLevel.error, tracer, options);
                return;
            }
        }

        else
        {
            AddMessage("2個以上の値を指定することはできません．", Tracer.MessageLevel.error, tracer, options);
        }
    }

    static string GetHelpText(bool showDescription)
    {
        var text = "コマンド一覧：\n";

        foreach (var command in CommandReceiver.CommandList)
        {
            if (command.Value.commandType == CommandType.values) { continue; }

            if (showDescription)
            {
                text += "【" + command.Key + "】\n";
                text += command.Value.description + "\n\n";
            }

            else
            {
                text += command.Key + "\n";
            }
        }

        if (!showDescription)
        {
            text += "\n" + "詳細付きで一覧を表示するときは，'command description'を実行してください．";
        }

        return text.TrimEnd(new char[1] { '\n' });
    }
}
