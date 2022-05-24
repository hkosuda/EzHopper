using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PM_PlaneVector : ControllerComponent
{
    static public Vector2 PlaneVector { get; private set; }

    public override void Update(float dt)
    {
        if (Ghost.DemoMode) { return; }

        var landingIndicator = PM_Landing.LandingIndicator;

        var currentVector = CurrentVector();
        var inputVector = PM_InputVector.InputVector;

        // in the air
        if (landingIndicator < 0 || PM_Jumping.JumpingBegin)
        {
            PlaneVector = PmUtil.CalcVector(inputVector, currentVector, dt, false);
            return;
        }

        // half landing
        if (landingIndicator == 0)
        {
            PlaneVector = PmUtil.CalcVector(inputVector, currentVector, dt, false);
            return;
        }

        // perfect landing
        else
        {
            PlaneVector = PmUtil.CalcVector(inputVector, currentVector, dt, true);
        }

        static Vector2 CurrentVector()
        {
            var v = PM_Main.Rb.velocity;
            return new Vector2(v.x, v.z);
        }
    }
}
