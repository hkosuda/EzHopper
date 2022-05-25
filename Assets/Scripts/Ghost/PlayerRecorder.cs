using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRecorder : MonoBehaviour
{
    static public EventHandler<List<float[]>> EndRecording { get; set; }

    static public readonly int dataSize = 10;

    static List<float[]> dataList;
    static bool recording;

    static float pastTime;

    void Start()
    {
        SetEvent(1);
    }

    private void OnDestroy()
    {
        SetEvent(-1);
    }

    static void SetEvent(int indicator)
    {
        if (indicator > 0)
        {
            Timer.Updated += UpdateMethod;
        }

        else
        {
            Timer.Updated -= UpdateMethod;
        }
    }

    static void UpdateMethod(object obj, float dt)
    {
        if (!recording) { return; }
        if (Ghost.DemoMode) { return; }

        if (dataList == null) { dataList = new List<float[]>(); }

        pastTime += dt;

        var data = new float[dataSize];
        var pos = PM_Main.Myself.transform.position;
        var rot = PM_Camera.EulerAngles();

        data[0] = pastTime;
        data[1] = pos.x;
        data[2] = pos.y;
        data[3] = pos.z;
        data[4] = rot.x;
        data[5] = rot.y;
        data[6] = CheckInput(Keyconfig.CheckInput(Keyconfig.KeyAction.forward, false));
        data[7] = CheckInput(Keyconfig.CheckInput(Keyconfig.KeyAction.backward, false));
        data[8] = CheckInput(Keyconfig.CheckInput(Keyconfig.KeyAction.right, false));
        data[9] = CheckInput(Keyconfig.CheckInput(Keyconfig.KeyAction.left, false));

        dataList.Add(data);

        // - inner function
        static float CheckInput(bool value)
        {
            if (value) { return 1.0f; }
            return 0.0f;
        }
    }

    static public void BeginRecording()
    {
        dataList = new List<float[]>();
        recording = true;
        pastTime = 0.0f;
    }

    static public void FinishRecording(bool replay, bool demoMode)
    {
        if (!recording) { return; }

        recording = false;
        if (!replay) { return; }

        Ghost.BeginReplay(dataList, demoMode);
        EndRecording?.Invoke(null, new List<float[]>(dataList));

        dataList = new List<float[]>();
    }

    static public int RecordSize()
    {
        return dataList.Count;
    }
}
