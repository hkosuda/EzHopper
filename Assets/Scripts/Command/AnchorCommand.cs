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

    public override void CommandMethod(Tracer tracer, List<string> values)
    {
        if (values == null || values.Count == 0) { return; }

        if (values.Count == 1)
        {
            tracer.AddMessage("���݂̍��W ... " + PositionInfo(PM_Main.Myself.transform.position), Tracer.MessageLevel.error);

            if (anchoredParams == null)
            {
                tracer.AddMessage("���W�̋L�^�͂���܂���D", Tracer.MessageLevel.normal);
                return;
            }

            else
            {
                tracer.AddMessage("�L�^���ꂽ���W ... " + PositionInfo(anchoredParams.position), Tracer.MessageLevel.normal);
                return;
            }
        }

        else if (values.Count == 2)
        {
            var action = values[1];

            if (action == "set")
            {
                anchoredParams = new AnchoredParams();
                ChatMessages.SendChat("���W���L�^���܂����D", ChatMessages.Sender.system);
                return;
            }

            else if (action == "back")
            {
                if (anchoredParams == null)
                {
                    tracer.AddMessage("���W���L�^����Ă��܂���D'anchor set'���g�p���č��W���L�^���Ă�������", Tracer.MessageLevel.error);
                    return;
                }

                if (anchoredParams.mapName != MapsManager.CurrentMap.MapName)
                {
                    tracer.AddMessage("�L�^���ꂽ�ۂ̃}�b�v�ƌ��݂̃}�b�v���قȂ邽�߁C�L�^���ꂽ���W�ɖ߂�܂���D", Tracer.MessageLevel.error);
                    return;
                }

                PM_Main.Myself.transform.position = anchoredParams.position;
                PM_Camera.SetEulerAngles(anchoredParams.eulerAngle);
                PM_Main.Rb.velocity = Vector3.zero;
                PM_Jumping.InactivateAutoJump();
            }

            else
            {
                tracer.AddMessage("'set'��������'back'�̂ݒl�Ƃ��Ďw��\�ł��D", Tracer.MessageLevel.error);
                return;
            }

            return;
        }

        else
        {
            tracer.AddMessage("2�ȏ�̒l���w�肷�邱�Ƃ͂ł��܂���D", Tracer.MessageLevel.error);
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
