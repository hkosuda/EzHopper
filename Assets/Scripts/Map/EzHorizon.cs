using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EzHorizon : Map
{
    private void Awake()
    {
        MapName = MapName.ez_horizon;
    }

    public override void Initialize()
    {
        base.Initialize();

        SetEvent(1);
    }

    public override void Shutdown()
    {
        base.Shutdown();

        SetEvent(-1);
    }

    void SetEvent(int indicator)
    {
        if (indicator > 0)
        {
            InGameTimer.Updated += CheckFalling;
            PM_Landing.Landed += UpdateLandingPosition;
        }

        else
        {
            InGameTimer.Updated -= CheckFalling;
            PM_Landing.Landed -= UpdateLandingPosition;
        }
    }

    static Vector3 lastLandingPosition;

    void UpdateLandingPosition(object obj, RaycastHit hit)
    {
        lastLandingPosition = hit.point;
    }

    void CheckFalling(object obj, float dt)
    {
        var y = PM_Main.Myself.transform.position.y - PM_Main.centerY;

        if (y < lastLandingPosition.y - 1.5f)
        {
            var pos = respawnPositions[0].transform.position;
            var rotY = respawnPositions[0].transform.eulerAngles.y;

            PM_Main.ResetPosition(pos, rotY);
        }
    }
}
