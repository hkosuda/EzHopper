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
        detail = "�f���Ƃ́C���炩���ߗp�ӂ��ꂽ�f�[�^�̂��Ƃł��D��ɂ��̃}�b�v�̍U�����@���m�F����ۂɊ��p���܂��D\n" +
            "'replay'�Ɠ��l�ɁC�f�[�^�̃}�b�v��񂪌��݂̃}�b�v�ƈقȂ�ꍇ�C���s���Ƀ}�b�v���؂�ւ���Ă��܂��̂Œ��ӂ��Ă��������D\n" +
            "�Ȃ��C�f���̍Đ��Ɠ����ɃS�[�X�g���N�����܂��D";

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

    public override void CommandMethod(Tracer tracer, List<string> values, List<string> options)
    {
        if (values == null || values.Count == 0) { return; }

        if (values.Count == 1)
        {
            AddMessage("�Đ�����f�[�^���w�肵�Ă��������D", Tracer.MessageLevel.error, tracer, options);
        }

        else if (values.Count == 2)
        {
            var filename = values[1];
            var asset = Resources.Load<TextAsset>("DemoData/" + filename + ".ghost");

            if (asset != null)
            {
                var mapName = DemoFileUtils.FullText2MapName(asset.text);
                if (mapName == MapName.none){ AddMessage("�f�[�^�Ɏw�肳�ꂽ�}�b�v��������܂���D", Tracer.MessageLevel.error, tracer, options); return; }

                var dataList = DemoFileUtils.FullText2DataList(asset.text);

                if (dataList == null || dataList.Count == 0)
                {
                    AddMessage("�f�[�^���X�g�̓ǂݍ��݂Ɏ��s���܂���", Tracer.MessageLevel.error, tracer, options);
                    return;
                }

                else if (!MapsManager.MapList.ContainsKey(mapName))
                {
                    AddMessage("�w�肳�ꂽ�}�b�v�͓ǂݍ��߂܂���D", Tracer.MessageLevel.error, tracer, options);
                    return;
                }

                else if (mapName != MapsManager.CurrentMap.MapName)
                {
                    MapsManager.Begin(mapName);
                    AddMessage("���݂̃}�b�v�ƈقȂ�}�b�v�̃f���f�[�^�ł��邽�߁C�}�b�v��؂�ւ��Ď��s���܂��D", Tracer.MessageLevel.warning, tracer, options);
                }

                Ghost.BeginReplay(new List<float[]>(dataList), mapName);
                DemoManager.BeginDemo(new List<float[]>(dataList));
                
                AddMessage("�f�����N�����܂����D", Tracer.MessageLevel.normal, tracer, options);
            }

            else
            {
                AddMessage(filename + "�ɊY������f�[�^��������܂���ł����D", Tracer.MessageLevel.error, tracer, options);
            }
        }

        else
        {
            AddMessage(ERROR_OverValues(2), Tracer.MessageLevel.error, tracer, options);
        }
    }
}
