using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecorderCommand : Command
{
    public RecorderCommand()
    {
        commandName = "recorder";
        description = "�v���C���[�̓������L�^����@�\�i���R�[�_�[�j��񋟂��܂��D\n" +
            "���R�[�_�[�́C�����ȃG���A�ɐN�������Ƃ��C��������" + ((int)PlayerRecorder.limitTime).ToString() + "�b�o�߂���Ǝ����Œ�~���܂��D\n" +
            "'recorder begin'�ŋL�^���J�n���C'recorder end'�ŋL�^���~���܂��D�L�^�����f�[�^�́C���̋L�^���I������܂ňꎞ�I�ɕۑ�����܂��D\n" +
            "�ꎞ�I�ɕۑ�����Ă���Ԃ�'recorder save <name>'�����s����ƁC�Q�[�����N�����Ă���Ԃ������O�t���Ńf�[�^��ێ��������܂��i<name>�̕����ɔC�ӂ̖��O����͂��܂��D" +
            "�����ō쐬�������O�t���f�[�^�́Cdemo�R�}���h��ghost�R�}���h�ŗ��p�\�ƂȂ�܂��D\n" +
            "�ۑ������f�[�^���폜����ɂ́C'recorder begin <name>'�����s���Ă��������D";
    }

    public override List<string> AvailableValues(List<string> values)
    {
        if (values == null || values.Count == 0) { return new List<string>(); }

        if (values.Count < 3)
        {
            return new List<string>() { "begin", "end", "stop", "save", "remove" };
        }

        return new List<string>();
    }

    public override void CommandMethod(Tracer tracer, List<string> values)
    {
        if (values == null || values.Count == 0) { return; }

        if (values.Count == 1)
        {
            tracer.AddMessage(AvailabeDataList(), Tracer.MessageLevel.normal);
            PlayerRecorder.CurrentRecorderStatus(tracer);
            return;
        }

        if (values.Count == 2)
        {
            var value = values[1];

            if (value == "begin")
            {
                PlayerRecorder.BeginRecording();

                tracer.AddMessage("���R�[�_�[���N�����܂����D", Tracer.MessageLevel.normal);
                return;
            }

            if (value == "end")
            {
                PlayerRecorder.FinishRecording(true);

                tracer.AddMessage("���R�[�_�[���~���܂����D", Tracer.MessageLevel.normal);
                return;
            }

            if (value == "stop")
            {
                PlayerRecorder.FinishRecording(false, true);

                tracer.AddMessage("���R�[�_�[�ɂ��L�^�𒆒f���܂����D", Tracer.MessageLevel.normal);
                return;
            }

            if (value == "save")
            {
                tracer.AddMessage("�f�[�^��ۑ�����ɂ́C���O���w�肵�Ă��������D", Tracer.MessageLevel.error);
                return;
            }

            if (value == "remove")
            {
                tracer.AddMessage("�f�[�^���폜����ɂ́C���O���w�肵�Ă��������D", Tracer.MessageLevel.error);
                return;
            }

            tracer.AddMessage("��Ԗڂ̒l�Ƃ��ẮC'begin', 'end', 'stop', 'save', 'remove' �̂ݎw��\�ł��D", Tracer.MessageLevel.error);
            return;
        }
        
        if (values.Count == 3)
        {
            if (values[1] == "save")
            {
                RecordCacheSystem.CacheData(values[2], tracer);
                return;
            }

            if (values[1] == "remove")
            {
                RecordCacheSystem.RemoveData(values[2], tracer);
                return;
            }

            else
            {
                tracer.AddMessage("��Ԗڂ̒l�Ƃ��ẮC'begin', 'end', 'stop', 'save', 'remove' �̂ݎw��\�ł��D", Tracer.MessageLevel.error);
                return;
            }
        }

        tracer.AddMessage("3�ȏ�̒l���w�肷�邱�Ƃ͂ł��܂���D", Tracer.MessageLevel.error);
    }

    static string AvailabeDataList()
    {
        var text = "���p�\�ȃf�[�^�̈ꗗ�i�f�[�^��|�}�b�v��|�f�[�^�T�C�Y|�p�����ԁj\n";

        if (RecordCacheSystem.CachedDataList == null || RecordCacheSystem.CachedDataList.Count == 0)
        {
            text += "\t\t�i���ݗ��p�\�ȃf�[�^�͂���܂���j";
            return text;
        }

        foreach(var  data in RecordCacheSystem.CachedDataList)
        {
            text += "\t\t| " + data.Key + "\t | " + data.Value.mapName + "\t | " + data.Value.dataList.Count 
                + "\t | " + data.Value.dataList.Last()[0].ToString("f1") + "\n";
        }

        return text.TrimEnd(new char[1] { '\n' });
    }
}
