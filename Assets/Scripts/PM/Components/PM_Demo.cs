using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PM_Demo : ControllerComponent
{
    public override void Update(float dt)
    {
        if (!Ghost.DemoMode) { return; }
    }
}
