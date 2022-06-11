using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandCommand : Command
{
    public CommandCommand()
    {
        commandName = "command";
        description = "�R�}���h�̈ꗗ��\�����܂��D�ڍוt���ňꗗ��\������Ƃ��́C'command description'�����s���Ă��������D";
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
                AddMessage("�ڍׂ��ňꗗ��\������Ƃ���'command description'�C�ڍׂ��Ȃ��Ĉꗗ��\������Ƃ���'command'�����s���Ă��������D", Tracer.MessageLevel.error, tracer, options);
                return;
            }
        }

        else
        {
            AddMessage("2�ȏ�̒l���w�肷�邱�Ƃ͂ł��܂���D", Tracer.MessageLevel.error, tracer, options);
        }
    }

    static string GetHelpText(bool showDescription)
    {
        var text = "�R�}���h�ꗗ�F\n";

        foreach (var command in CommandReceiver.CommandList)
        {
            if (command.Value.commandType == CommandType.values) { continue; }

            if (showDescription)
            {
                text += "�y" + command.Key + "�z\n";
                text += command.Value.description + "\n\n";
            }

            else
            {
                text += command.Key + "\n";
            }
        }

        if (!showDescription)
        {
            text += "\n" + "�ڍוt���ňꗗ��\������Ƃ��́C'command description'�����s���Ă��������D";
        }

        return text.TrimEnd(new char[1] { '\n' });
    }
}
