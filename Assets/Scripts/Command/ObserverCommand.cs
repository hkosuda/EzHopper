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
        description = "神視点モードを起動し，プレイヤーがマップ上を自由に移動する機能を提供します．\n" +
            "'observer start'で神視点モードを起動し，'observer end'で神視点モードを起動した場所まで戻ります．" +
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
            AddMessage("現在，着地点を調査中のためコマンドを実行できません", Tracer.MessageLevel.error, tracer, options);
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
                    AddMessage("すでに神視点モードが起動しています．", Tracer.MessageLevel.error, tracer, options);
                }

                else
                {
                    originalPosition = PM_Main.Myself.transform.position;
                    originalEulerAngle = PM_Camera.EulerAngles();

                    Active = true;
                    SetPlayerPhysics(false);

                    AddMessage("神視点モードを起動しました．", Tracer.MessageLevel.normal, tracer, options);
                }
            }

            else if (value == "end")
            {
                if (Active)
                {
                    Land(originalPosition, originalEulerAngle);

                    var pos = PM_Main.Myself.transform.position;
                    var posString = "x : " + pos.x.ToString() + ", y: " + pos.y.ToString() + ", z : " + pos.z.ToString();

                    AddMessage("神視点モードを終了しました．現在の座標は " + posString + " です．", Tracer.MessageLevel.normal, tracer, options);
                }

                else
                {
                    AddMessage("神視点モードは起動していません．", Tracer.MessageLevel.error, tracer, options);
                }
            }

            else
            {
                AddMessage("神視点モードを起動するには'observer start'を，神視点モードを終了するには'observer end'を実行してください．", Tracer.MessageLevel.error, tracer,  options);
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
