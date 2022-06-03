using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PM_ClipVector : ControllerComponent
{
    static readonly float dyOnSlope = 1.062f;

    static public Vector3 ClipVector { get; private set; }

    public override void Initialize()
    {
        base.Initialize();
    }

    public override void Shutdown()
    {
        base.Shutdown();
    }

    public override void Update(float dt)
    {
        var pv = PM_PlaneVector.PlaneVector;
        var v = new Vector3(pv.x, PM_Main.Rb.velocity.y, pv.y);

        //ClipVector = v; return;

        if (PM_Landing.HitInfo.collider == null)
        {
            ClipVector = v;
            return;
        }

        if (PM_Landing.HitInfo.collider.gameObject.layer == 7 && PM_Landing.DeltaY < dyOnSlope)
        {
            CalcClipVector(v);
            return;
        }

        //if (CheckSlopeAbove())
        //{
        //    CalcClipVector(v);
        //    return;
        //}

        ClipVector = v;
    }

    static bool CheckSlopeAbove()
    {
        var radius = PM_Main.playerRadius - 0.02f;
        var rbPosition = PM_Main.Rb.transform.position;

        if (Physics.SphereCast(rbPosition, radius, Vector3.up, out RaycastHit hitInfo, Mathf.Infinity, 1 << 7))
        {
            var deltaY = hitInfo.point.y - rbPosition.y;
            if (deltaY <= dyOnSlope + PM_Landing.landingHeightEpsilon) { return true; }
        }

        return false;
    }

    static void CalcClipVector(Vector3 v)
    {
        Debug.Log("CLIP");

        var normal = PM_Landing.HitInfo.normal;
        var backoff = Vector3.Dot(v, normal);

        var vx = v.x - normal.x * backoff;
        var vy = v.y - normal.y * backoff;
        var vz = v.z - normal.z * backoff;

        ClipVector = new Vector3(vx, vy, vz);
    }
}
