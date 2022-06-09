using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoCommand : Command
{
    static public readonly List<string> availableValues = new List<string>()
    {
        "athletic_piles",
        "athletic_tiles",
        "athletic_slopes",
        "flyer",
        "horizon_0101",
        "nostalgia_0101",
        "nostalgia_0101_02",
        "nostalgia_0102",
        "nostalgia_0103",
        "nostalgia_0201",
        "nostalgia_0202",
        "nostalgia_0203",
        "nostalgia_0203_shortcut",
        "nostalgia_0301",
        "nostalgia_0302",
        "square_0101_01",
        "square_0102_01",
        "square_0103_01",
        "square_0103_02",
        "square_0103_03",
        "square_0103_04",
        "training",

    };

    public DemoCommand()
    {
        commandName = "demo";
        description = "デモを再生する機能を提供します．\n";

#if UNITY_EDITOR
        foreach(var value in availableValues)
        {
            var asset = Resources.Load<TextAsset>("DemoData/" + value + ".ghost");
            if (asset == null)
            {
                Debug.LogError("Not available : " + value);
            }
        }
#endif
    }

    public override List<string> AvailableValues(List<string> values)
    {
        if (values == null || values.Count == 0) { return new List<string>(); }

        if (values.Count < 3)
        {
            return availableValues;
        }

        return new List<string>();
    }

    public override void CommandMethod(Tracer tracer, List<string> values)
    {
        if (values == null || values.Count == 0) { return; }

        if (values.Count == 1)
        {
            tracer.AddMessage("再生するデータを指定してください．", Tracer.MessageLevel.error);
            return;
        }

        if (values.Count == 2)
        {
            var filename = values[1];

            var asset = Resources.Load<TextAsset>("DemoData/" + filename + ".ghost");

            if (asset != null)
            {
                var mapName = DemoFileUtils.FullText2MapName(asset.text);
                if (mapName == MapName.none) { tracer.AddMessage("データに指定されたマップが見つかりません．", Tracer.MessageLevel.error); return; }

                var dataList = DemoFileUtils.FullText2DataList(asset.text);
                if (dataList == null || dataList.Count == 0)
                {
                    tracer.AddMessage("データリストの読み込みに失敗しました", Tracer.MessageLevel.error);
                    return;
                }

                if (!MapsManager.MapList.ContainsKey(mapName))
                {
                    tracer.AddMessage("指定されたマップは読み込めません．", Tracer.MessageLevel.error);
                    return;
                }

                if (mapName != MapsManager.CurrentMap.MapName)
                {
                    tracer.AddMessage("現在のマップと異なるマップのデモデータであるため，マップを切り替えて実行します．", Tracer.MessageLevel.warning);
                    MapsManager.Begin(mapName);
                }

                DemoManager.BeginDemo(dataList);
                tracer.AddMessage("デモを起動しました．", Tracer.MessageLevel.normal);
                return;
            }

            else
            {
                tracer.AddMessage(filename + "に該当するデータが見つかりませんでした．", Tracer.MessageLevel.error);
            }
        }

        else
        {
            tracer.AddMessage("2個以上の値を指定することはできません．", Tracer.MessageLevel.error);
            return;
        }
    }
}
