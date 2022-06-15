using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackCommand : Command
{
    public BackCommand()
    {
        commandName = "back";
        description = "�Ō�ɓ��B�����`�F�b�N�|�C���g�܂Ŗ߂�܂��D�l���w�肷��ƁC���̒l�ɑΉ�����`�F�b�N�|�C���g�܂Ŗ߂�܂��D";
        detail = "�P��'back'�Ƃ��Ď��s����ƁC�Ō�ɓ��B�����`�F�b�N�|�C���g�܂Ŗ߂�܂��D\n" +
            "'back 1'�ȂǁC�l���w�肵�Ď��s����Ƃ��̒l�ɑΉ�����`�F�b�N�|�C���g�܂Ŗ߂�܂��D�`�F�b�N�|�C���g�̔ԍ��́C" +
            "�X�^�[�g�ʒu�ƂȂ�`�F�b�N�|�C���g��0�C���̎���1...�Ƃ������悤�ɁC0����n�܂�̂Œ��ӂ��Ă��������D\n" +
            "���on_course_out�����������Ƃ��Ɏ������s�����邩�C�蓮�œ���̃`�F�b�N�|�C���g�Ɉړ��������Ƃ��Ɏg�p���܂��D";
    }

    public override List<string> AvailableValues(List<string> values)
    {
        if (values == null || values.Count == 0) { return new List<string>(); }

        else if (values.Count < 3)
        {
            var list = new List<string>();

            for(var n = 0; n < MapsManager.CurrentMap.respawnPositions.Length; n++)
            {
                list.Add(n.ToString());
            }

            return list;
        }

        else
        {
            return new List<string>();
        }
    }

    public override void CommandMethod(Tracer tracer, List<string> values, List<string> options)
    {
        if (values == null || values.Count == 0) { return; }

        else if (values.Count == 1)
        {
            MapsManager.CurrentMap.Back();
            AddMessage("check point : " + MapsManager.CurrentMap.Index.ToString(), Tracer.MessageLevel.normal, tracer, options);
        }

        // ex) back(0) 0(1)
        else if (values.Count == 2)
        {
            var indexString = values[1];

            if (int.TryParse(indexString, out var index))
            {
                MapsManager.CurrentMap.Back(index);
                AddMessage("check point : " + MapsManager.CurrentMap.Index.ToString(), Tracer.MessageLevel.normal, tracer, options);
            }

            else
            {
                AddMessage(ERROR_NotInteger(indexString), Tracer.MessageLevel.error, tracer, options);
            }
        }

        else
        {
            AddMessage(ERROR_OverValues(0), Tracer.MessageLevel.error, tracer, options);
        }
    }
}
