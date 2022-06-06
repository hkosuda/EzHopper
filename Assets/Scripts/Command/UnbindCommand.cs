using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnbindCommand : Command
{
    public UnbindCommand()
    {
        commandName = "unbind";
        description = "いちど作成したキーバインドを削除します";
    }

    public override void CommandMethod(Tracer tracer, List<string> values)
    {
        if (values == null || values.Count == 0) { return; }

        if (values.Count == 1)
        {
            tracer.AddMessage("値を指定してください", Tracer.MessageLevel.error);
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
