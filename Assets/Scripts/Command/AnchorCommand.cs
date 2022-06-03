using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorCommand : Command
{
    static AnchoredParams anchoredParams;

    public AnchorCommand()
    {
        commandName = "anchor";
    }

    public override void CommandMethod(Tracer tracer, List<string> values)
    {
        if (values == null || values.Count < 2) { return; }
        var action = values[1];

        if (action == "set")
        {
            anchoredParams = new AnchoredParams();
            ChatMessages.SendChat("座標を記録しました．", ChatMessages.Sender.system);
            return;
        }

        if (action == "back")
        {
            if (anchoredParams == null) 
            {
                tracer.AddMessage("座標が記録されていません．'anchor set'を使用して座標を記録してください", Tracer.MessageLevel.error);
                return;
            }

            if (anchoredParams.mapName != MapsManager.CurrentMap.MapName)
            {
                tracer.AddMessage("記録された際のマップと現在のマップが異なるため，記録された座標に戻れません．", Tracer.MessageLevel.error);
                return;
            }

            PM_Main.Myself.transform.position = anchoredParams.position;
            PM_Camera.SetEulerAngles(anchoredParams.eulerAngle);
            PM_Main.Rb.velocity = Vector3.zero;
            PM_Jumping.InactivateAutoJump();
        }
    }

    class AnchoredParams
    {
        public MapName mapName;
        public Vector3 position;
        public Vector3 eulerAngle;

        public AnchoredParams()
        {
            mapName = MapsManager.CurrentMap.MapName;
            position = PM_Main.Myself.transform.position;
            eulerAngle = PM_Camera.EulerAngles();
        }
    }
}
