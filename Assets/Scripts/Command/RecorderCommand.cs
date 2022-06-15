using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecorderCommand : Command
{
    static List<string> availables = new List<string>()
    {
        "start", "end", "stop", "save", "remove", "remove_last"
    };

    public RecorderCommand()
    {
        commandName = "recorder";
        description = "�v���C���[�̓������L�^����@�\�i���R�[�_�[�j��񋟂��܂��D\n";
        detail = "'recorder start' �ŋL�^���J�n���C'recorder end' �ŋL�^���~���܂��D�L�^�����f�[�^�́C���̋L�^���I������܂ňꎞ�I�ɕۑ�����܂��D" + 
            "'ghost' �� 'replay' �̎��s���ɗ��p�����f�[�^�́C���̈ꎞ�I�ɕۑ����ꂽ�f�[�^�ł��D\n" +
            "'recorder stop' �����s����ƁC�ꎞ�I�ȕۑ��f�[�^�����������邱�ƂȂ����R�[�_�[���~�ł��܂��D" + 
            "�ꎞ�I�ɕۑ�����Ă���Ԃ�'recorder save <name>'�����s����ƁC�Q�[�����N�����Ă���Ԃ������O�t���Ńf�[�^��ێ��������܂��i<name>�̕����ɔC�ӂ̖��O����͂��܂��j�D" +
            "�����ō쐬�������O�t���f�[�^�́C'replay' �R�}���h�� 'ghost' �R�}���h�ŗ��p�\�ƂȂ�܂��D\n" +
            "���R�[�_�[�́C" + Floats.Item.recorder_limit_time.ToString() + "�Ŏw�肳�ꂽ���Ԃ��o�߂���Ǝ����Œ�~���܂��D\n" +
            "�ۑ������f�[�^���폜����ɂ́C'recorder remove <name>'�����s���Ă��������D�܂��C'remove_last' �ōŌ�ɕۑ����s��ꂽ�f�[�^�������f�[�^���폜���邱�Ƃ��ł��܂��D";
    }

    public override List<string> AvailableValues(List<string> values)
    {
        if (values == null || values.Count == 0) { return new List<string>(); }

        if (values.Count < 3)
        {
            return availables;
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

            else if (value == "remove_last")
            {
                RecordCacheSystem.RemoveLast(tracer, options);
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
                AddMessage(ERROR_AvailableOnly(1, availables), Tracer.MessageLevel.error, tracer, options);
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
                AddMessage("�l��2�w�肷��ꍇ�C" + ERROR_AvailableOnly(1, new List<string>() { "save", "remove" }), Tracer.MessageLevel.error, tracer, options);
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
