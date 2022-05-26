using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecorderCommand : Command
{
    static readonly List<string> availableValues = new List<string>()
    {
        "begin", "end"
    };

    public RecorderCommand()
    {
        commandName = "recorder";
        description = "�v���C���[�̌��݈ʒu���L�^���Ă����@�\��񋟂��܂��D";
    }

    public override bool CheckValues(Tracer tracer, List<string> values)
    {
        if (values.Count < 2)
        {
            NoValues(tracer);
            return false;
        }

        var value = values[1];

        if (!availableValues.Contains(value))
        {
            NotAvailable(tracer, value);
            return false;
        }

        return true;

        // - inner function
        static void NoValues(Tracer tracer)
        {
            tracer.AddMessage("'" + availableValues[0] + "' �������� '" + availableValues[1] + "' ��l�Ƃ��Ďw�肵�Ă��������D", Tracer.MessageLevel.error);
        }

        // - inner function
        static void NotAvailable(Tracer tracer, string value)
        {
            tracer.AddMessage("'" + value + "' �͒l�Ƃ��ĕs�K�؂ł��D", Tracer.MessageLevel.error);
            tracer.AddMessage("'" + availableValues[0] + "' �������� '" + availableValues[1] + "' ��l�Ƃ��Ďw�肵�Ă��������D", Tracer.MessageLevel.error);
        }
    }

    public override void CommandMethod(Tracer tracer, List<string> values)
    {
        if (values.Count < 2) { return; }

        var value = values[1];

        if (value == "begin")
        {
            PlayerRecorder.BeginRecording();
            return;
        }

        if (value == "end")
        {
            PlayerRecorder.FinishRecording();
            return;
        }
    }
}
