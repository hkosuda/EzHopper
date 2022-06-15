using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultCommand : Command
{
    public DefaultCommand()
    {
        commandName = "default";
        description = "'pm_' で始まる設定をデフォルト値に戻します．";
        detail = "'pm_' で始まる設定は，プレイヤーの動きに関する設定を示します．";
    }

    public override void CommandMethod(Tracer tracer, List<string> values, List<string> options)
    {
        if (values == null || values.Count == 0) { return; }

        else if (values.Count == 1)
        {
            foreach(var setting in Floats.Settings)
            {
                if (!setting.Key.ToString().StartsWith("pm_")) { continue; }
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
