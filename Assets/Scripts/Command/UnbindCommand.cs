using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnbindCommand : Command
{
    public UnbindCommand()
    {
        commandName = "unbind";
        description = "作成したキーバインドを削除する機能を提供します．\n" +
            "特定のキーバインドを指定するには，キーバインドの番号を使用します．そのため，いちどコンソールで'bind'と入力し" +
            "削除したいキーバインドが何番に指定されているかを確認してから実行しましょう．";
    }

    public override List<string> AvailableValues(List<string> values)
    {
        if (values == null || values.Count == 0) { return new List<string>(); }

        if (values.Count < 3)
        {
            var available = new List<string>();

            for(var n = 0; n < BindCommand.KeyBindingList.Count - 1; n++)
            {
                available.Add(n.ToString());
            }

            return available;
        }

        return new List<string>();
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
                    BindCommand.RemoveKeybind(num, tracer);
                    return;
                }
            }

            else
            {
                tracer.AddMessage(value + "を整数に変換できません．", Tracer.MessageLevel.error);
            }
        }

        tracer.AddMessage("2個以上の値を指定することはできません．", Tracer.MessageLevel.error);
    }
}
