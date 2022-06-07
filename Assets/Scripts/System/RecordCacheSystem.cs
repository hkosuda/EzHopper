using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordCacheSystem : IKernelManager
{
    static public DataListParams CachedData { get; private set; }

    static public Dictionary<string, DataListParams> CachedDataList { get; private set; }

    public void Initialize()
    {
        CachedDataList = new Dictionary<string, DataListParams>();

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
            PlayerRecorder.RecordingEnd += CacheData;
        }

        else
        {
            PlayerRecorder.RecordingEnd -= CacheData;
        }
    }

    public void Reset()
    {

    }

    static void CacheData(object obj, List<float[]> dataList)
    {
        CachedData = new DataListParams(MapsManager.CurrentMap.MapName, dataList);
    }

    static public void CacheData(string name, Tracer tracer)
    {
        if (CachedData == null)
        {
            tracer.AddMessage("データが存在しないため，保存に失敗しました．", Tracer.MessageLevel.error);
            return;
        }

        if (CachedData.dataList.Count == 0)
        {
            tracer.AddMessage("データの長さが0であるため，保存できません．", Tracer.MessageLevel.error);
            return;
        }

        if (CachedDataList.ContainsKey(name))
        {
            tracer.AddMessage("すでに同名のデータが存在するため，保存できません．", Tracer.MessageLevel.error);
            return;
        }

        CachedDataList.Add(name, new DataListParams(CachedData.mapName, CachedData.dataList));
    }

    public class DataListParams
    {
        public MapName mapName;
        public List<float[]> dataList;

        public DataListParams(MapName mapName, List<float[]> dataList)
        {
            this.mapName = mapName;
            this.dataList = new List<float[]>(dataList);
        }
    }
}
