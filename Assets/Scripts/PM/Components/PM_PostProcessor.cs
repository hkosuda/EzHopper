using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PM_PostProcessor : ControllerComponent
{
    public override void Update(float dt)
    {
        //var pvec = PM_PlaneVector.PlaneVector;

        //if (PM_Jumping.JumpingBegin)
        //{
        //    PM_Main.Rb.velocity = new Vector3(pvec.x, 4.3f, pvec.y);
        //}

        //else
        //{
        //    PM_Main.Rb.velocity = new Vector3(pvec.x, PM_Main.Rb.velocity.y, pvec.y);
        //}

        // vy = 4.3f;

        // g : -13, vy : 5.3
        // g : -15, vy : 5.7
        // g : -16, vy : 5.85

        var cv = PM_ClipVector.ClipVector;

        if (PM_Jumping.JumpingBegin)
        {
            PM_Main.Rb.velocity = new Vector3(cv.x, 5.85f, cv.z);
        }

        else
        {
            PM_Main.Rb.velocity = new Vector3(cv.x, cv.y, cv.z);
        }
    }
}
