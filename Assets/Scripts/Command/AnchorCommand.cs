using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorCommand : Command
{
    static AnchoredParams anchoredParams;

    enum Values
    {
        set,
        back,
    }

    public AnchorCommand()
    {
        commandName = "anchor";
        description = "プレイヤーの座標を記録し，その場所に戻る機能を提供します．\n" +
            "'anchor " + Values.set.ToString() + "'で座標を記録し，'anchor " + Values.back.ToString() + "'でその座標まで戻ります．\n" +
            "なお，マップが変更されると座標のデータは削除されます．";
    }

    public override void OnMapInitialized()
    {
        anchoredParams = null;
    }

    public override List<string> AvailableValues(List<string> values)
    {
        if (values == null || values.Count == 0) { return new List<string>(); }

        if (values.Count < 3)
        {
            return new List<string>() { Values.set.ToString(), Values.back.ToString() };
        }

        return new List<string>();
    }

    public override void CommandMethod(Tracer tracer, List<string> values)
    {
        if (values == null || values.Count == 0) { return; }

        if (values.Count == 1)
        {
            tracer.AddMessage("現在の座標 ... " + PositionInfo(PM_Main.Myself.transform.position), Tracer.MessageLevel.error);

            if (anchoredParams == null)
            {
                tracer.AddMessage("座標の記録はありません．", Tracer.MessageLevel.normal);
                return;
            }

            else
            {
                tracer.AddMessage("記録された座標 ... " + PositionInfo(anchoredParams.position), Tracer.MessageLevel.normal);
                return;
            }
        }

        else if (values.Count == 2)
        {
            var action = values[1];

            if (action == "set")
            {
                anchoredParams = new AnchoredParams();
                ChatMessages.SendChat("座標を記録しました．", ChatMessages.Sender.system);
                return;
            }

            else if (action == "back")
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

            else
            {
                tracer.AddMessage("'set'もしくは'back'のみ値として指定可能です．", Tracer.MessageLevel.error);
                return;
            }

            return;
        }

        else
        {
            tracer.AddMessage("2個以上の値を指定することはできません．", Tracer.MessageLevel.error);
        }
        
        // - inner function
        static string PositionInfo(Vector3 pos)
        {
            return "X : " + pos.x.ToString("f2") + ", Y : " + pos.y.ToString("2") + ", Z : " + pos.z.ToString("f2");
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
