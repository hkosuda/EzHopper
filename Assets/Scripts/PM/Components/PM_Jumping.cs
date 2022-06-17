using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PM_Jumping : ControllerComponent
{
    // constants
    static readonly int jumpingFrameBuffer = 10;

    // valiables
    static public int JumpingFrameBufferRemain { get; private set; }
    static public bool JumpingBegin { get; private set; }
    static public bool AutoJump { get; set; }

    public override void Initialize()
    {
        SetEvent(1);
    }

    public override void Shutdown()
    {
        SetEvent(-1);
    }

    static void SetEvent(int indicator)
    {
        if (indicator > 0)
        {
            InvalidArea.CourseOut += InactivateAutoJumpOnCourseOut;
        }

        else
        {
            InvalidArea.CourseOut -= InactivateAutoJumpOnCourseOut;
        }
    }

    public override bool Update(float dt)
    {
        if (Keyconfig.CheckInput(Keyconfig.KeyAction.autoJump, true)) { AutoJump = !AutoJump; }

        if (PM_Landing.LandingIndicator < 0) 
        {
            JumpingFrameBufferRemain--;
            if (JumpingFrameBufferRemain < 0) { JumpingFrameBufferRemain = 0; }

            return true; 
        }

        if (PM_Main.Suspend) { return true; }

        if (JumpingFrameBufferRemain <= 0 && !PM_StoppingDetector.Stopped)
        {
            if (Keyconfig.CheckInput(Keyconfig.KeyAction.jump, true) || AutoJump)
            {
                JumpingFrameBufferRemain = jumpingFrameBuffer;
                JumpingBegin = true;
            }
        }

        return true;
    }

    public override void LateUpdate()
    {
        JumpingBegin = false;
    }

    static void InactivateAutoJumpOnCourseOut(object obj, Vector3 position)
    {
        InactivateAutoJump();
    }

    static public void InactivateAutoJump()
    {
        AutoJump = false;
    }
}
