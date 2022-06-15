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
        detail = "デモとは，あらかじめ用意されたデータのことです．主にそのマップの攻略方法を確認する際に活用します．\n" +
            "'replay'と同様に，データのマップ情報が現在のマップと異なる場合，実行時にマップが切り替わってしまうので注意してください．\n" +
            "なお，デモの再生と同時にゴーストも起動します．";

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

    public override void CommandMethod(Tracer tracer, List<string> values, List<string> options)
    {
        if (values == null || values.Count == 0) { return; }

        if (values.Count == 1)
        {
            AddMessage("再生するデータを指定してください．", Tracer.MessageLevel.error, tracer, options);
        }

        else if (values.Count == 2)
        {
            var filename = values[1];
            var asset = Resources.Load<TextAsset>("DemoData/" + filename + ".ghost");

            if (asset != null)
            {
                var mapName = DemoFileUtils.FullText2MapName(asset.text);
                if (mapName == MapName.none){ AddMessage("データに指定されたマップが見つかりません．", Tracer.MessageLevel.error, tracer, options); return; }

                var dataList = DemoFileUtils.FullText2DataList(asset.text);

                if (dataList == null || dataList.Count == 0)
                {
                    AddMessage("データリストの読み込みに失敗しました", Tracer.MessageLevel.error, tracer, options);
                    return;
                }

                else if (!MapsManager.MapList.ContainsKey(mapName))
                {
                    AddMessage("指定されたマップは読み込めません．", Tracer.MessageLevel.error, tracer, options);
                    return;
                }

                else if (mapName != MapsManager.CurrentMap.MapName)
                {
                    MapsManager.Begin(mapName);
                    AddMessage("現在のマップと異なるマップのデモデータであるため，マップを切り替えて実行します．", Tracer.MessageLevel.warning, tracer, options);
                }

                Ghost.BeginReplay(new List<float[]>(dataList), mapName);
                DemoManager.BeginDemo(new List<float[]>(dataList));
                
                AddMessage("デモを起動しました．", Tracer.MessageLevel.normal, tracer, options);
            }

            else
            {
                AddMessage(filename + "に該当するデータが見つかりませんでした．", Tracer.MessageLevel.error, tracer, options);
            }
        }

        else
        {
            AddMessage(ERROR_OverValues(2), Tracer.MessageLevel.error, tracer, options);
        }
    }
}
