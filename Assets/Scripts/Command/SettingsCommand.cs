using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsCommand : Command
{
    public SettingsCommand()
    {
        commandName = "settings";
        description = "�ݒ�Ɋւ���R�}���h�̈ꗗ��\�����܂��D";
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
                AddMessage("�ڍׂ��ňꗗ��\������Ƃ���'settings description'�C�ڍׂ��Ȃ��Ĉꗗ��\������Ƃ���'settings'�����s���Ă��������D", Tracer.MessageLevel.error, tracer, options);
            }
            
        }

        else
        {
            AddMessage(ERROR_OverValues(2), Tracer.MessageLevel.error, tracer, options);
        }
    }

    static string HelpText(bool showDescription)
    {
        var text = "�Z�b�e�B���O�ꗗ�F\n";

        foreach(var command in CommandReceiver.CommandList.Values)
        {
            if (command.commandType == CommandType.normal) { continue; }

            if (showDescription)
            {
                text += "�y" + Info(command) + "�z\n";
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
            return command.commandName + "\n\t���݂̒l�F" + command.CurrentValue() + "\t, �f�t�H���g�l�F" + command.DefaultValue();
        }
    }
}
