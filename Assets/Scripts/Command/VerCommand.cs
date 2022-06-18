using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class VerCommand : Command
{
    static public readonly string currentVersion = "1.1";

    static readonly List<string> availableOnly = new List<string>()
    {
        "history"
    };

    public VerCommand()
    {
        commandName = "ver";
        description = "バージョンを確認するために使用できます";
        detail = "単に'ver'を実行すると，現在のバージョンを表示します．\n" +
            "'ver history' を実行すると，いままでのアップデートの内容を確認できます．";
    }

    public override List<string> AvailableValues(List<string> values)
    {
        if (values == null || values.Count == 0)
        {
            return new List<string>();
        }

        else if (values.Count  < 3)
        {
            return availableOnly;
        }

        return new List<string>();
    }

    public override void CommandMethod(Tracer tracer, List<string> values, List<string> options)
    {
        if (values == null || values.Count == 0) { return; }

        if (values.Count == 1)
        {
            AddMessage(currentVersion, Tracer.MessageLevel.normal, tracer, options);
        }

        else if (values.Count == 2)
        {
            var v = values[1];

            if (v == "history")
            {
                AddMessage(ShowHistory(), Tracer.MessageLevel.normal, tracer, options, 0);
            }

            else
            {
                AddMessage(ERROR_AvailableOnly(1, availableOnly), Tracer.MessageLevel.error, tracer, options);
            }
        }

        else
        {
            AddMessage(ERROR_OverValues(1), Tracer.MessageLevel.error, tracer, options);
        }
    }

    static string ShowHistory()
    {
        var message = "";

        foreach(var h in history)
        {
            var v = Regex.Replace(h.Key.ToString(), "_", ".");
            message += "【" + v + "】\n";
            
            foreach(var m in h.Value)
            {
                message += "・" + m + "\n";
            }

            message += "\n";
        }

        return message.TrimEnd(new char[] { '\n' });
    }

    enum Version
    {
        v1_0,
        v1_1,
    }

    static Dictionary<Version, List< string>> history = new Dictionary<Version, List<string>>()
    {
        { Version.v1_0, new List<string>() { "ez_bhopを公開しました．" } },

        { Version.v1_1, new List<string>() 
            { 
                "verコマンドを追加しました．", 
                "invokeコマンドでコードを追加等するときに無限ループを回避するためのチェックを強化しました．",
                "invokeコマンドのメッセージを修正しました．" ,
                "デモデータを追加しました．",
                "コンソールのデフォルトのメッセージを一部修正しました．",
            }
        }
    };
}


