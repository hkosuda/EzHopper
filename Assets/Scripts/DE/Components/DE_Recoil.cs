using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DE_Recoil : ControllerComponent
{
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
            DE_Shooter.Shot += StartRecoilling;
        }

        else
        {
            DE_Shooter.Shot -= StartRecoilling;
        }
    }

    static void StartRecoilling(object obj, Vector3 direction)
    {
        LinerDampingSolver.Initialize();
    }

    public override void Update(float dt)
    {
        if (!LinerDampingSolver.Active) { return; }

        LinerDampingSolver.UpdateTime(dt);

        var addRot = LinerDampingSolver.GetPosition();
        PM_Camera.SetAddRot(addRot[0], addRot[1]);
    }
}
