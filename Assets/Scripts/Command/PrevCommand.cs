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

    public override void CommandMethod(Tracer tracer, List<string> values)
    {
        MapsManager.CurrentMap.Prev();
    }
}
