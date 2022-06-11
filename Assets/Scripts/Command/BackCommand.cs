using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackCommand : Command
{
    public BackCommand()
    {
        commandName = "back";
        description = "最後に到達したチェックポイントまで戻ります";
    }

    public override void CommandMethod(Tracer tracer, List<string> values, List<string> options)
    {
        if (values == null || values.Count == 0) { return; }

        else if (values.Count == 1)
        {
            MapsManager.CurrentMap.Back();
            AddMessage("check point : " + MapsManager.CurrentMap.Index.ToString(), Tracer.MessageLevel.normal, tracer, options);
        }

        else
        {
            AddMessage(ERROR_OverValues(0), Tracer.MessageLevel.error, tracer, options);
        }
    }
}
