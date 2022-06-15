using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterCommand : Command
{
    static readonly List<string> actions = new List<string>()
    {
        "add", "sub", "set", "now"
    };

    static public int Counter { get; private set; }

    public CounterCommand()
    {
        commandName = "counter";
        description = "回数を数える機能を提供します．";
        detail = "'counter set <value>' で任意の数にカウンターをセットします（<value>には任意の整数を指定してください）．'counter add'で，現在のカウンターの値を1増加させます．" +
            "'counter sub' で現在のカウンターの値を1減少させます．'counter now' で現在のカウンターの値を確認できます．\n" +
            "たとえば，'invoke add on_enter_checkpoint \"counter add\"' と 'invoke add on_enter_next_checkpoint \"chat クリアまでに要した回数：%counter%\"' " +
            "をあらかじめ実行したうえで，'counter set 1' を実行すると，以降クリアまでに何度トライしたかを確認することができます．";
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

            else if (action == "now")
            {
                AddMessage(Counter.ToString(), Tracer.MessageLevel.normal, tracer, options);
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
