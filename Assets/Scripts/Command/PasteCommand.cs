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
        description = "�N���b�v�{�[�h�ɃR�s�[���ꂽ�R�}���h�����s���܂��D�R�}���h�͕����s�ɂ킽���Ĉꊇ�Ŏ��s�ł��܂��D\n" +
            "�Ȃ��C'//'�ȍ~�̕�����̓R�����g�Ƃ��Ė�������܂��D";
    }

    public override void CommandMethod(Tracer tracer, List<string> values, List<string> options)
    {
        if (values == null || values.Count == 0) { return; }

        if (values.Count == 1)
        {
            var content = GUIUtility.systemCopyBuffer;
            ConsoleMessage.WriteLog("<color=lime>�N���b�v�{�[�h�̓��e��ǂݍ��݁C�R�}���h�������Ŏ��s���܂��D</color>");
            Run(content, tracer, options);
            AddMessage("�������s���I�����܂����D", Tracer.MessageLevel.emphasis, tracer, options);
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
            AddMessage("�N���b�v�{�[�h���當������擾�ł��܂���ł����D", Tracer.MessageLevel.error, tracer, options);
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
                AddMessage("(" + n.ToString() + ") " + commandName + "�Ƃ����R�}���h�͑��݂��܂���D", Tracer.MessageLevel.error, tracer, options);
            }
        }

        if (!existenceFlag)
        {
            AddMessage("���s����R�}���h�͂���܂���D", Tracer.MessageLevel.warning, tracer, options);
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
