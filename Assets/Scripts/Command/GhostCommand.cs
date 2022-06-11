using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostCommand : Command
{
    public GhostCommand()
    {
        commandName = "ghost";
        description = "ゴーストを起動する機能を提供します．\n" +
            "単に'ghost'と入力すると，直前に記録したプレイヤーの動きを再現します．記録がなければ再現は行われません．\n" +
            "保存したデータを呼び出して実行するときは，'ghost start <name>としてください（<name>の部分にデータ名）．\n" +
            "ゴーストを終了するには，'ghost end'を実行します．";
    }

    public override List<string> AvailableValues(List<string> values)
    {
        if (values == null || values.Count == 0) { return new List<string>(); }

        if (values.Count < 3)
        {
            return new List<string>()
            {
                "start", "end"
            };
        }

        else if (values.Count < 4)
        {
            var value = values[1];

            if (value == "start")
            {
                var available = new List<string>();

                if (RecordCacheSystem.CachedDataList == null) { return new List<string>(); }

                foreach (var data in RecordCacheSystem.CachedDataList)
                {
                    available.Add(data.Key);
                }

                return available;
            }

            else
            {
                return new List<string>();
            }
        }

        else
        {
            return new List<string>();
        }
    }

    public override void CommandMethod(Tracer tracer, List<string> values, List<string> options)
    {
        if (values == null || values.Count == 0) { return; }

        if (values.Count == 1)
        {
            var dataParams = RecordCacheSystem.CachedData;

            if (dataParams == null) 
            {
                AddMessage("データが存在しないため，利用できません．", Tracer.MessageLevel.error, tracer, options);
            }

            else
            {
                BeginGhostFromParams(dataParams, tracer, options);
            }
        }

        else if (values.Count == 2)
        {
            var value = values[1];

            if (value == "start")
            {
                AddMessage("再生するデータの名前を指定してください．", Tracer.MessageLevel.error, tracer, options);
            }

            else if (value == "end")
            {
                Ghost.EndReplay();
                AddMessage("ゴーストを停止しました．", Tracer.MessageLevel.normal, tracer, options);
            }

            else
            {
                AddMessage("1番目の値としては，'start'もしくは'end'のみ設定可能です．", Tracer.MessageLevel.error, tracer, options);
            }
        }

        else if (values.Count == 3)
        {
            var value = values[1];
            var name = values[2];

            if (value == "start")
            {
                if (RecordCacheSystem.CachedDataList != null && RecordCacheSystem.CachedDataList.ContainsKey(name))
                {
                    var dataParams = RecordCacheSystem.CachedDataList[name];
                    BeginGhostFromParams(dataParams, tracer, options);
                }

                else
                {
                    AddMessage(name + "というデータは存在しません．", Tracer.MessageLevel.error, tracer, options);
                }
            }

            else
            {
                AddMessage("データ名を指定してゴーストを起動するには，'ghost start <name>'として実行してください．", Tracer.MessageLevel.error, tracer, options);
            }
        }

        else
        {
            AddMessage("4個以上の値を指定することはできません．", Tracer.MessageLevel.error, tracer, options);
        }
        

        // - inner function
        static void BeginGhostFromParams(RecordCacheSystem.DataListParams param, Tracer tracer, List<string> options)
        {
            if (PlayerRecorder.Recording)
            {
                AddMessage("レコーダーが起動しているため，ゴーストを利用できません．", Tracer.MessageLevel.error, tracer, options);
            }

            if (param.mapName != MapsManager.CurrentMap.MapName)
            {
                AddMessage("現在のマップと異なるマップのデータであるため，マップを切り替えて実行します．", Tracer.MessageLevel.warning, tracer, options);
                MapsManager.Begin(param.mapName);
            }

            Ghost.BeginReplay(param.dataList);
            AddMessage("ゴーストを起動しました．", Tracer.MessageLevel.normal, tracer, options);
        }
    }
}
