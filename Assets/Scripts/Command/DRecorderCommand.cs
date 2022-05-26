using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DRecorderCommand : Command
{
    public DRecorderCommand()
    {
        commandName = "drecorder";
        description = "デバッグ用の記録システムを提供します．";
    }

    public override bool CheckValues(Tracer tracer, List<string> values)
    {
        if (values == null || values.Count == 0) { return false; }
        if (values.Count < 2) { return false; }

        if (values[1] == "save") { return true; }

        return false;
    }

    public override void CommandMethod(Tracer tracer, List<string> values)
    {
        var dataList = DebugPlayerRecorder.CachedDataList;

        if (dataList == null || dataList.Count == 0) { NoData(tracer); return; }
        if (values == null || values.Count < 2) { return; }
        if (values[1] != "save") { return; }

        if (values.Count > 2)
        {
            var filepath = DemoUtils.FilePath(Filename(values[2]), true);
            DemoUtils.SaveFile(filepath, DebugPlayerRecorder.CachedDataList);
        }

        else
        {
            var filepath = DemoUtils.FilePath(Filename(""), true);
            DemoUtils.SaveFile(filepath, DebugPlayerRecorder.CachedDataList);
        }

        // - inner function
        static void NoData(Tracer tracer)
        {
            tracer.AddMessage("データが存在しないため，保存に失敗しました．", Tracer.MessageLevel.error);
        }

        // - inner function
        static string Filename(string filename)
        {
            var now = DateTime.Now;

            var datetime = now.Year.ToString() + now.Month.ToString() + now.Day.ToString() + "_" +
                now.Hour.ToString() + "h" + now.Minute.ToString() + "m" + now.Second.ToString() + "s" + now.Millisecond.ToString();

            if (filename == "")
            {
                return datetime;
            }

            else
            {
                return filename + "_" + datetime;
            }
        }
    }
}
