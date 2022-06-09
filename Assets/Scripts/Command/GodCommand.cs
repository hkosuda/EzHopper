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
        description = "神視点モードを起動し，プレイヤーがマップ上を自由に移動する機能を提供します．\n" +
            "'god'で神視点モードを起動し，'god end'で神視点モードを起動した場所まで戻ります．" +
            "また，神視点モード中にジャンプ入力を行うと，任意の場所に着地できます．\n" +
            "ただし，着地すると中間地点まで戻されるような場所では，すぐに中間地点まで戻されるので注意しましょう．\n" +
            "悪用しないようにしましょう．";

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
                ChatMessages.SendChat("無効な着地地点です．", ChatMessages.Sender.system);
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
            tracer.AddMessage("現在，着地点を調査中のためコマンドを実行できません", Tracer.MessageLevel.error);
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

                tracer.AddMessage("神視点モードを起動しました．ジャンプ入力で任意の場所に着地できます．", Tracer.MessageLevel.normal);

                return;
            }

            else
            {
                tracer.AddMessage("すでに神視点モードが起動しています．", Tracer.MessageLevel.error);
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

                tracer.AddMessage("神視点モードを終了しました．現在の座標は " + posString + " です．", Tracer.MessageLevel.normal);
                return;
            }

            if (!Active && values[1] == "end")
            {
                tracer.AddMessage("神視点モードは起動していません．", Tracer.MessageLevel.error);
                return;
            }

            else
            {
                tracer.AddMessage("無効な値です．神視点モードを終了するには，'god end'と入力するか，任意の場所でジャンプ入力を行い着地を試みてください．", Tracer.MessageLevel.error);
                return;
            }
        }

        else
        {
            tracer.AddMessage("2個以上の値を指定することはできません．", Tracer.MessageLevel.error);
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
