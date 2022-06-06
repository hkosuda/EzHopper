using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DRecorderCommand : Command
{
    public DRecorderCommand()
    {
        commandName = "drecorder";
        description = "�f�o�b�O�p�̋L�^�V�X�e����񋟂��܂��D";
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
            var filepath = DemoFileUtils.FilePath(Filename(values[2]), true);
            DemoFileUtils.SaveFile(filepath, DebugPlayerRecorder.CachedDataList);

            ChatMessages.SendChat("�t�@�C����ۑ����܂����D", ChatMessages.Sender.system);
        }

        else
        {
            var filepath = DemoFileUtils.FilePath(Filename(""), true);
            DemoFileUtils.SaveFile(filepath, DebugPlayerRecorder.CachedDataList);

            ChatMessages.SendChat("�t�@�C����ۑ����܂����D", ChatMessages.Sender.system);
        }

        // - inner function
        static void NoData(Tracer tracer)
        {
            tracer.AddMessage("�f�[�^�����݂��Ȃ����߁C�ۑ��Ɏ��s���܂����D", Tracer.MessageLevel.error);
        }

        // - inner function
        static string Filename(string filename)
        {
            var now = DateTime.Now;

            var datetime = now.Month.ToString() + "M" + now.Day.ToString() + "D" + "_" + now.Hour.ToString() + "h" + now.Minute.ToString() + "m" + now.Second.ToString() + "s";

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
