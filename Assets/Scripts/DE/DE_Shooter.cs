using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DE_Shooter : ControllerComponent
{
    static public EventHandler<Vector3> Shot { get; set; }
    static public EventHandler<RaycastHit> ShootingHit { get; set; }

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
        if (!Keyconfig.CheckInput(Keyconfig.KeyAction.shot, true)) { return; }
        if (!DE_Availability.Availability) { return; }

        DeShot();
    }

    static void DeShot()
    {
        var ray = PM_Camera.Camera.ScreenPointToRay(Input.mousePosition);

        Shot?.Invoke(null, ray.direction);

        if (Physics.Raycast(ray, hitInfo: out RaycastHit hit))
        {
            ShootingHit?.Invoke(null, hit);
        }
    }
}
