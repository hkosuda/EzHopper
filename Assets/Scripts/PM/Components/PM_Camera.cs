using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PM_Camera : ControllerComponent
{
    static float omegaThreshold = 1800.0f;

    static public Camera Camera;
    static public Transform CameraTr;

    static float degRotX;
    static float degRotY;

    static float dxPrev;
    static float dyPrev;

    public override void Initialize()
    {
        CameraTr = PM_Main.Myself.transform.GetChild(0).gameObject.transform;
        Camera = CameraTr.gameObject.GetComponent<Camera>();
        CameraTr.eulerAngles = new Vector3(degRotX, degRotY, 0.0f);
    }

    public override void Update(float dt)
    {
        var dx = Input.GetAxis("Mouse Y") * ClientParams.MouseSensi;
        var dy = Input.GetAxis("Mouse X") * ClientParams.MouseSensi;

        if (Mathf.Abs(dx) / dt > omegaThreshold) { dx = dxPrev; }
        if (Mathf.Abs(dy) / dt > omegaThreshold) { dy = dyPrev; }

        if (degRotX > 90.0f) { degRotX = 90.0f; }
        if (degRotX < -90.0f) { degRotX = -90.0f; }

        degRotX -= dx;
        degRotY += dy;

        CameraTr.eulerAngles = new Vector3(degRotX, degRotY, 0.0f);

        dxPrev = dx;
        dyPrev = dy;
    }

    /// <summary>
    /// camera's euler angles in degree.
    /// </summary>
    static public Vector3 EulerAngles()
    {
        return CameraTr.eulerAngles;
    }

    static public void SetEulerAngles(Vector3 euler)
    {
        CameraTr.eulerAngles = euler;
        degRotX = euler.x;
        degRotY = euler.y;
    }

    static public void Rotate(float _degRotY)
    {
        degRotY += _degRotY;
    }
}
