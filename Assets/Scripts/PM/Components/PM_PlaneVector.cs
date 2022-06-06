using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PM_PlaneVector : ControllerComponent
{
    static public Vector2 PlaneVector { get; private set; }

    public override bool Update(float dt)
    {
        var landingIndicator = PM_Landing.LandingIndicator;

        var currentVector = CurrentVector();
        var inputVector = PM_InputVector.InputVector;

        // in the air
        if (landingIndicator < 0 || PM_Jumping.JumpingBegin)
        {
            PlaneVector = PmUtil.CalcVector(inputVector, currentVector, dt, false);
            return true;
        }

        // half landing
        if (landingIndicator == 0)
        {
            PlaneVector = PmUtil.CalcVector(inputVector, currentVector, dt, false);
            return true;
        }

        // perfect landing
        else
        {
            PlaneVector = PmUtil.CalcVector(inputVector, currentVector, dt, true);
        }

        return true;

        static Vector2 CurrentVector()
        {
            var v = PM_Main.Rb.velocity;
            return new Vector2(v.x, v.z);
        }
    }
}
