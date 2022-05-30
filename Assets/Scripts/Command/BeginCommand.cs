using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginCommand : Command
{
    public BeginCommand()
    {
        commandName = "begin";
    }

    public override List<string> AvailableValues(List<string> values)
    {
        var list = new List<string>();

        foreach(var map in MapsManager.MapList.Keys)
        {
            list.Add(map.ToString());
        }

        return list;
    }

    public override bool CheckValues(Tracer tracer, List<string> values)
    {
        if (values == null || values.Count < 2) 
        {
            tracer.AddMessage("’l‚ðŽw’è‚µ‚Ä‚­‚¾‚³‚¢", Tracer.MessageLevel.error);
            return false; 
        }

        var value = values[1];

        foreach(MapName mapName in Enum.GetValues(typeof(MapName)))
        {
            if (value.ToLower() == mapName.ToString().ToLower())
            {
                return true;
            }
        }

        return false;
    }

    public override void CommandMethod(Tracer tracer, List<string> values)
    {
        if (values == null || values.Count < 2) { return; }

        var value = values[1];

        foreach (MapName mapName in Enum.GetValues(typeof(MapName)))
        {
            if (value.ToLower() != mapName.ToString().ToLower()) { continue; }

            MapsManager.Begin(mapName);
        }
    }
}
