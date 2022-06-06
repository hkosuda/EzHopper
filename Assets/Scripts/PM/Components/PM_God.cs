using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PM_God : ControllerComponent
{
    static float currentSpeed;

    public override bool Update(float dt)
    {
        if (!GodCommand.Active) { currentSpeed = 0.0f; return true; }

        PM_Main.Rb.velocity = Vector3.zero;

        var pos = PM_Main.Myself.transform.position;
        var vec = GetVector();

        if (vec.magnitude > 0.0f) 
        {
            currentSpeed += Floats.Get(Floats.Item.god_moving_accel) * dt;
        }

        else
        {
            currentSpeed = 0.0f;
        }

        if (currentSpeed > Floats.Get(Floats.Item.god_moving_speed)) { currentSpeed = Floats.Get(Floats.Item.god_moving_speed); }

        PM_Main.Myself.transform.position = pos + GetVector() * currentSpeed * dt;

        return false;
    }

    static Vector3 GetVector()
    {
        var vm = PM_InputVector.ML_InputVector.x;
        var vl = PM_InputVector.ML_InputVector.y;

        var rotX = -PM_Camera.EulerAngles().x * Mathf.Deg2Rad;
        var rotY = PM_Camera.EulerAngles().y * Mathf.Deg2Rad;

        var vz = vm * Mathf.Cos(rotX) * Mathf.Cos(rotY) - vl * Mathf.Sin(rotY);
        var vx = vm * Mathf.Cos(rotX) * Mathf.Sin(rotY) + vl * Mathf.Cos(rotY);
        var vy = vm * Mathf.Sin(rotX);

        return new Vector3(vx, vy, vz).normalized;
    }
}
