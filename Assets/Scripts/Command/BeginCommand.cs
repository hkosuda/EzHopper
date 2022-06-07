using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginCommand : Command
{
    public BeginCommand()
    {
        commandName = "begin";

        description = "マップを切り替える機能を提供します．\n" +
            "マップを切り替えるには，'begin'のあとにマップの名称を指定してください．";
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

    public override void CommandMethod(Tracer tracer, List<string> values)
    {
        if (values == null || values.Count == 0) { return; }

        if (values.Count == 1)
        {
            tracer.AddMessage("マップの名称を指定してください．", Tracer.MessageLevel.error);
            return;
        }

        if (values.Count == 2)
        {
            var value = values[1];

            foreach (MapName mapName in Enum.GetValues(typeof(MapName)))
            {
                if (mapName == MapName.none) { continue; }
                if (value.ToLower() != mapName.ToString().ToLower()) { continue; }

                tracer.AddMessage(mapName.ToString().ToLower() + "を開始します．", Tracer.MessageLevel.normal);
                MapsManager.Begin(mapName);
                return;
            }

            tracer.AddMessage(value + "は存在しません．", Tracer.MessageLevel.error);
            return;
        }

        else
        {
            tracer.AddMessage("値を2個以上指定することはできません．", Tracer.MessageLevel.error);
        }
    }
}
