using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PM_Landing : ControllerComponent
{
    static public EventHandler<bool> Landed { get; set; }

    /// <summary>
    /// landing indicator ... 1: perfect landing, 0: half landing (landing. but processing as 'in the air'), -1: in the air
    /// </summary>
    static public int LandingIndicator { get; private set; }

    // constants
    static public readonly int landingFrameBuffer = 5;
    static public readonly float sphereRadius = 0.1f;
    static public readonly float landingHeightEpsilon = 0.001f;

    // valiables
    static int landingFrameBufferRemain;

    public override void Update(float dt)
    {
        LandingIndicator = CheckLanding();
    }

    // main function
    static int CheckLanding()
    {
        if (PM_Jumping.JumpingFrameBufferRemain > 0)
        {
            return -1;
        }

        if (SphereCastCheck())
        {
            return LandingOrHalf();
        }

        InitializeLandingFrameBuffer();
        return -1;
    }

    // sub functions
    static void InitializeLandingFrameBuffer()
    {
        landingFrameBufferRemain = landingFrameBuffer;
    }

    static int LandingOrHalf()
    {
        if (LandingIndicator < 0)
        {
            Landed?.Invoke(null, false);
        }

        landingFrameBufferRemain--;

        if (landingFrameBufferRemain > 0)
        {
            return 0;
        }

        landingFrameBufferRemain = 0;
        return 1;
    }

    static bool SphereCastCheck()
    {
        var r = PM_Main.playerRadius - sphereRadius;
        var rbPosition = PM_Main.Rb.position;

        for (var n = 0; n < 6; n++)
        {
            var angle = 60.0f * n * Mathf.Deg2Rad;

            var dx = r * Mathf.Sin(angle);
            var dz = r * Mathf.Cos(angle);

            var origin = rbPosition + new Vector3(dx, 0.0f, dz);

            if (Physics.SphereCast(origin: origin, sphereRadius, Vector3.down, out RaycastHit hit))
            {
                var deltaY = rbPosition.y - hit.point.y;

                if (deltaY <= PM_Main.centerY + landingHeightEpsilon)
                {
                    return true;
                }
            }
        }

        return false;
    }

    static bool BoxCastCheck()
    {
        var half = PM_Main.playerRadius * 0.35f;
        var rbPosition = PM_Main.Rb.transform.position;

        if (Physics.BoxCast(rbPosition, new Vector3(half, half, half), Vector3.down, hitInfo: out RaycastHit hit))
        {
            var deltaY = rbPosition.y - hit.point.y;

            if (deltaY <= PM_Main.centerY + landingHeightEpsilon)
            {
                return true;
            }
        }

        return false;
    }
}
