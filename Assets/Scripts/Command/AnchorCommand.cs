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

    public override void CommandMethod(Tracer tracer, List<string> values, List<string> options)
    {
        if (values == null || values.Count == 0) { return; }

        if (values.Count == 1)
        {
            AddMessage("現在の座標 ... " + PositionInfo(PM_Main.Myself.transform.position), Tracer.MessageLevel.normal, tracer, options);

            if (anchoredParams == null)
            {
                AddMessage("座標の記録はありません．", Tracer.MessageLevel.normal, tracer, options);
            }

            else
            {
                AddMessage("記録された座標 ... " + PositionInfo(anchoredParams.position), Tracer.MessageLevel.normal, tracer, options);
            }
        }

        else if (values.Count == 2)
        {
            var action = values[1];

            if (action == "set")
            {
                anchoredParams = new AnchoredParams();
                AddMessage("座標を記録しました．", Tracer.MessageLevel.normal, tracer, options);
            }

            else if (action == "back")
            {
                if (anchoredParams == null)
                {
                    AddMessage("座標が記録されていません．'anchor set'を使用して座標を記録してください", Tracer.MessageLevel.error, tracer, options);
                }

                else if (anchoredParams.mapName != MapsManager.CurrentMap.MapName)
                {
                    AddMessage("記録された際のマップと現在のマップが異なるため，記録された座標に戻れません．", Tracer.MessageLevel.error, tracer, options);
                }

                else
                {
                    PM_Main.ResetPosition(anchoredParams.position, anchoredParams.eulerAngle.y);
                    AddMessage("記録された座標にプレイヤーを移動させました．", Tracer.MessageLevel.normal, tracer, options);
                }
            }

            else
            {
                AddMessage("'set'もしくは'back'のみ値として指定可能です．", Tracer.MessageLevel.error, tracer, options);
            }
        }

        else
        {
            AddMessage(ERROR_OverValues(2), Tracer.MessageLevel.error, tracer, options);
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
