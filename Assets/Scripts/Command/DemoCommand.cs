using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoCommand : Command
{
    static public readonly List<string> availableValues = new List<string>()
    {
        "athletic_piles",
        "athletic_tiles",
        "athletic_slopes",
        "flyer",
        "horizon_0101",
        "nostalgia_0101",
        "nostalgia_0101_02",
        "nostalgia_0102",
        "nostalgia_0103",
        "nostalgia_0201",
        "nostalgia_0202",
        "nostalgia_0203",
        "nostalgia_0203_shortcut",
        "nostalgia_0301",
        "nostalgia_0302",
        "square_0101_01",
        "square_0102_01",
        "square_0103_01",
        "square_0103_02",
        "square_0103_03",
        "square_0103_04",
        "training",

    };

    public DemoCommand()
    {
        commandName = "demo";
        description = "�f�����Đ�����@�\��񋟂��܂��D\n";

#if UNITY_EDITOR
        foreach(var value in availableValues)
        {
            var asset = Resources.Load<TextAsset>("DemoData/" + value + ".ghost");
            if (asset == null)
            {
                Debug.LogError("Not available : " + value);
            }
        }
#endif
    }

    public override List<string> AvailableValues(List<string> values)
    {
        if (values == null || values.Count == 0) { return new List<string>(); }

        if (values.Count < 3)
        {
            return availableValues;
        }

        return new List<string>();
    }

    public override void CommandMethod(Tracer tracer, List<string> values)
    {
        if (values == null || values.Count == 0) { return; }

        if (values.Count == 1)
        {
            tracer.AddMessage("�Đ�����f�[�^���w�肵�Ă��������D", Tracer.MessageLevel.error);
            return;
        }

        if (values.Count == 2)
        {
            var filename = values[1];

            var asset = Resources.Load<TextAsset>("DemoData/" + filename + ".ghost");

            if (asset != null)
            {
                var mapName = DemoFileUtils.FullText2MapName(asset.text);
                if (mapName == MapName.none) { tracer.AddMessage("�f�[�^�Ɏw�肳�ꂽ�}�b�v��������܂���D", Tracer.MessageLevel.error); return; }

                var dataList = DemoFileUtils.FullText2DataList(asset.text);
                if (dataList == null || dataList.Count == 0)
                {
                    tracer.AddMessage("�f�[�^���X�g�̓ǂݍ��݂Ɏ��s���܂���", Tracer.MessageLevel.error);
                    return;
                }

                if (!MapsManager.MapList.ContainsKey(mapName))
                {
                    tracer.AddMessage("�w�肳�ꂽ�}�b�v�͓ǂݍ��߂܂���D", Tracer.MessageLevel.error);
                    return;
                }

                if (mapName != MapsManager.CurrentMap.MapName)
                {
                    tracer.AddMessage("���݂̃}�b�v�ƈقȂ�}�b�v�̃f���f�[�^�ł��邽�߁C�}�b�v��؂�ւ��Ď��s���܂��D", Tracer.MessageLevel.warning);
                    MapsManager.Begin(mapName);
                }

                DemoManager.BeginDemo(dataList);
                tracer.AddMessage("�f�����N�����܂����D", Tracer.MessageLevel.normal);
                return;
            }

            else
            {
                tracer.AddMessage(filename + "�ɊY������f�[�^��������܂���ł����D", Tracer.MessageLevel.error);
            }
        }

        else
        {
            tracer.AddMessage("2�ȏ�̒l���w�肷�邱�Ƃ͂ł��܂���D", Tracer.MessageLevel.error);
            return;
        }
    }
}
