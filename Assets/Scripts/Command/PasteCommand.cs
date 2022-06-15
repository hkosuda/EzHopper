using System;   
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class PasteCommand : Command
{
    public PasteCommand()
    {
        commandName = "paste";
        description = "クリップボードにコピーされたコマンドを実行します．";
        detail = "具体的な使用方法としては，'copy' を使用して得られたコードあるいは自ら作成したコードをメモ帳などに記載しておき，" +
            "それを範囲選択->コピーします．これにより，コードがクリップボードにコピーされた状態となります．" +
            "この状態で 'paste' を実行することで，クリップボードの内容が読み込まれ，コードが自動的に実行されます．\n" +
            "コードは複数行にわたって一括で実行できます．また，同一行の '//' 以降の文字列はコメントとして無視されます．";
    }

    public override void CommandMethod(Tracer tracer, List<string> values, List<string> options)
    {
        if (values == null || values.Count == 0) { return; }

        if (values.Count == 1)
        {
            var content = GUIUtility.systemCopyBuffer;
            ConsoleMessage.WriteLog("<color=lime>クリップボードの内容を読み込み，コマンドを自動で実行します．</color>");
            Run(content, tracer, options);
            AddMessage("自動実行が終了しました．", Tracer.MessageLevel.emphasis, tracer, options);
        }

        else
        {
            AddMessage(ERROR_OverValues(0), Tracer.MessageLevel.error, tracer, options);
        }
    }

    static void Run(string content, Tracer tracer, List<string> options)
    {
        var splitted = new List<string>(content.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries));

        if (splitted == null || splitted.Count == 0)
        {
            AddMessage("クリップボードから文字列を取得できませんでした．", Tracer.MessageLevel.error, tracer, options);
            return;
        }

        var normalCommandNameList = CommandNameList(CommandType.normal);
        var valuesCommandNameList = CommandNameList(CommandType.values);

        var normalCommandList = new List<string>();
        var boolsCommandList = new List<string>();
        var floatsCommandList = new List<string>();

        var existenceFlag = false;

        for (var n = 0; n < splitted.Count; n++)
        {
            var line = splitted[n].ToLower().Trim();
            line = Regex.Replace(line, @"\/\/.*", "");

            var values = CommandReceiver.GetValues(line);
            if (values == null || values.Count == 0) { continue; }

            var commandName = values[0];
            if (commandName == "paste")
            {
                AddMessage("'paste'を自動実行することはできません．", Tracer.MessageLevel.error, tracer, options);
                return;
            }

            if (normalCommandNameList.Contains(commandName))
            {
                normalCommandList.Add(line);
                existenceFlag = true;
            }

            else if (valuesCommandNameList.Contains(commandName))
            {
                foreach(var setting in Bools.Settings)
                {
                    if (commandName == setting.Key.ToString().ToLower())
                    {
                        boolsCommandList.Add(line);
                        existenceFlag = true;
                        break;
                    }
                }

                foreach(var setting in Floats.Settings)
                {
                    if (commandName == setting.Key.ToString().ToLower())
                    {
                        floatsCommandList.Add(line);
                        existenceFlag = true;
                        break;
                    }
                }
            }

            else
            {
                AddMessage("(" + n.ToString() + ") " + commandName + "というコマンドは存在しません．", Tracer.MessageLevel.error, tracer, options);
            }
        }

        if (!existenceFlag)
        {
            AddMessage("実行するコマンドはありません．", Tracer.MessageLevel.warning, tracer, options);
            return;
        }

        foreach(var normalCommand in normalCommandList)
        {
            CommandReceiver.RequestCommand(normalCommand);
        }

        foreach(var boolsCommand in boolsCommandList)
        {
            CommandReceiver.RequestCommand(boolsCommand);
        }

        foreach(var floatCommand in floatsCommandList)
        {
            CommandReceiver.RequestCommand(floatCommand);
        }
    }

    static List<string> CommandNameList(CommandType commandType)
    {
        if (CommandReceiver.CommandList == null) { return new List<string>(); }

        var commandNameList = new List<string>();

        foreach(var command in CommandReceiver.CommandList)
        {
            if (command.Value.commandType == commandType)
            {
                commandNameList.Add(command.Key);
            }
        }

        return commandNameList;
    }
}
