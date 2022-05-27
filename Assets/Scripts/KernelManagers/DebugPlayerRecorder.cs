using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPlayerRecorder : IKernelManager
{
    static public List<float[]> CachedDataList { get; private set; }

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

    }

    static void SetEvent(int indicator)
    {
        if (indicator > 0)
        {
            PlayerRecorder.RecordingEnd += CacheData;
        }

        else
        {
            PlayerRecorder.RecordingEnd -= CacheData;
        }
    }

    static void CacheData(object obj ,List<float[]> dataList)
    {
        CachedDataList = new List<float[]>(dataList);
    }
}
