using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsCommand : Command
{
    public SettingsCommand()
    {
        commandName = "settings";
        description = "設定に関するコマンドの一覧を表示します．";
    }

    public override List<string> AvailableValues(List<string> values)
    {
        if (values == null || values.Count == 0) { return new List<string>(); }

        else if (values.Count < 3)
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

        else if (values.Count == 1)
        {
            AddMessage(HelpText(false), Tracer.MessageLevel.normal, tracer, options);
            return;
        }

        else if (values.Count == 2)
        {
            var value = values[1];

            if (value == "description")
            {
                AddMessage(HelpText(true), Tracer.MessageLevel.normal, tracer, options);
            }

            else
            {
                AddMessage("詳細つきで一覧を表示するときは'settings description'，詳細を省いて一覧を表示するときは'settings'を実行してください．", Tracer.MessageLevel.error, tracer, options);
            }
            
        }

        else
        {
            AddMessage(ERROR_OverValues(2), Tracer.MessageLevel.error, tracer, options);
        }
    }

    static string HelpText(bool showDescription)
    {
        var text = "セッティング一覧：\n";

        foreach(var command in CommandReceiver.CommandList.Values)
        {
            if (command.commandType == CommandType.normal) { continue; }

            if (showDescription)
            {
                text += "【" + Info(command) + "】\n";
                text += command.description + "\n\n";
            }

            else
            {
                if (command.DefaultValue() != command.CurrentValue())
                {
                    text += "<color=lime>" + Info(command) + "</color>\n";
                }

                else
                {
                    text += Info(command) + "\n";
                }
            }
        }

        return text;

        // - inner function
        static string Info(Command command)
        {
            return command.commandName + "\n\t現在の値：" + command.CurrentValue() + "\t, デフォルト値：" + command.DefaultValue();
        }
    }
}
