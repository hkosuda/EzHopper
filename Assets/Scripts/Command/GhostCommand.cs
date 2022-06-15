using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostCommand : Command
{
    public GhostCommand()
    {
        commandName = "ghost";
        description = "�S�[�X�g���N������@�\��񋟂��܂��D";
        detail = "�P��'ghost'�Ɠ��͂���ƁC���O�ɋL�^�����v���C���[�̓����Ɋւ���f�[�^���Č����܂��D�f�[�^���Ȃ���΍Č��͍s���܂���D" +
            "�f�[�^�̍쐬��'recorder'�R�}���h���g�p���čs���܂��D'ghost' �������Ŏ��s����ꍇ�́C�K�� 'recorder end' �����s���Ă���s���悤�ɂ��܂��傤�D\n" +
            "�ۑ������f�[�^���Ăяo���Ď��s����Ƃ��́C'ghost play <name>' �����s���Ă��������i<name>�͔C�ӂ̃f�[�^���j�D" +
            "�S�[�X�g���I������ɂ́C'ghost end'�����s���܂��D\n" +
            "�S�[�X�g�́C'demo' �� 'replay' �ƈقȂ�C�f�[�^�̃}�b�v��񂪌��݂̃}�b�v�ƈقȂ�ꍇ�͍Đ����s���Ȃ����ߒ��ӂ��Ă��������D";
    }

    public override List<string> AvailableValues(List<string> values)
    {
        if (values == null || values.Count == 0) { return new List<string>(); }

        if (values.Count < 3)
        {
            return new List<string>()
            {
                "play", "end"
            };
        }

        else if (values.Count < 4)
        {
            var value = values[1];

            if (value == "play")
            {
                var available = new List<string>();

                if (RecordCacheSystem.CachedDataList == null) { return new List<string>(); }

                foreach (var data in RecordCacheSystem.CachedDataList)
                {
                    available.Add(data.Key);
                }

                return available;
            }

            else
            {
                return new List<string>();
            }
        }

        else
        {
            return new List<string>();
        }
    }

    public override void CommandMethod(Tracer tracer, List<string> values, List<string> options)
    {
        if (values == null || values.Count == 0) { return; }

        if (values.Count == 1)
        {
            var dataParams = RecordCacheSystem.CachedData;

            if (dataParams == null)
            {
                AddMessage("�f�[�^�����݂��Ȃ����߁C���p�ł��܂���D", Tracer.MessageLevel.error, tracer, options);
            }

            else
            {
                BeginGhostFromParams(dataParams, tracer, options);
            }
        }

        else if (values.Count == 2)
        {
            var value = values[1];

            if (value == "play")
            {
                AddMessage("�Đ�����f�[�^�̖��O���w�肵�Ă��������D", Tracer.MessageLevel.error, tracer, options);
            }

            else if (value == "end")
            {
                Ghost.EndReplay();
                AddMessage("�S�[�X�g���~���܂����D", Tracer.MessageLevel.normal, tracer, options);
            }

            else
            {
                AddMessage("1�Ԗڂ̒l�Ƃ��ẮC'play'��������'end'�̂ݐݒ�\�ł��D", Tracer.MessageLevel.error, tracer, options);
            }
        }

        else if (values.Count == 3)
        {
            var value = values[1];
            var name = values[2];

            if (value == "play")
            {
                if (RecordCacheSystem.CachedDataList != null && RecordCacheSystem.CachedDataList.ContainsKey(name))
                {
                    var dataParams = RecordCacheSystem.CachedDataList[name];
                    BeginGhostFromParams(dataParams, tracer, options);
                }

                else
                {
                    AddMessage(name + "�Ƃ����f�[�^�͑��݂��܂���D", Tracer.MessageLevel.error, tracer, options);
                }
            }

            else
            {
                AddMessage("�f�[�^�����w�肵�ăS�[�X�g���N������ɂ́C'ghost play <name>'�Ƃ��Ď��s���Ă��������D", Tracer.MessageLevel.error, tracer, options);
            }
        }

        else
        {
            AddMessage("4�ȏ�̒l���w�肷�邱�Ƃ͂ł��܂���D", Tracer.MessageLevel.error, tracer, options);
        }
        

        // - inner function
        static void BeginGhostFromParams(RecordCacheSystem.DataListParams param, Tracer tracer, List<string> options)
        {
            if (param.mapName != MapsManager.CurrentMap.MapName)
            {
                AddMessage("���݂̃}�b�v�ƈقȂ�}�b�v�̃f�[�^�ł��邽�ߎ��s�ł��܂���D", Tracer.MessageLevel.error, tracer, options);
                return;
            }

            Ghost.BeginReplay(param.dataList, param.mapName);
            AddMessage("�S�[�X�g���N�����܂����D", Tracer.MessageLevel.normal, tracer, options);
        }
    }
}
