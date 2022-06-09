using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayCommand : Command
{
    public ReplayCommand()
    {
        commandName = "replay";
    }

    public override List<string> AvailableValues(List<string> values)
    {
        if (values == null || values.Count == 0) { return new List<string>(); }

        if (values.Count < 3)
        {
            var available = new List<string>();

            if (RecordCacheSystem.CachedDataList == null) { return new List<string>(); }

            foreach(var data in RecordCacheSystem.CachedDataList)
            {
                available.Add(data.Key);
            }

            return available;
        }

        return new List<string>();
    }

    public override void CommandMethod(Tracer tracer, List<string> values)
    {
        if (values == null || values.Count == 0) { return; }

        if (values.Count == 1)
        {
            var dataParams = RecordCacheSystem.CachedData;

            if (dataParams == null)
            {
                tracer.AddMessage("データが存在しないため，デモを再生できません．", Tracer.MessageLevel.error); 
                return;
            }

            else
            {
                BeginDemoFromParams(dataParams, tracer);
                return;
            }
        }

        if (values.Count == 2)
        {
            var filename = values[1];

            if (RecordCacheSystem.CachedDataList != null && RecordCacheSystem.CachedDataList.ContainsKey(filename))
            {
                var dataParams = RecordCacheSystem.CachedDataList[filename];

                BeginDemoFromParams(dataParams, tracer);
                return;
            }

            else
            {
                tracer.AddMessage(filename + "というデータは存在しません．", Tracer.MessageLevel.error);
                return;
            }
        }

        tracer.AddMessage("3個以上の値を指定することはできません．", Tracer.MessageLevel.error);

        // - inner function
        static void BeginDemoFromParams(RecordCacheSystem.DataListParams param, Tracer tracer)
        {
            if (param.mapName != MapsManager.CurrentMap.MapName)
            {
                tracer.AddMessage("現在のマップと異なるマップのデータであるため，マップを切り替えて実行します．", Tracer.MessageLevel.warning);
                MapsManager.Begin(param.mapName);
            }

            DemoManager.BeginDemo(param.dataList);
        }
    }
}
