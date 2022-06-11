using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorCommand : Command
{
    static AnchoredParams anchoredParams;

    enum Values
    {
        set,
        back,
    }

    public AnchorCommand()
    {
        commandName = "anchor";
        description = "�v���C���[�̍��W���L�^���C���̏ꏊ�ɖ߂�@�\��񋟂��܂��D\n" +
            "'anchor " + Values.set.ToString() + "'�ō��W���L�^���C'anchor " + Values.back.ToString() + "'�ł��̍��W�܂Ŗ߂�܂��D\n" +
            "�Ȃ��C�}�b�v���ύX�����ƍ��W�̃f�[�^�͍폜����܂��D";
    }

    public override void OnMapInitialized()
    {
        anchoredParams = null;
    }

    public override List<string> AvailableValues(List<string> values)
    {
        if (values == null || values.Count == 0) { return new List<string>(); }

        if (values.Count < 3)
        {
            return new List<string>() { Values.set.ToString(), Values.back.ToString() };
        }

        return new List<string>();
    }

    public override void CommandMethod(Tracer tracer, List<string> values, List<string> options)
    {
        if (values == null || values.Count == 0) { return; }

        if (values.Count == 1)
        {
            AddMessage("���݂̍��W ... " + PositionInfo(PM_Main.Myself.transform.position), Tracer.MessageLevel.normal, tracer, options);

            if (anchoredParams == null)
            {
                AddMessage("���W�̋L�^�͂���܂���D", Tracer.MessageLevel.normal, tracer, options);
            }

            else
            {
                AddMessage("�L�^���ꂽ���W ... " + PositionInfo(anchoredParams.position), Tracer.MessageLevel.normal, tracer, options);
            }
        }

        else if (values.Count == 2)
        {
            var action = values[1];

            if (action == "set")
            {
                anchoredParams = new AnchoredParams();
                AddMessage("���W���L�^���܂����D", Tracer.MessageLevel.normal, tracer, options);
            }

            else if (action == "back")
            {
                if (anchoredParams == null)
                {
                    AddMessage("���W���L�^����Ă��܂���D'anchor set'���g�p���č��W���L�^���Ă�������", Tracer.MessageLevel.error, tracer, options);
                }

                else if (anchoredParams.mapName != MapsManager.CurrentMap.MapName)
                {
                    AddMessage("�L�^���ꂽ�ۂ̃}�b�v�ƌ��݂̃}�b�v���قȂ邽�߁C�L�^���ꂽ���W�ɖ߂�܂���D", Tracer.MessageLevel.error, tracer, options);
                }

                else
                {
                    PM_Main.ResetPosition(anchoredParams.position, anchoredParams.eulerAngle.y);
                    AddMessage("�L�^���ꂽ���W�Ƀv���C���[���ړ������܂����D", Tracer.MessageLevel.normal, tracer, options);
                }
            }

            else
            {
                AddMessage("'set'��������'back'�̂ݒl�Ƃ��Ďw��\�ł��D", Tracer.MessageLevel.error, tracer, options);
            }
        }

        else
        {
            AddMessage(ERROR_OverValues(2), Tracer.MessageLevel.error, tracer, options);
        }
        
        // - inner function
        static string PositionInfo(Vector3 pos)
        {
            return "X : " + pos.x.ToString("f2") + ", Y : " + pos.y.ToString("2") + ", Z : " + pos.z.ToString("f2");
        }
    }

    class AnchoredParams
    {
        public MapName mapName;
        public Vector3 position;
        public Vector3 eulerAngle;

        public AnchoredParams()
        {
            mapName = MapsManager.CurrentMap.MapName;
            position = PM_Main.Myself.transform.position;
            eulerAngle = PM_Camera.EulerAngles();
        }
    }
}
