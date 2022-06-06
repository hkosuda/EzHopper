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

        var asset = Resources.Load<TextAsset>("DemoData/" + filename + ".ghost");
        if (asset == null) { tracer.AddMessage("�f�[�^��������܂���ł����D", Tracer.MessageLevel.error); return; }

        var dataList = DemoFileUtils.FullText2DataList(asset.text);

        if (dataList == null || dataList.Count == 0) 
        {
            tracer.AddMessage("�f�[�^���X�g�̓ǂݍ��݂Ɏ��s���܂���", Tracer.MessageLevel.error); 
            return;
        }

        DemoManager.BeginDemo(dataList);
    }
}
