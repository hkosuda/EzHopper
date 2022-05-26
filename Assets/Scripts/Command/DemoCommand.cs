using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoCommand : Command
{
    public DemoCommand()
    {
        commandName = "demo";
        description = "�f�����X�g���[�V�������s���@�\��񋟂���D";
    }

    public override bool CheckValues(Tracer tracer, List<string> values)
    {
        if (values == null || values.Count < 2) { return false; }

        return true;
    }

    public override void CommandMethod(Tracer tracer, List<string> values)
    {
        if (values.Count < 2) { return; }
        var filename = values[1];

        var dataList = DemoManager.GetDataList(filename);

        Ghost.BeginReplay(dataList, false);
    }
}
