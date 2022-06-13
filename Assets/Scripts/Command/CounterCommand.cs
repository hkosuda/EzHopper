using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterCommand : Command
{
    static readonly List<string> actions = new List<string>()
    {
        "add", "sub", "set"
    };

    static public int Counter { get; private set; }

    public CounterCommand()
    {
        commandName = "counter";

    }

    public override List<string> AvailableValues(List<string> values)
    {
        if (values == null || values.Count == 0) { return new List<string>(); }

        if (values.Count < 3)
        {
            return actions;
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
            AddMessage(ERROR_NeedValue(1), Tracer.MessageLevel.error, tracer, options);
        }

        else if (values.Count == 2)
        {
            var action = values[1];

            if (action == "add")
            {
                Counter++;
                AddMessage(Counter.ToString(), Tracer.MessageLevel.normal, tracer, options);
            }

            else if (action == "sub")
            {
                Counter--;
                AddMessage(Counter.ToString(), Tracer.MessageLevel.normal, tracer, options);
            }

            else if (action == "set")
            {
                AddMessage("'set'のあとに，整数を指定してください．", Tracer.MessageLevel.error, tracer, options);
            }

            else
            {
                AddMessage(ERROR_AvailableOnly(1, actions), Tracer.MessageLevel.error, tracer, options);
            }
        }

        else if (values.Count == 3)
        {
            var action = values[1];

            if (action == "set")
            {
                var numString = values[2];

                if (int.TryParse(numString, out var num))
                {
                    Counter = num;
                    AddMessage("カウンターの値を" + Counter.ToString() + "にセットしました．", Tracer.MessageLevel.normal, tracer, options);
                }

                else
                {
                    AddMessage(ERROR_NotInteger(numString), Tracer.MessageLevel.error, tracer, options);
                }
            }

            else
            {
                AddMessage(ERROR_AvailableOnly(1, actions), Tracer.MessageLevel.error, tracer, options);
            }
        }

        else
        {
            AddMessage(ERROR_OverValues(2), Tracer.MessageLevel.error, tracer, options);
        }
    }
}
