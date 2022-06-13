using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultCommand : Command
{
    public DefaultCommand()
    {
        commandName = "default";
        description = "値に関する設定をデフォルト値に戻します．";
    }

    public override void CommandMethod(Tracer tracer, List<string> values, List<string> options)
    {
        if (values == null || values.Count == 0) { return; }

        else if (values.Count == 1)
        {
            foreach(var setting in Floats.Settings)
            {
                setting.Value.SetDefault();
            }

            AddMessage("全ての値をデフォルト値にもどしました．", Tracer.MessageLevel.normal, tracer, options);
        }

        else
        {
            AddMessage(ERROR_OverValues(0), Tracer.MessageLevel.error, tracer, options);
        }
    }
}
