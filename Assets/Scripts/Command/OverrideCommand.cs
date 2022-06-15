using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverrideCommand : Command
{
    static readonly List<string> available = new List<string>()
    {
        "echo", "flash", "mute", "silent", "none"
    };

    enum OverrideOption
    {
        echo,
        flash,
        mute,
        silent,
        none,
    }

    static OverrideOption overrideOption = OverrideOption.none;

    public OverrideCommand()
    {
        commandName = "override";
        description = "システムが内部的に実行するコードを含め，実行されるコードのオプションをすべて書き換える機能を提供します．";
        detail = "メッセージを一括で '-mute' にする場合や，'bind' などで自動実行に指定しているコードが正しく機能しない際に，確認するための機能として使用することができます．" +
            "たとえば，'override flash' を実行することでオプションに '-mute' を指定しているコードも '-flash' で実行されるため，" +
            "正しく機能していないコマンドのメッセージを見てその原因を調べることができるようになります．\n" +
            "利用できるオーバーライドは以下の通りです．\n" +
            "echo  : すべてのコードを '-echo' つきで実行します．オプションに '-echo' を付けることで，コンソールとチャットの両方にメッセージが表示されます．\n" +
            "flash : すべてのコードを '-flash' つきで実行します．オプションに '-flash' を付けることで，チャットにのみメッセージが表示されます．\n" +
            "mute  : すべてのコードを '-mute' つきで実行します．オプションに '-mute' を付けることで，コンソール，チャットのどちらにもメッセージが表示されません．\n" +
            "silent: すべてのコードのメッセージをコンソールにのみ表示します．\n" +
            "none  : オーバーライドの利用を停止します．";
    }

    public override List<string> AvailableValues(List<string> values)
    {
        if (values == null || values.Count == 0) { return new List<string>(); }

        else if (values.Count < 3)
        {
            return new List<string>() { "echo", "flash", "mute", "silent", "none" };
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
            AddMessage("オプションを指定してください．", Tracer.MessageLevel.error, tracer, options);
        }

        else if (values.Count == 2)
        {
            var value = values[1];

            if (value == "echo")
            {
                overrideOption = OverrideOption.echo;
                _AddMessage(OverrideOption.echo, tracer, options);
            }

            else if (value == "flash")
            {
                overrideOption = OverrideOption.flash;
                _AddMessage(OverrideOption.flash, tracer, options);
            }

            else if (value == "mute")
            {
                overrideOption = OverrideOption.mute;
                _AddMessage(OverrideOption.mute, tracer, options);
            }

            else if (value == "silent")
            {
                overrideOption = OverrideOption.silent;
                _AddMessage(OverrideOption.silent, tracer, options);
            }

            else if (value == "none")
            {
                overrideOption = OverrideOption.none;
                _AddMessage(OverrideOption.none, tracer, options);
            }

            else
            {
                AddMessage(ERROR_AvailableOnly(1, available), Tracer.MessageLevel.error, tracer, options);
            }
        }

        else
        {
            AddMessage(ERROR_OverValues(1), Tracer.MessageLevel.error, tracer, options);
        }

        // - inner function
        static void _AddMessage(OverrideOption option, Tracer tracer, List<string> options)
        {
            if (option == OverrideOption.none)
            {
                AddMessage("オーバーライドを解除しました．", Tracer.MessageLevel.normal, tracer, options);
            }

            else
            {
                AddMessage("オーバーライドを書き換えました．コマンドはすべて" + option.ToString() + "で実行されます．", Tracer.MessageLevel.normal, tracer, options);
            }
        }
    }

    static public string OverrideSentence(string sentence)
    {
        if (overrideOption == OverrideOption.none)
        {
            return sentence;
        }

        if (overrideOption == OverrideOption.echo)
        {
            return sentence + " -e";
        }

        if (overrideOption == OverrideOption.flash)
        {
            return sentence + " -f";
        }

        if (overrideOption == OverrideOption.mute)
        {
            return sentence + " -m";
        }

        // silent
        else
        {
            var values = CommandReceiver.GetValues(sentence);
            var overrided = "";

            if (values == null) { return overrided; }

            foreach(var value in values)
            {
                overrided += value + " ";
            }

            return overrided;
        }
    }
}
