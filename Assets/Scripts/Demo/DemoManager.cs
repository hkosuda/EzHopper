using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoManager : IKernelManager
{
    // public 
    static public float[] InterpolatedData { get; private set; }
    static public float PastTime { get; private set; }
    static public float PlaySpeed { get; set; }
    static public float Duration { get; set; }

    // private
    static List<float[]> dataList;

    static GameObject _ui;
    static GameObject ui;

    public void Initialize()
    {
        _ui = Resources.Load<GameObject>("UI/DemoUI");

        SetEvent(1);
    }

    public void Shutdown()
    {
        SetEvent(-1);
    }

    static void SetEvent(int indicator)
    {
        if (indicator > 0)
        {
            DemoTimer.Updated += UpdateMethod;
        }

        else
        {
            DemoTimer.Updated -= UpdateMethod;
        }
    }

    public void Reset()
    {

    }

    static void UpdateMethod(object obj, float dt)
    {
        if (dataList == null) { return; }

        PastTime += dt * PlaySpeed;
        if (PastTime > Duration) { PastTime = Duration; }
        if (PastTime < 0.0f) { PastTime = 0.0f; }

        InterpolatedData = DemoUtils.Interpolate(dataList, PastTime);
        SetPlayerTransform();
    }

    static public void BeginDemo(List<float[]> _dataList)
    {
        dataList = _dataList;

        PastTime = 0.0f;
        PlaySpeed = 1.0f;
        Duration = _dataList.Last()[0];

        Ghost.BeginReplay(dataList);
        PlayerStatusRecorder.Save();

        DemoTimer.Resume();
        ui = GameObject.Instantiate(_ui);

        InputSystem.Inactivate();
    }

    static public void EndDemo()
    {
        PlayerStatusRecorder.Load();
        DemoTimer.Pause();
        Timer.Resume();

        InputSystem.Activate();

        if (ui != null) { GameObject.Destroy(ui); }
    }

    static public void ChangePastTime(float dt, bool directly = false)
    {
        if (directly) { PastTime = dt; } else { PastTime += dt; }

        if (PastTime < 0.0f) { PastTime = 0.0f; }
        if (PastTime > Duration) { PastTime = Duration; }

        InterpolatedData = DemoUtils.Interpolate(dataList, PastTime);
        SetPlayerTransform();
    }

    static void SetPlayerTransform()
    {
        var position = Vec3(InterpolatedData);
        PM_Main.Myself.transform.position = position;

        var rotX = InterpolatedData[4];
        var rotY = InterpolatedData[5];

        PM_Camera.SetEulerAngles(new Vector3(rotX, rotY, 0.0f));

        // - inner function
        static Vector3 Vec3(float[] data, float dy = 0.0f)
        {
            return new Vector3(data[1], data[2] + dy, data[3]);
        }
    }

    static public void ChangeSpeed(float speed)
    {
        if (speed < 0.1f) { speed = 0.1f; }
        if (speed > 5.0f) { speed = 5.0f; }

        PlaySpeed = speed;
    }
}

static public class PlayerStatusRecorder
{
    static public MapName MapName { get; private set; }
    static public Vector3 Position { get; private set; }
    static public Vector3 EulerAngle { get; private set; }
    static public Vector3 Velocity { get; private set; }

    static public void Save()
    {
        Position = PM_Main.Myself.transform.position;
        EulerAngle = PM_Camera.EulerAngles();
        Velocity = PM_Main.Rb.velocity;
    }

    static public void Load()
    {
        PM_Main.Myself.transform.position = Position;
        PM_Camera.SetEulerAngles(new Vector3(0.0f, EulerAngle.y, 0.0f));
        PM_Main.Rb.velocity = Velocity;
    }
}
