using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PM_Landing : ControllerComponent
{
    static public EventHandler<RaycastHit> Landed { get; set; }

    /// <summary>
    /// landing indicator ... 1: perfect landing, 0: half landing (landing. but processing as 'in the air'), -1: in the air
    /// </summary>
    static public int LandingIndicator { get; private set; }
    static public RaycastHit HitInfo { get; private set; }
    static public float DeltaY { get; private set; }

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

        if (SingleSphereCastCheck())
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
        var previousLandingIndicator = LandingIndicator;

        if (previousLandingIndicator < 0)
        {
            Landed?.Invoke(null, HitInfo);
        }

        landingFrameBufferRemain--;

        if (landingFrameBufferRemain > 0)
        {
            return 0;
        }

        landingFrameBufferRemain = 0;
        return 1;
    }

    static bool SingleSphereCastCheck()
    {
        var radius = PM_Main.playerRadius - 0.02f;
        var rbPosition = PM_Main.Rb.transform.position;

        Physics.SphereCast(rbPosition, radius, Vector3.down, out RaycastHit hitInfo, Mathf.Infinity);
        HitInfo = hitInfo;

        if (hitInfo.collider != null)
        {
            DeltaY = rbPosition.y - hitInfo.point.y;

            if (hitInfo.collider.gameObject.layer == 7) { return false; }

            if (DeltaY <= PM_Main.centerY + landingHeightEpsilon)
            {
                return true;
            }
        }

        return false;
    }
}
