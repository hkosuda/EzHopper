using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginCommand : Command
{
    public BeginCommand()
    {
        commandName = "begin";
        description = "マップを切り替える機能を提供します．";
        detail = "マップを切り替えるには，'begin'のあとにマップの名称を指定してください．" +
            "たとえば，最初のマップに移動したい場合は 'begin ez_athletic' を実行します．\n" +
            "※マップの名称の一覧：\n" +
            "\tez_athletic\n" +
            "\tez_square\n" +
            "\tez_square2\n" +
            "\tez_nostalgia\n" +
            "\tez_horizon\n" +
            "\tez_training\n" +
            "\tez_flyer\n" +
            "\tez_settingroom";
    }

    public override List<string> AvailableValues(List<string> values)
    {
        if (values == null || values.Count == 0) { return new List<string>(); }

        if (values.Count < 3)
        {
            var list = new List<string>();

            foreach (var map in MapsManager.MapList.Keys)
            {
                list.Add(map.ToString());
            }

            return list;
        }

        return new List<string>();
    }

    public override void CommandMethod(Tracer tracer, List<string> values, List<string> options)
    {
        if (values == null || values.Count == 0) { return; }

        if (values.Count == 1)
        {
            AddMessage("マップの名称を指定してください．", Tracer.MessageLevel.error, tracer, options);
        }

        else if (values.Count == 2)
        {
            var value = values[1];

            foreach (MapName mapName in Enum.GetValues(typeof(MapName)))
            {
                if (mapName == MapName.none) { continue; }
                if (value.ToLower() != mapName.ToString().ToLower()) { continue; }

                MapsManager.Begin(mapName);
                AddMessage(mapName.ToString().ToLower() + "を開始します．", Tracer.MessageLevel.normal, tracer, options);
                return;
            }

            AddMessage(value + "は存在しません．", Tracer.MessageLevel.error, tracer, options);
            return;
        }

        else
        {
            AddMessage("値を2個以上指定することはできません．", Tracer.MessageLevel.error, tracer, options);
        }
    }
}
