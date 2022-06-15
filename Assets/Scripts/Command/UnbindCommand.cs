using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnbindCommand : Command
{
    public UnbindCommand()
    {
        commandName = "unbind";
        description = "�쐬�����o�C���h���폜����@�\��񋟂��܂��D";
        detail = "�o�C���h���w�肷��ɂ́C'unbind 0' �̂悤�� 'bind' �̂��Ƃɔԍ����w�肵�܂��D" +
            "���̂��߁C���O�ɃR���\�[����'bind'�Ɠ��͂��폜�������o�C���h�����ԂɎw�肳��Ă��邩���m�F���܂��傤�D";
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

            available.Add("all");

            return available;
        }

        return new List<string>();
    }

    public override void CommandMethod(Tracer tracer, List<string> values, List<string> options)
    {
        if (values == null || values.Count == 0) { return; }

        if (values.Count == 1)
        {
            AddMessage("�폜����o�C���h�̃C���f�b�N�X���w�肵�Ă�������", Tracer.MessageLevel.error, tracer, options);
        }

        // unbind(0) 1(1) 4(2) 5(3) ... / unbinid(0) all(1)
        else
        {
            var value = values[1];

            if (values.Count == 2 && value == "all")
            {
                BindCommand.RemoveAll(tracer, options);
                return;
            }

            var indexes = GetIndexes(values, 1, tracer, options);
            if (indexes == null) { return; }

            BindCommand.RemoveKeybind(indexes, tracer, options);
        }
    }


    static public List<int> GetIndexes(List<string> values, int startIndex, Tracer tracer, List<string> options)
    {
        var indexLim = values.Count - 1;
        if (startIndex < 0 || startIndex > indexLim) { return null; }

        var list = new List<int>();

        for (var n = startIndex; n < values.Count; n++)
        {
            var value = values[n];

            if (int.TryParse(value, out var index))
            {
                if (list.Contains(index))
                {
                    AddMessage("�����C���f�b�N�X�i" + index.ToString() + "�j���܂܂�Ă��܂��D", Tracer.MessageLevel.error, tracer, options);
                }

                else
                {
                    list.Add(index);
                }
            }

            else
            {
                AddMessage(ERROR_NotInteger(value), Tracer.MessageLevel.error, tracer, options);
            }
        }

        if (!tracer.NoError) { return null; }

        list.Sort();
        list.Reverse();

        return list;
    }
}
