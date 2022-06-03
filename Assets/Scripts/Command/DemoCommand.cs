using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoCommand : Command
{
    public DemoCommand()
    {
        commandName = "demo";
        description = "デモンストレーションを行う機能を提供する．";
    }

    public override bool CheckValues(Tracer tracer, List<string> values)
    {
        if (values == null || values.Count < 2) { return false; }

        return true;
    }

    public override void CommandMethod(Tracer tracer, List<string> values)
    {
        if (values.Count < 2) { return; }
        var filename = values[1];

        var dataList = GetDataList(filename);

        if (dataList == null || dataList.Count == 0) 
        {
            tracer.AddMessage("データリストの読み込みに失敗しました", Tracer.MessageLevel.error); 
            return;
        }

        DemoManager.BeginDemo(dataList);

        // - inner function
        static List<float[]> GetDataList(string filename)
        {
            foreach (var demoData in DemoManager.DemoDataList)
            {
                if (demoData.filename == filename)
                {
                    return demoData.DataList();
                }
            }

            return new List<float[]>();
        }
    }
}
