using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrevCommand : Command
{
    public PrevCommand()
    {
        commandName = "prev";
        description = "���Ԓn�_�������ݒ肳��Ă���}�b�v�ŁC�ЂƂO�̒��Ԓn�_�Ɉړ�����@�\��񋟂��܂��D";
    }

    public override void CommandMethod(Tracer tracer, List<string> values, List<string> options)
    {
        var prev = MapsManager.CurrentMap.Index;
        MapsManager.CurrentMap.Prev();
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
}
