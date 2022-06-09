using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodCommand : Command
{
    static public bool Active { get; private set; }

    static GameObject _virtualPlayer;

    static Vector3 originalPosition;
    static Vector3 originalEulerAngle;

    public GodCommand()
    {
        commandName = "god";
        description = "�_���_���[�h���N�����C�v���C���[���}�b�v������R�Ɉړ�����@�\��񋟂��܂��D\n" +
            "'god'�Ő_���_���[�h���N�����C'god end'�Ő_���_���[�h���N�������ꏊ�܂Ŗ߂�܂��D" +
            "�܂��C�_���_���[�h���ɃW�����v���͂��s���ƁC�C�ӂ̏ꏊ�ɒ��n�ł��܂��D\n" +
            "�������C���n����ƒ��Ԓn�_�܂Ŗ߂����悤�ȏꏊ�ł́C�����ɒ��Ԓn�_�܂Ŗ߂����̂Œ��ӂ��܂��傤�D\n" +
            "���p���Ȃ��悤�ɂ��܂��傤�D";

        _virtualPlayer = Resources.Load<GameObject>("God/VirtualPlayer");

        SetEvent(1);
    }

    static void SetEvent(int indicator)
    {
        if (indicator > 0)
        {
            Timer.Updated += UpdateMethod;
        }

        else
        {
            Timer.Updated -= UpdateMethod;
        }
    }

    public override void OnMapInitialized()
    {
        Inactivate();
    }

    static void UpdateMethod(object obj, float dt)
    {
        if (!Active) { return; }
        if (VirtualPlayer.CheckLandingNow) { return; }

        if (InputSystem.CheckInput(Keyconfig.KeybindList[Keyconfig.KeyAction.jump], true))
        {
            var hit = SphereCastCheck();

            if (hit.collider == null)
            {
                ChatMessages.SendChat("�����Ȓ��n�n�_�ł��D", ChatMessages.Sender.system);
                return;
            }

            var landingPoint = hit.point + new Vector3(0.0f, PM_Main.centerY + 0.1f, 0.0f);
            var virtualPlayer = GameObject.Instantiate(_virtualPlayer, landingPoint, Quaternion.identity);

            virtualPlayer.GetComponent<VirtualPlayer>().Initialize(landingPoint);
            GameSystem.SetChildOfRoot(virtualPlayer);
        }
    }

    public override List<string> AvailableValues(List<string> values)
    {
        if (values == null || values.Count == 0) { return new List<string>(); }

        if (values.Count < 3)
        {
            return new List<string>() { "end" };
        }

        return new List<string>();
    }

    public override void CommandMethod(Tracer tracer, List<string> values)
    {
        if (values == null || values.Count == 0) { return; }

        if (VirtualPlayer.CheckLandingNow)
        {
            tracer.AddMessage("���݁C���n�_�𒲍����̂��߃R�}���h�����s�ł��܂���", Tracer.MessageLevel.error);
            return;
        }

        if (values.Count == 1)
        {
            if (!Active)
            {
                originalPosition = PM_Main.Myself.transform.position;
                originalEulerAngle = PM_Camera.EulerAngles();

                Active = true;
                SetPlayerPhysics(false);

                tracer.AddMessage("�_���_���[�h���N�����܂����D�W�����v���͂ŔC�ӂ̏ꏊ�ɒ��n�ł��܂��D", Tracer.MessageLevel.normal);

                return;
            }

            else
            {
                tracer.AddMessage("���łɐ_���_���[�h���N�����Ă��܂��D", Tracer.MessageLevel.error);
                return;
            }
        }

        if (values.Count == 2)
        {
            if (Active && values[1] == "end")
            {
                Land(originalPosition, originalEulerAngle);

                var pos = PM_Main.Myself.transform.position;
                var posString = "x : " + pos.x.ToString() + ", y: " + pos.y.ToString() + ", z : " + pos.z.ToString();

                tracer.AddMessage("�_���_���[�h���I�����܂����D���݂̍��W�� " + posString + " �ł��D", Tracer.MessageLevel.normal);
                return;
            }

            if (!Active && values[1] == "end")
            {
                tracer.AddMessage("�_���_���[�h�͋N�����Ă��܂���D", Tracer.MessageLevel.error);
                return;
            }

            else
            {
                tracer.AddMessage("�����Ȓl�ł��D�_���_���[�h���I������ɂ́C'god end'�Ɠ��͂��邩�C�C�ӂ̏ꏊ�ŃW�����v���͂��s�����n�����݂Ă��������D", Tracer.MessageLevel.error);
                return;
            }
        }

        else
        {
            tracer.AddMessage("2�ȏ�̒l���w�肷�邱�Ƃ͂ł��܂���D", Tracer.MessageLevel.error);
        }
    }

    static RaycastHit SphereCastCheck()
    {
        var origin = PM_Main.Myself.transform.position;

        Physics.SphereCast(origin, PM_Main.playerRadius, Vector3.down, out RaycastHit hitinfo);

        return hitinfo;
    }

    static void Inactivate()
    {
        Active = false;
        SetPlayerPhysics(true);
    }

    static public void Land(Vector3 position, Vector3 eulerAngle)
    {
        Active = false;

        PM_Main.ResetPosition(position, eulerAngle.y);
        SetPlayerPhysics(true);
    }

    static void SetPlayerPhysics(bool status)
    {
        PM_Main.Rb.useGravity = status;
        PM_Main.Collider.enabled = status;
    }
}
