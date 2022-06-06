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

        var asset = Resources.Load<TextAsset>("DemoData/" + filename + ".ghost");
        if (asset == null) { tracer.AddMessage("データが見つかりませんでした．", Tracer.MessageLevel.error); return; }

        var dataList = DemoFileUtils.FullText2DataList(asset.text);

        if (dataList == null || dataList.Count == 0) 
        {
            tracer.AddMessage("データリストの読み込みに失敗しました", Tracer.MessageLevel.error); 
            return;
        }

        DemoManager.BeginDemo(dataList);
    }
}
