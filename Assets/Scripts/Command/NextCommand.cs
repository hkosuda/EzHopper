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
        MapsManager.CurrentMap.Next();
    }
}
