using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PM_ClipVector : ControllerComponent
{
    static readonly float dyOnSlope = 1.070f;

    static public Vector3 ClipVector { get; private set; }

    public override void Initialize()
    {
        base.Initialize();
    }

    public override void Shutdown()
    {
        base.Shutdown();
    }

    public override bool Update(float dt)
    {
        var pv = PM_PlaneVector.PlaneVector;
        var v = new Vector3(pv.x, PM_Main.Rb.velocity.y, pv.y);

        if (PM_Landing.HitInfo.collider == null)
        {
            ClipVector = v;
            return true;
        }

        if (PM_Landing.HitInfo.collider.gameObject.layer == 7 && PM_Landing.DeltaY < dyOnSlope)
        {
            CalcClipVector(v);
            return true;
        }

        ClipVector = v;
        return true;
    }

    static void CalcClipVector(Vector3 v)
    {
        var normal = PM_Landing.HitInfo.normal;
        var backoff = Vector3.Dot(v, normal);

        var vx = v.x - normal.x * backoff;
        var vy = v.y - normal.y * backoff;
        var vz = v.z - normal.z * backoff;

        ClipVector = new Vector3(vx, vy, vz);
    }
}
