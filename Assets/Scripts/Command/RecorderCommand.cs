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
            "'recorder start'�ŋL�^���J�n���C'recorder end'�ŋL�^���~���܂��D�L�^�����f�[�^�́C���̋L�^���I������܂ňꎞ�I�ɕۑ�����܂��D\n" +
            "�ꎞ�I�ɕۑ�����Ă���Ԃ�'recorder save <name>'�����s����ƁC�Q�[�����N�����Ă���Ԃ������O�t���Ńf�[�^��ێ��������܂��i<name>�̕����ɔC�ӂ̖��O����͂��܂��D" +
            "�����ō쐬�������O�t���f�[�^�́Cdemo�R�}���h��ghost�R�}���h�ŗ��p�\�ƂȂ�܂��D\n" +
            "�ۑ������f�[�^���폜����ɂ́C'recorder start <name>'�����s���Ă��������D";
    }

    public override List<string> AvailableValues(List<string> values)
    {
        if (values == null || values.Count == 0) { return new List<string>(); }

        if (values.Count < 3)
        {
            return new List<string>() { "start", "end", "stop", "save", "remove" };
        }

        return new List<string>();
    }

    public override void CommandMethod(Tracer tracer, List<string> values, List<string> options)
    {
        if (values == null || values.Count == 0) { return; }

        if (values.Count == 1)
        {
            AddMessage(AvailabeDataList(), Tracer.MessageLevel.normal, tracer, options);
        }

        else if (values.Count == 2)
        {
            var value = values[1];

            if (value == "start")
            {
                PlayerRecorder.BeginRecording();
                AddMessage("���R�[�_�[���N�����܂����D", Tracer.MessageLevel.normal, tracer, options);
            }

            else if (value == "end")
            {
                PlayerRecorder.FinishRecording(true);
                AddMessage("���R�[�_�[���~���܂����D", Tracer.MessageLevel.normal, tracer, options);
            }

            else if (value == "stop")
            {
                PlayerRecorder.FinishRecording(false);
                AddMessage("���R�[�_�[�ɂ��L�^�𒆒f���܂����D", Tracer.MessageLevel.normal, tracer, options);
            }

            else if (value == "save")
            {
                AddMessage("�f�[�^��ۑ�����ɂ́C���O���w�肵�Ă��������D", Tracer.MessageLevel.error, tracer, options);
            }

            else if (value == "remove")
            {
                AddMessage("�f�[�^���폜����ɂ́C���O���w�肵�Ă��������D", Tracer.MessageLevel.error, tracer, options);
            }

            else
            {
                AddMessage("��Ԗڂ̒l�Ƃ��ẮC'start', 'end', 'stop', 'save', 'remove' �̂ݎw��\�ł��D", Tracer.MessageLevel.error, tracer, options);
            }
        }
        
        else if (values.Count == 3)
        {
            var value = values[1];
            var name = values[2];

            if (value == "save")
            {
                RecordCacheSystem.CacheData(name, tracer, options);
            }

            else if (value == "remove")
            {
                RecordCacheSystem.RemoveData(name, tracer, options);
            }

            else
            {
                AddMessage("��Ԗڂ̒l�Ƃ��ẮC'start', 'end', 'stop', 'save', 'remove' �̂ݎw��\�ł��D", Tracer.MessageLevel.error, tracer, options);
            }
        }

        else
        {
            AddMessage("3�ȏ�̒l���w�肷�邱�Ƃ͂ł��܂���D", Tracer.MessageLevel.error, tracer, options);
        }
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
