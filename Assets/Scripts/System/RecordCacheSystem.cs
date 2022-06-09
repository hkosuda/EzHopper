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
            tracer.AddMessage("�f�[�^�����݂��Ȃ����߁C�ۑ��Ɏ��s���܂����D", Tracer.MessageLevel.error);
            return;
        }

        if (CachedData.dataList.Count == 0)
        {
            tracer.AddMessage("�f�[�^�̒�����0�ł��邽�߁C�ۑ��ł��܂���D", Tracer.MessageLevel.error);
            return;
        }

        if (CachedDataList.ContainsKey(name))
        {
            tracer.AddMessage("���łɓ����̃f�[�^�����݂��邽�߁C�ۑ��ł��܂���D", Tracer.MessageLevel.error);
            return;
        }

        CachedDataList.Add(name, new DataListParams(CachedData.mapName, CachedData.dataList));

        var info = "\t\t���́F" + name + "\n\t\t�}�b�v�F" + CachedData.mapName.ToString() + "\n\t\t�p�����ԁF" + CachedData.dataList.Last()[0].ToString("f1");
        tracer.AddMessage("�f�[�^��ۑ����܂����D\n" + info, Tracer.MessageLevel.normal);
    }

    static public void RemoveData(string name, Tracer tracer)
    {
        if (CachedDataList.ContainsKey(name))
        {
            CachedDataList.Remove(name);
            tracer.AddMessage(name + "���폜���܂����D", Tracer.MessageLevel.normal);
        }

        else
        {
            tracer.AddMessage(name + "�Ƃ����f�[�^�͑��݂��܂���D", Tracer.MessageLevel.error);
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
