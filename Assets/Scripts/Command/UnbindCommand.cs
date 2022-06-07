using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnbindCommand : Command
{
    public UnbindCommand()
    {
        commandName = "unbind";
        description = "�쐬�����L�[�o�C���h���폜����@�\��񋟂��܂��D\n" +
            "����̃L�[�o�C���h���w�肷��ɂ́C�L�[�o�C���h�̔ԍ����g�p���܂��D���̂��߁C�����ǃR���\�[����'bind'�Ɠ��͂�" +
            "�폜�������L�[�o�C���h�����ԂɎw�肳��Ă��邩���m�F���Ă�����s���܂��傤�D";
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
            tracer.AddMessage("�l���w�肵�Ă�������", Tracer.MessageLevel.error);
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
                tracer.AddMessage(value + "�𐮐��ɕϊ��ł��܂���D", Tracer.MessageLevel.error);
            }
        }

        tracer.AddMessage("2�ȏ�̒l���w�肷�邱�Ƃ͂ł��܂���D", Tracer.MessageLevel.error);
    }
}
