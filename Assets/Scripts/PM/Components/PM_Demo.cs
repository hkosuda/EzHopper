using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PM_Demo : ControllerComponent
{
    static public EventHandler<bool> Landed { get; set; }

    static readonly int frameBuffer = 3;

    static bool isLanding;
    static int frameBufferRemain;

    public override bool Update(float dt)
    {
        var _isLanding = SphereCastCheck();

        if (!_isLanding)
        {
            frameBufferRemain--;
            if (frameBufferRemain < 0) { frameBufferRemain = 0; }
        }

        if (!isLanding && _isLanding)
        {
            if (frameBufferRemain <= 0)
            {
                Landed?.Invoke(null, false);
            }
        }

        if (_isLanding)
        {
            frameBufferRemain = frameBuffer;
        }

        isLanding = _isLanding;
        return true;
    }

    static bool SphereCastCheck()
    {
        var r = PM_Main.playerRadius - PM_Landing.sphereRadius;
        var rbPosition = PM_Main.Rb.position;

        for (var n = 0; n < 6; n++)
        {
            var angle = 60.0f * n * Mathf.Deg2Rad;

            var dx = r * Mathf.Sin(angle);
            var dz = r * Mathf.Cos(angle);

            var origin = rbPosition + new Vector3(dx, 0.0f, dz);

            if (Physics.SphereCast(origin: origin, PM_Landing.sphereRadius, Vector3.down, out RaycastHit hit))
            {
                var deltaY = rbPosition.y - hit.point.y;

                if (deltaY <= PM_Main.centerY + PM_Landing.landingHeightEpsilon)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
