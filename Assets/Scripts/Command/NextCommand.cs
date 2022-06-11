using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextCommand : Command
{
    public NextCommand()
    {
        commandName = "next";
        description = "���Ԓn�_�������ݒ肳��Ă���}�b�v�ŁC���̒��Ԓn�_�Ɉړ�����@�\��񋟂��܂��D";
    }

    public override void CommandMethod(Tracer tracer, List<string> values, List<string> options)
    {
        if (values == null || values.Count == 0) { return; }

        else if (values.Count == 1)
        {
            var prev = MapsManager.CurrentMap.Index;
            MapsManager.CurrentMap.Next();
            var current = MapsManager.CurrentMap.Index;

            if (MapsManager.CurrentMap.respawnPositions.Length == 1)
            {
                AddMessage("���݂̃}�b�v�ɂ̓`�F�b�N�|�C���g��1��������܂���D", Tracer.MessageLevel.warning, tracer, options);
            }

            else
            {
                AddMessage("check point : " + prev.ToString() + " -> " + current.ToString(), Tracer.MessageLevel.normal, tracer, options);
            }
        }

        else
        {
            AddMessage(ERROR_OverValues(0), Tracer.MessageLevel.error, tracer, options);
        }
    }
}
