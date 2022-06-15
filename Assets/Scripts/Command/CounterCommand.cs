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
        description = "�񐔂𐔂���@�\��񋟂��܂��D";
        detail = "'counter set <value>' �ŔC�ӂ̐��ɃJ�E���^�[���Z�b�g���܂��i<value>�ɂ͔C�ӂ̐������w�肵�Ă��������j�D'counter add'�ŁC���݂̃J�E���^�[�̒l��1���������܂��D" +
            "'counter sub' �Ō��݂̃J�E���^�[�̒l��1���������܂��D'counter now' �Ō��݂̃J�E���^�[�̒l���m�F�ł��܂��D\n" +
            "���Ƃ��΁C'invoke add on_enter_checkpoint \"counter add\"' �� 'invoke add on_enter_next_checkpoint \"chat �N���A�܂łɗv�����񐔁F%counter%\"' " +
            "�����炩���ߎ��s���������ŁC'counter set 1' �����s����ƁC�ȍ~�N���A�܂łɉ��x�g���C���������m�F���邱�Ƃ��ł��܂��D";
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
                AddMessage("'set'�̂��ƂɁC�������w�肵�Ă��������D", Tracer.MessageLevel.error, tracer, options);
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
                    AddMessage("�J�E���^�[�̒l��" + Counter.ToString() + "�ɃZ�b�g���܂����D", Tracer.MessageLevel.normal, tracer, options);
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
