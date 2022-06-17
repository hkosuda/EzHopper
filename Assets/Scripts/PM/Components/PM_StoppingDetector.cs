using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PM_StoppingDetector : ControllerComponent
{
    static readonly float stoppingCoeff = 0.7f;
    static readonly int stoppingFrameBuffer = 5;

    static public bool Stopped { get; private set; }

    static int stoppingFrameBufferRemain;
    static float prevVelocity;

    public override bool Update(float dt)
    {
        var v = PM_Main.Rb.velocity;
        var pv = new Vector2(v.x, v.z);

        var velocity = pv.magnitude;

        if (velocity < prevVelocity * stoppingCoeff)
        {
            Stopped = true;
            stoppingFrameBufferRemain = stoppingFrameBuffer;

            Debug.Log("Stopped");
        }

        prevVelocity = velocity;

        return true;
    }

    public override void LateUpdate()
    {
        stoppingFrameBufferRemain--;
        if (stoppingFrameBufferRemain < 0) { stoppingFrameBufferRemain = -1; }

        if (stoppingFrameBufferRemain == 0)
        {
            Stopped = false;
        }
    }
}
