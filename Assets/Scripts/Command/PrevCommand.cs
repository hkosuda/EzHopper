using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrevCommand : Command
{
    public PrevCommand()
    {
        commandName = "prev";
    }

    public override void CommandMethod(Tracer tracer, List<string> values)
    {
        MapsManager.CurrentMap.Prev();
    }
}
