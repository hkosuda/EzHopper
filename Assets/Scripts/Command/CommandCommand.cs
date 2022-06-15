using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandCommand : Command
{
    public CommandCommand()
    {
        commandName = "command";
        description = "�R�}���h�̈ꗗ��\�����܂��D";
        detail = "�R�}���h�̈ꗗ��\������ɂ�'command'�����s���܂��D�ȒP�Ȑ����t���ňꗗ��\������Ƃ��́C'command description'�����s���Ă��������D" +
            "�ڍׂ��ŃR�}���h�̈ꗗ��\������ɂ�'command detail'�����s���܂��D\n" +
            "�������R���\�[���ł͓ǂ݂ɂ����ł��傤����C�ڍׂ܂Ō������ꍇ�̓��j���[����R�}���h�ꗗ���Q�Ƃ��邱�Ƃ������߂��܂��D";
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
                AddMessage("�ȒP�Ȑ������ňꗗ��\������Ƃ���'command description'�C" +
                    "�ڍוt���ňꗗ��\������Ƃ���'command detail'�����s���Ă��������D", Tracer.MessageLevel.error, tracer, options);
            }
        }

        else
        {
            AddMessage(ERROR_OverValues(1), Tracer.MessageLevel.error, tracer, options);
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

            info += "�y" + command.Key + "�z\n";
            info += "�E�T�v\n";
            info += Paragraph(command.Value.description) + "\n";
            info += "�E�ڍ�\n";
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
