using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRecorder : IKernelManager
{
    static public readonly int dataSize = 10;

    static public EventHandler<List<float[]>> RecordingEnd { get; set; }

    static List<float[]> dataList;
    static public bool Recording;

    static float pastTime;

    public void Initialize()
    {
        SetEvent(1);
    }

    public void Shutdown()
    {
        SetEvent(-1);
    }

    public void Reset()
    {
        FinishRecording(false);
    }

    static void SetEvent(int indicator)
    {
        if (indicator > 0)
        {
            InGameTimer.Updated += UpdateMethod;
            MapsManager.Initialized += StopRecorderOnMapInitialized;
        }

        else
        {
            InGameTimer.Updated -= UpdateMethod;
            MapsManager.Initialized -= StopRecorderOnMapInitialized;
        }
    }

    static void UpdateMethod(object obj, float dt)
    {
        if (!Recording) { return; }
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

        var limitTime = Floats.Get(Floats.Item.recorder_limit_time);

        if (pastTime > limitTime)
        {
            ChatMessages.SendChat(limitTime.ToString() + "秒が経過したため，レコーダーを停止します．", ChatMessages.Sender.system);
            FinishRecording(true);
        }

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
        Recording = true;
        pastTime = 0.0f;
    }

    static public void FinishRecording(bool cache)
    {
        if (!Recording) { return; }
        Recording = false;

        if (dataList == null || dataList.Count == 0) { return; }

        if (cache) { RecordingEnd?.Invoke(null, new List<float[]>(dataList)); }
        dataList = new List<float[]>();
    }

    static public int DataListSize()
    {
        if (dataList == null) { return 0; }
        return dataList.Count;
    }

    static void StopRecorderOnMapInitialized(object obj, bool mute)
    {
        FinishRecording(false);
    }
}
