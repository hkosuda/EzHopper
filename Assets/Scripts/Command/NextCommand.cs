using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextCommand : Command
{
    public NextCommand()
    {
        commandName = "next";
    }

    public override void CommandMethod(Tracer tracer, List<string> values)
    {
        MapsManager.CurrentMap.Next();
    }
}
