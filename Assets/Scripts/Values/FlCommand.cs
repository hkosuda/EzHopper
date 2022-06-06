using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlCommand : Command
{
    Floats.Item item;

    public FlCommand(Floats.Item item)
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
            var value = values[1];

            if (float.TryParse(value, out var num))
            {
                var setting = Floats.Settings[item];

                if (setting.ValidationCheck(num, tracer))
                {
                    setting.SetValue(num);
                    return;
                }

                else
                {
                    tracer.AddMessage("不適切な値：", Tracer.MessageLevel.error);
                    return;
                }
            }

            else
            {
                tracer.AddMessage(value + "を数値に変換できません．", Tracer.MessageLevel.error);
                return;
            }
        }

        else
        {
            tracer.AddMessage("値を2個以上指定することはできません．", Tracer.MessageLevel.error);
        }
    }

    static string HelpText(Floats.Item item)
    {
        var setting = Floats.Settings[item];

        var text = "";
        text += "現在の値 : ";
        text += setting.CurrentValue.ToString();
        text += ", デフォルト値 : ";
        text += setting.DefaultValue.ToString();
        text += "\n";
        text += "\t詳細 : ";
        text += setting.Description;

        if (setting.Validations != null)
        {
            text += "\n";
            text += "\t制約 : \n";

            foreach(var validation in setting.Validations)
            {
                text += "\t\t" + validation.GetDiscription() + "\n";
            }
        }

        return text;
    }
}
