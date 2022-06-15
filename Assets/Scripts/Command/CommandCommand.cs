using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandCommand : Command
{
    public CommandCommand()
    {
        commandName = "command";
        description = "コマンドの一覧を表示します．";
        detail = "コマンドの一覧を表示するには'command'を実行します．簡単な説明付きで一覧を表示するときは，'command description'を実行してください．" +
            "詳細つきでコマンドの一覧を表示するには'command detail'を実行します．\n" +
            "ただしコンソールでは読みにくいでしょうから，詳細まで見たい場合はメニューからコマンド一覧を参照することをお勧めします．";
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
            AddMessage(GetHelpText(false), Tracer.MessageLevel.normal, tracer, options, 0);
        }

        else if (values.Count == 2)
        {
            var value = values[1];

            if (value == "description")
            {
                AddMessage(GetHelpText(true), Tracer.MessageLevel.normal, tracer, options, 0);
            }

            else if (value == "detail")
            {
                AddMessage(CommandsWithDetail(), Tracer.MessageLevel.normal, tracer, options, 0);
            }

            else
            {
                AddMessage("簡単な説明つきで一覧を表示するときは'command description'，" +
                    "詳細付きで一覧を表示するときは'command detail'を実行してください．", Tracer.MessageLevel.error, tracer, options);
            }
        }

        else
        {
            AddMessage(ERROR_OverValues(1), Tracer.MessageLevel.error, tracer, options);
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
                text += Paragraph(command.Value.description) + "\n\n";
            }

            else
            {
                text += command.Key + "\n";
            }
        }

        return text.TrimEnd(new char[1] { '\n' });
    }

    static public string CommandsWithDetail()
    {
        var info = "";

        foreach (var command in CommandReceiver.CommandList)
        {
            if (command.Value.commandType == CommandType.values) { continue; }

            info += "【" + command.Key + "】\n";
            info += "・概要\n";
            info += Paragraph(command.Value.description) + "\n";
            info += "・詳細\n";
            info += Paragraph(command.Value.detail) + "\n\n";
        }

        return info.TrimEnd(new char[] { '\n' });
    }

    static public string Paragraph(string content)
    {
        var splitted = content.Split(new char[] { '\n' });
        if (splitted == null || splitted.Length == 0) { return ""; }

        var paragraph = "";

        foreach (var s in splitted)
        {
            paragraph += " " + s + "\n";
        }

        return paragraph.TrimEnd(new char[] { '\n' });
    }
}
