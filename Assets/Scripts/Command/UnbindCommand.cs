using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnbindCommand : Command
{
    public UnbindCommand()
    {
        commandName = "unbind";
        description = "�����Ǎ쐬�����L�[�o�C���h���폜���܂�";
    }

    public override void CommandMethod(Tracer tracer, List<string> values)
    {
        if (values == null || values.Count == 0) { return; }

        if (values.Count == 1)
        {
            tracer.AddMessage("�l���w�肵�Ă�������", Tracer.MessageLevel.error);
        }

        if (values.Count == 2)
        {
            var value = values[1];

            if (int.TryParse(value, out var num))
            {
                if (num > 0 && num < BindCommand.KeyBindingList.Count)
                {
                    BindCommand.RemoveKeybind(num);
                }
            }
        }

        tracer.AddMessage("", Tracer.MessageLevel.error);
    }
}
