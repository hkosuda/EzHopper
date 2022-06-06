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
        description = "�v���C���[���}�b�v������R�Ɉړ��ł���悤�ɂȂ�܂�";

        _virtualPlayer = Resources.Load<GameObject>("God/VirtualPlayer");

        SetEvent(1);
    }

    static void SetEvent(int indicator)
    {
        if (indicator > 0)
        {
            MapsManager.Initialized += Inactivate;
            Timer.Updated += UpdateMethod;
        }

        else
        {
            MapsManager.Initialized -= Inactivate;
            Timer.Updated -= UpdateMethod;
        }
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


    public override void CommandMethod(Tracer tracer, List<string> values)
    {
        if (values == null || values.Count == 0) { return; }

        if (VirtualPlayer.CheckLandingNow)
        {
            tracer.AddMessage("���݁C���n�_�𒲍����̂��߃R�}���h�����s�ł��܂���", Tracer.MessageLevel.error);
        }

        if (values.Count == 2)
        {
            if (values[0] == "end")
            {
                Land(originalPosition, originalEulerAngle);
            }
        }

        if (!Active)
        {
            originalPosition = PM_Main.Myself.transform.position;
            originalEulerAngle = PM_Camera.EulerAngles();

            PM_Main.Collider.enabled = false;
            PM_Main.Rb.useGravity = false;

            Active = true;
        }
    }

    static RaycastHit SphereCastCheck()
    {
        var origin = PM_Main.Myself.transform.position;

        Physics.SphereCast(origin, PM_Main.playerRadius, Vector3.down, out RaycastHit hitinfo);

        return hitinfo;
    }

    static void Inactivate(object obj, bool mute)
    {
        Active = false;

        PM_Main.Rb.useGravity = true;
        PM_Main.Collider.enabled = true;
    }

    static public void Land(Vector3 position, Vector3 eulerAngle)
    {
        Active = false;

        PM_Main.ResetPosition(position, eulerAngle.y);

        PM_Main.Rb.useGravity = true;
        PM_Main.Collider.enabled = true;
    }
}
