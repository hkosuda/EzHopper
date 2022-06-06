using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PM_PostProcessor : ControllerComponent
{
    public override bool Update(float dt)
    {
        var cv = PM_ClipVector.ClipVector;

        if (PM_Jumping.JumpingBegin)
        {
            PM_Main.Rb.velocity = new Vector3(cv.x, 5.85f, cv.z);
        }

        else
        {
            PM_Main.Rb.velocity = new Vector3(cv.x, cv.y, cv.z);
        }

        return true;
    }
}
