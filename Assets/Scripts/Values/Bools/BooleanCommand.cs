using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BooleanCommand : Command
{
    Bools.Item item;

    public BooleanCommand(Bools.Item item)
    {
        this.item = item;

        commandName = item.ToString();
        commandType = CommandType.values;
    }

    public override void CommandMethod(Tracer tracer, List<string> values)
    {
        if (values == null || values.Count == 0) { return; }

        if (values.Count == 1)
        {
            tracer.AddMessage(HelpText(item), Tracer.MessageLevel.normal);
            return;
        }

        if (values.Count == 2)
        {
            ChangeValue(item, tracer, values);
        }

        else
        {
            tracer.AddMessage("�l��2�ȏ�w�肷�邱�Ƃ͂ł��܂���D", Tracer.MessageLevel.error);
        }
        
        // - inner function
        static void ChangeValue(Bools.Item item, Tracer tracer, List<string> values)
        {
            var setting = Bools.Settings[item];

            var value = values[1];
            
            if (int.TryParse(value, out var num))
            {
                if (num == 1)
                {
                    setting.SetValue(true);
                    return;
                }

                else if (num == 0)
                {
                    setting.SetValue(false);
                    return;
                }
            }

            if (value.ToLower() == "on")
            {
                setting.SetValue(true);
                return;
            }

            if (value.ToLower() == "off")
            {
                setting.SetValue(false);
                return;
            }

            tracer.AddMessage(value + "��L���Ȓl�ɕϊ��ł��܂���D'on'��������'off'��l�Ƃ��Ďw�肵�Ă��������D", Tracer.MessageLevel.error);
        }
    }

    static string HelpText(Bools.Item item)
    {
        var setting = Bools.Settings[item];

        var text = "";
        text += "���݂̒l : ";
        text += ToString(setting.CurrentValue);
        text += ", �f�t�H���g�l : ";
        text += ToString(setting.DefaultValue);
        text += "\n";
        text += "\t�ڍ� : ";
        text += setting.Description;

        return text;
    }

    static string ToString(bool value)
    {
        if (value)
        {
            return "on";
        }

        return "off";
    }
}
