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

        var setting = Floats.Settings[item];

        description = setting.Description;
        description += "\n現在の値：" + CurrentValue() + ", デフォルト値：" + DefaultValue();
    }

    public override void CommandMethod(Tracer tracer, List<string> values, List<string> options)
    {
        if (values == null || values.Count == 0) { return; }

        if (values.Count == 1)
        {
            AddMessage(HelpText(item), Tracer.MessageLevel.normal, tracer, options);
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
                    AddMessage("不適切な値：", Tracer.MessageLevel.error, tracer, options);
                    return;
                }
            }

            else
            {
                AddMessage(value + "を数値に変換できません．", Tracer.MessageLevel.error, tracer, options);
                return;
            }
        }

        else
        {
            AddMessage("値を2個以上指定することはできません．", Tracer.MessageLevel.error, tracer, options);
        }
    }

    public override string CurrentValue()
    {
        return Floats.Settings[item].CurrentValue.ToString();
    }

    public override string DefaultValue()
    {
        return Floats.Settings[item].DefaultValue.ToString();
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
