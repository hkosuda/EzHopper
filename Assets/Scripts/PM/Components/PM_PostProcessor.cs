using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PM_PostProcessor : ControllerComponent
{
    public override void Update(float dt)
    {
        var pvec = PM_PlaneVector.PlaneVector;

        if (PM_Jumping.JumpingBegin)
        {
            PM_Main.Rb.velocity = new Vector3(pvec.x, 4.2f, pvec.y);
        }

        else
        {
            PM_Main.Rb.velocity = new Vector3(pvec.x, PM_Main.Rb.velocity.y, pvec.y);
        }
    }
}
