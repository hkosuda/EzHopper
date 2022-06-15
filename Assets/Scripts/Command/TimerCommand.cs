using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerCommand : Command
{
    static readonly List<string> availables = new List<string>()
    {
        "now", "start", "stop", "restart"
    };

    static public float PastTime { get; private set; }
    static public bool Active { get; private set; }

    public TimerCommand()
    {
        commandName = "timer";
        description = "�Q�[�����ł̌o�ߎ��Ԃ��v������@�\��񋟂��܂��D";
        detail = "'timer start' �Ń^�C�}�[���N�����C'timer stop' �Ń^�C�}�[���~���܂��D'timer now' �͌��݂̌o�ߎ��Ԃ�\������ꍇ�Ɏg�p�ł��܂��D\n" +
            "'timer restart' �̓^�C�}�[�����������邱�ƂȂ��^�C�}�[���N���ł��܂��D";

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

    static void UpdateMethod(object obj, float dt)
    {
        if (Active)
        {
            PastTime += dt;
        }
    }

    public override List<string> AvailableValues(List<string> values)
    {
        if (values == null || values.Count == 0) { return new List<string>(); }

        else if (values.Count < 3)
        {
            return availables;
        }

        else
        {
            return new List<string>();
        }
    }

    public override void CommandMethod(Tracer tracer, List<string> values, List<string> options)
    {
        if (values == null || values.Count == 0) { return; }

        if (values.Count == 1)
        {
            AddMessage("'now'�C'start'�C'stop'��������'restart'���w�肵�Ă��������D", Tracer.MessageLevel.error, tracer, options);
        }

        else if (values.Count == 2)
        {
            var value = values[1];

            if(value == "now")
            {
                var info = TimeString(PastTime);
                AddMessage(info, Tracer.MessageLevel.normal, tracer, options);
            }

            else if (value == "start")
            {
                PastTime = 0.0f;
                Active = true;

                var info = "�^�C�}�[���N�����܂����D";
                AddMessage(info, Tracer.MessageLevel.normal, tracer, options);
            }

            else if (value == "stop")
            {
                Active = false;

                var info = "�^�C�}�[���~���܂����y" + TimeString(PastTime) + "�z";
                AddMessage(info, Tracer.MessageLevel.normal, tracer, options);
            }

            else if (value == "restart")
            {
                var info = "�^�C�}�[���ċN�����܂����y" + TimeString(PastTime) + "�z";
                AddMessage(info, Tracer.MessageLevel.normal, tracer, options);
            }

            else
            {
                AddMessage(ERROR_AvailableOnly(1, availables), Tracer.MessageLevel.error, tracer, options);
            }
        }

        else
        {
            AddMessage(ERROR_OverValues(1), Tracer.MessageLevel.error, tracer, options);
        }
    }

    static public string TimeString(float time, string separator = ":", bool mSec = true)
    {
        var min = Mathf.FloorToInt(time / 60.0f);
        time -= 60.0f * min;

        var sec = (int)time;
        time -= sec;

        if (mSec)
        {
            var msec = (int)(time * 100.0f);
            return PaddingZero(min) + separator + PaddingZero(sec) + separator + PaddingZero(msec);
        }

        else
        {
            return PaddingZero(min) + separator + PaddingZero(sec);
        }
    }

    static public string SecMSec(float time, string separator = ":")
    {
        var sec = (int)time;
        time -= sec;

        var msec = (int)(time * 100.0f);
        return PaddingZero(sec) + separator + PaddingZero(msec);
    }

    static public string DateTimeString()
    {
        var now = System.DateTime.Now;

        var h = PaddingZero(now.Hour);
        var m = PaddingZero(now.Minute);
        var s = PaddingZero(now.Second);

        return h + "h" + m + "m" + s;
    }

    // function
    static public string PaddingZero(int n)
    {
        if (n < 10)
        {
            return "0" + n.ToString();
        }

        else
        {
            return n.ToString();
        }
    }
}
