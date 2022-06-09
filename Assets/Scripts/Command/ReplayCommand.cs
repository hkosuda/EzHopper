using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayCommand : Command
{
    public ReplayCommand()
    {
        commandName = "replay";
    }

    public override List<string> AvailableValues(List<string> values)
    {
        if (values == null || values.Count == 0) { return new List<string>(); }

        if (values.Count < 3)
        {
            var available = new List<string>();

            if (RecordCacheSystem.CachedDataList == null) { return new List<string>(); }

            foreach(var data in RecordCacheSystem.CachedDataList)
            {
                available.Add(data.Key);
            }

            return available;
        }

        return new List<string>();
    }

    public override void CommandMethod(Tracer tracer, List<string> values)
    {
        if (values == null || values.Count == 0) { return; }

        if (values.Count == 1)
        {
            var dataParams = RecordCacheSystem.CachedData;

            if (dataParams == null)
            {
                tracer.AddMessage("�f�[�^�����݂��Ȃ����߁C�f�����Đ��ł��܂���D", Tracer.MessageLevel.error); 
                return;
            }

            else
            {
                BeginDemoFromParams(dataParams, tracer);
                return;
            }
        }

        if (values.Count == 2)
        {
            var filename = values[1];

            if (RecordCacheSystem.CachedDataList != null && RecordCacheSystem.CachedDataList.ContainsKey(filename))
            {
                var dataParams = RecordCacheSystem.CachedDataList[filename];

                BeginDemoFromParams(dataParams, tracer);
                return;
            }

            else
            {
                tracer.AddMessage(filename + "�Ƃ����f�[�^�͑��݂��܂���D", Tracer.MessageLevel.error);
                return;
            }
        }

        tracer.AddMessage("3�ȏ�̒l���w�肷�邱�Ƃ͂ł��܂���D", Tracer.MessageLevel.error);

        // - inner function
        static void BeginDemoFromParams(RecordCacheSystem.DataListParams param, Tracer tracer)
        {
            if (param.mapName != MapsManager.CurrentMap.MapName)
            {
                tracer.AddMessage("���݂̃}�b�v�ƈقȂ�}�b�v�̃f�[�^�ł��邽�߁C�}�b�v��؂�ւ��Ď��s���܂��D", Tracer.MessageLevel.warning);
                MapsManager.Begin(param.mapName);
            }

            DemoManager.BeginDemo(param.dataList);
        }
    }
}
