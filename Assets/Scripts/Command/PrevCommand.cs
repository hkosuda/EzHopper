using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrevCommand : Command
{
    public PrevCommand()
    {
        commandName = "prev";
        description = "中間地点が複数設定されているマップで，ひとつ前の中間地点に移動する機能を提供します．";
    }

    public override void CommandMethod(Tracer tracer, List<string> values, List<string> options)
    {
        var prev = MapsManager.CurrentMap.Index;
        MapsManager.CurrentMap.Prev();
        var current = MapsManager.CurrentMap.Index;

        if (MapsManager.CurrentMap.respawnPositions.Length == 1)
        {
            AddMessage("現在のマップにはチェックポイントが1つしかありません．", Tracer.MessageLevel.warning, tracer, options);
        }

        else
        {
            AddMessage("check point : " + prev.ToString() + " -> " + current.ToString(), Tracer.MessageLevel.normal, tracer, options);
        }
    }
}
