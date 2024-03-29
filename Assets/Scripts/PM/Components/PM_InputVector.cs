using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PM_InputVector : ControllerComponent
{
    static public Vector2 ML_InputVector { get; private set; }
    static public Vector2 InputVector { get; private set; }

    static float prevRotY;

    public override bool Update(float dt)
    {
        if (PM_Main.Suspend)
        {
            InputVector = Vector2.zero;
            return true;
        }

        var vm = 0.0f;
        var vl = 0.0f;

        if (Keyconfig.CheckInput(Keyconfig.KeyAction.forward, false)) { vm += 1.0f; }
        if (Keyconfig.CheckInput(Keyconfig.KeyAction.backward, false)) { vm += -1.0f; }

        if (Keyconfig.CheckInput(Keyconfig.KeyAction.right, false)) { vl += 1.0f; }
        if (Keyconfig.CheckInput(Keyconfig.KeyAction.left, false)) { vl += -1.0f; }

        ML_InputVector = new Vector2(vm, vl).normalized;

        var rotY = PM_Camera.EulerAngles().y * Mathf.Deg2Rad;

        var vz = vm * Mathf.Cos(rotY) - vl * Mathf.Sin(rotY);
        var vx = vm * Mathf.Sin(rotY) + vl * Mathf.Cos(rotY);

        if (vz == 0.0f && vx == 0.0f)
        {
            InputVector = Vector2.zero;
            return true;
        }

        InputVector = new Vector2(vx, vz).normalized;

        return true;
    }
}
