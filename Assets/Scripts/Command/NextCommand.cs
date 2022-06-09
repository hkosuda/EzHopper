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

    public override void CommandMethod(Tracer tracer, List<string> values)
    {
        var prev = MapsManager.CurrentMap.Index;
        MapsManager.CurrentMap.Next();
        var current = MapsManager.CurrentMap.Index;

        if (MapsManager.CurrentMap.respawnPositions.Length == 1)
        {
            tracer.AddMessage("���݂̃}�b�v�ɂ̓`�F�b�N�|�C���g��1��������܂���D", Tracer.MessageLevel.warning);
        }

        else
        {
            tracer.AddMessage("check point : " + prev.ToString() + " -> " + current.ToString(), Tracer.MessageLevel.normal);
        }
    }
}
