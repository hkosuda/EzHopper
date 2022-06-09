using System.Linq;
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

        var info = "\t\t名称：" + name + "\n\t\tマップ：" + CachedData.mapName.ToString() + "\n\t\t継続時間：" + CachedData.dataList.Last()[0].ToString("f1");
        tracer.AddMessage("データを保存しました．\n" + info, Tracer.MessageLevel.normal);
    }

    static public void RemoveData(string name, Tracer tracer)
    {
        if (CachedDataList.ContainsKey(name))
        {
            CachedDataList.Remove(name);
            tracer.AddMessage(name + "を削除しました．", Tracer.MessageLevel.normal);
        }

        else
        {
            tracer.AddMessage(name + "というデータは存在しません．", Tracer.MessageLevel.error);
            return;
        }
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
