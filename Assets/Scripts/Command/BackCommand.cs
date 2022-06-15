using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackCommand : Command
{
    public BackCommand()
    {
        commandName = "back";
        description = "最後に到達したチェックポイントまで戻ります．値を指定すると，その値に対応するチェックポイントまで戻ります．";
        detail = "単に'back'として実行すると，最後に到達したチェックポイントまで戻ります．\n" +
            "'back 1'など，値を指定して実行するとその値に対応するチェックポイントまで戻ります．チェックポイントの番号は，" +
            "スタート位置となるチェックポイントが0，その次が1...といったように，0から始まるので注意してください．\n" +
            "主にon_course_outが発生したときに自動実行させるか，手動で特定のチェックポイントに移動したいときに使用します．";
    }

    public override List<string> AvailableValues(List<string> values)
    {
        if (values == null || values.Count == 0) { return new List<string>(); }

        else if (values.Count < 3)
        {
            var list = new List<string>();

            for(var n = 0; n < MapsManager.CurrentMap.respawnPositions.Length; n++)
            {
                list.Add(n.ToString());
            }

            return list;
        }

        else
        {
            return new List<string>();
        }
    }

    public override void CommandMethod(Tracer tracer, List<string> values, List<string> options)
    {
        if (values == null || values.Count == 0) { return; }

        else if (values.Count == 1)
        {
            MapsManager.CurrentMap.Back();
            AddMessage("check point : " + MapsManager.CurrentMap.Index.ToString(), Tracer.MessageLevel.normal, tracer, options);
        }

        // ex) back(0) 0(1)
        else if (values.Count == 2)
        {
            var indexString = values[1];

            if (int.TryParse(indexString, out var index))
            {
                MapsManager.CurrentMap.Back(index);
                AddMessage("check point : " + MapsManager.CurrentMap.Index.ToString(), Tracer.MessageLevel.normal, tracer, options);
            }

            else
            {
                AddMessage(ERROR_NotInteger(indexString), Tracer.MessageLevel.error, tracer, options);
            }
        }

        else
        {
            AddMessage(ERROR_OverValues(0), Tracer.MessageLevel.error, tracer, options);
        }
    }
}
