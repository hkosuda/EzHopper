using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordCacheSystem : IKernelManager
{
    static public DataListParams CachedData { get; private set; }
    static public Dictionary<string, DataListParams> CachedDataList { get; private set; }

    static string lastDataName = "";

    public void Initialize()
    {
        CachedDataList = new Dictionary<string, DataListParams>();

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
            PlayerRecorder.RecordingEnd += CacheDataOnRecordingEnd;
        }

        else
        {
            PlayerRecorder.RecordingEnd -= CacheDataOnRecordingEnd;
        }
    }

    static void CacheDataOnRecordingEnd(object obj, List<float[]> dataList)
    {
        CachedData = new DataListParams(MapsManager.CurrentMap.MapName, dataList);
    }

    static public void CacheData(string name, Tracer tracer, List<string> options)
    {
        if (CachedData == null)
        {
            Command.AddMessage("�f�[�^�����݂��Ȃ����߁C�ۑ��Ɏ��s���܂����D", Tracer.MessageLevel.error, tracer, options);
            return;
        }

        if (CachedData.dataList.Count == 0)
        {
            Command.AddMessage("�f�[�^�̒�����0�ł��邽�߁C�ۑ��ł��܂���D", Tracer.MessageLevel.error, tracer, options);
            return;
        }

        if (CachedDataList.ContainsKey(name))
        {
            Command.AddMessage("���łɓ����̃f�[�^�����݂��邽�߁C�ۑ��ł��܂���F" + name, Tracer.MessageLevel.error, tracer, options);
            return;
        }

        CachedDataList.Add(name, new DataListParams(CachedData.mapName, CachedData.dataList));

        Command.AddMessage("�f�[�^��ۑ����܂����D", Tracer.MessageLevel.normal, tracer, options);
        Command.AddMessage("���́F" + name, Tracer.MessageLevel.normal, tracer, options, 2);
        Command.AddMessage("�}�b�v�F" + CachedData.mapName.ToString(), Tracer.MessageLevel.normal, tracer, options, 2);
        Command.AddMessage("�p�����ԁF" + CachedData.dataList.Last()[0].ToString("f1"), Tracer.MessageLevel.normal, tracer, options, 2);

        lastDataName = name;
    }

    static public void RemoveData(string name, Tracer tracer, List<string> options)
    {
        if (CachedDataList.ContainsKey(name))
        {
            CachedDataList.Remove(name);
            Command.AddMessage(name + "���폜���܂����D", Tracer.MessageLevel.normal, tracer, options);
        }

        else
        {
            Command.AddMessage(name + "�Ƃ����f�[�^�͑��݂��܂���D", Tracer.MessageLevel.error, tracer, options);
            return;
        }
    }

    static public void RemoveLast(Tracer tracer, List<string> options)
    {
        if (CachedDataList == null || CachedDataList.Count == 0)
        {
            Command.AddMessage("���ݕۑ�����Ă���f�[�^�͂���܂���D", Tracer.MessageLevel.warning, tracer, options);
        }

        else
        {
            if (CachedDataList.ContainsKey(lastDataName))
            {
                CachedDataList.Remove(lastDataName);
                Command.AddMessage("�Ō�ɕۑ����ꂽ�f�[�^�i" + lastDataName + "���폜���܂����D", Tracer.MessageLevel.normal, tracer, options);
            }

            else
            {
                Command.AddMessage("�Ō�ɕۑ����s��ꂽ�f�[�^�̖��O�i" + lastDataName + "�j�ɑΉ�����f�[�^�����݂��܂���D", Tracer.MessageLevel.warning, tracer, options);
            }
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
