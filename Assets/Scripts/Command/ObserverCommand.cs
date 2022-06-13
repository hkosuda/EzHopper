using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObserverCommand : Command
{
    static readonly List<string> availableValues = new List<string>()
    {
        "start", "end", "land"
    };

    static public bool Active { get; private set; }

    static GameObject _virtualPlayer;

    static Vector3 originalPosition;
    static Vector3 originalEulerAngle;

    public ObserverCommand()
    {
        commandName = "observer";
        description = "�_���_���[�h���N�����C�v���C���[���}�b�v������R�Ɉړ�����@�\��񋟂��܂��D\n" +
            "'observer start'�Ő_���_���[�h���N�����C'observer end'�Ő_���_���[�h���N�������ꏊ�܂Ŗ߂�܂��D" +
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
            InGameTimer.Updated += UpdateMethod;
        }

        else
        {
            InGameTimer.Updated -= UpdateMethod;
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

        else if (values.Count < 3)
        {
            return availableValues;
        }

        return new List<string>();
    }

    public override void CommandMethod(Tracer tracer, List<string> values, List<string> options)
    {
        if (values == null || values.Count == 0) { return; }

        if (VirtualPlayer.CheckLandingNow)
        {
            AddMessage("���݁C���n�_�𒲍����̂��߃R�}���h�����s�ł��܂���", Tracer.MessageLevel.error, tracer, options);
        }

        else if (values.Count == 1)
        {
            AddMessage(ERROR_AvailableOnly(1, availableValues), Tracer.MessageLevel.error, tracer, options);
        }

        else if (values.Count == 2)
        {
            var value = values[1];

            if (value == "start")
            {
                if (Active)
                {
                    AddMessage("���łɐ_���_���[�h���N�����Ă��܂��D", Tracer.MessageLevel.error, tracer, options);
                }

                else
                {
                    originalPosition = PM_Main.Myself.transform.position;
                    originalEulerAngle = PM_Camera.EulerAngles();

                    Active = true;
                    SetPlayerPhysics(false);

                    AddMessage("�_���_���[�h���N�����܂����D", Tracer.MessageLevel.normal, tracer, options);
                }
            }

            else if (value == "end")
            {
                if (Active)
                {
                    Land(originalPosition, originalEulerAngle);

                    var pos = PM_Main.Myself.transform.position;
                    var posString = "x : " + pos.x.ToString() + ", y: " + pos.y.ToString() + ", z : " + pos.z.ToString();

                    AddMessage("�_���_���[�h���I�����܂����D���݂̍��W�� " + posString + " �ł��D", Tracer.MessageLevel.normal, tracer, options);
                }

                else
                {
                    AddMessage("�_���_���[�h�͋N�����Ă��܂���D", Tracer.MessageLevel.error, tracer, options);
                }
            }

            else
            {
                AddMessage("�_���_���[�h���N������ɂ�'observer start'���C�_���_���[�h���I������ɂ�'observer end'�����s���Ă��������D", Tracer.MessageLevel.error, tracer,  options);
            }
        }

        else
        {
            AddMessage(ERROR_OverValues(2), Tracer.MessageLevel.error, tracer, options);
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
