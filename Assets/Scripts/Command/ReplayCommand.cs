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

    public override void CommandMethod(Tracer tracer, List<string> values, List<string> options)
    {
        if (values == null || values.Count == 0) { return; }

        else if (values.Count == 1)
        {
            var dataParams = RecordCacheSystem.CachedData;

            if (dataParams == null)
            {
                AddMessage("データが存在しないため，デモを再生できません．", Tracer.MessageLevel.error, tracer, options); 
            }

            else
            {
                BeginDemoFromParams(dataParams, tracer, options);
            }
        }

        else if (values.Count == 2)
        {
            var filename = values[1];

            if (RecordCacheSystem.CachedDataList != null && RecordCacheSystem.CachedDataList.ContainsKey(filename))
            {
                var dataParams = RecordCacheSystem.CachedDataList[filename];

                BeginDemoFromParams(dataParams, tracer, options);
            }

            else
            {
                AddMessage(filename + "というデータは存在しません．", Tracer.MessageLevel.error, tracer, options);
            }
        }

        else
        {
            AddMessage(ERROR_OverValues(2), Tracer.MessageLevel.error, tracer, options);
        }

        // - inner function
        static void BeginDemoFromParams(RecordCacheSystem.DataListParams param, Tracer tracer, List<string> options)
        {
            if (param.mapName != MapsManager.CurrentMap.MapName)
            {
                AddMessage("現在のマップと異なるマップのデータであるため，マップを切り替えて実行します．", Tracer.MessageLevel.warning, tracer, options);
                MapsManager.Begin(param.mapName);
            }

            DemoManager.BeginDemo(param.dataList);
        }
    }
}
