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

    static public void CacheData(string name, Tracer tracer, List<string> options)
    {
        if (CachedData == null)
        {
            Command.AddMessage("データが存在しないため，保存に失敗しました．", Tracer.MessageLevel.error, tracer, options);
            return;
        }

        if (CachedData.dataList.Count == 0)
        {
            Command.AddMessage("データの長さが0であるため，保存できません．", Tracer.MessageLevel.error, tracer, options);
            return;
        }

        if (CachedDataList.ContainsKey(name))
        {
            Command.AddMessage("すでに同名のデータが存在するため，保存できません．", Tracer.MessageLevel.error, tracer, options);
            return;
        }

        CachedDataList.Add(name, new DataListParams(CachedData.mapName, CachedData.dataList));

        Command.AddMessage("データを保存しました．", Tracer.MessageLevel.normal, tracer, options);
        Command.AddMessage("名称：" + name, Tracer.MessageLevel.normal, tracer, options, 2);
        Command.AddMessage("マップ：" + CachedData.mapName.ToString(), Tracer.MessageLevel.normal, tracer, options, 2);
        Command.AddMessage("継続時間：" + CachedData.dataList.Last()[0].ToString("f1"), Tracer.MessageLevel.normal, tracer, options, 2);
    }

    static public void RemoveData(string name, Tracer tracer, List<string> options)
    {
        if (CachedDataList.ContainsKey(name))
        {
            CachedDataList.Remove(name);
            Command.AddMessage(name + "を削除しました．", Tracer.MessageLevel.normal, tracer, options);
        }

        else
        {
            Command.AddMessage(name + "というデータは存在しません．", Tracer.MessageLevel.error, tracer, options);
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
