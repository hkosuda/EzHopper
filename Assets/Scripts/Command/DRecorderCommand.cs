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

    public override void CommandMethod(Tracer tracer, List<string> values, List<string> options)
    {
        var dataList = DebugPlayerRecorder.CachedDataList;

        if (dataList == null || dataList.Count == 0) { NoData(tracer, options); return; }
        if (values == null || values.Count < 2) { return; }
        if (values[1] != "save") { return; }

        if (values.Count > 2)
        {
            var filepath = DemoFileUtils.FilePath(Filename(values[2]), true);
            DemoFileUtils.SaveFile(filepath, DebugPlayerRecorder.CachedDataList);

            ChatMessages.SendChat("ファイルを保存しました．", ChatMessages.Sender.system);
        }

        else
        {
            var filepath = DemoFileUtils.FilePath(Filename(""), true);
            DemoFileUtils.SaveFile(filepath, DebugPlayerRecorder.CachedDataList);

            ChatMessages.SendChat("ファイルを保存しました．", ChatMessages.Sender.system);
        }

        // - inner function
        static void NoData(Tracer tracer, List<string> options)
        {
            AddMessage("データが存在しないため，保存に失敗しました．", Tracer.MessageLevel.error, tracer, options);
        }

        // - inner function
        static string Filename(string filename)
        {
            var now = DateTime.Now;

            var datetime = "M" + now.Month.ToString() + "D" + now.Day.ToString() + "_" + now.Hour.ToString() + "h" + now.Minute.ToString() + "m" + now.Second.ToString() + "s";

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
