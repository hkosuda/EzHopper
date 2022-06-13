using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeycheckCommand : Command
{
    static public bool Active { get; private set; } = false;

    public KeycheckCommand()
    {
        commandName = "keycheck";
        description = "���͂����L�[���C�Q�[�����łǂ�ȕ�����Ƃ��Ĉ����邩���m�F����@�\��񋟂��܂��D";
    }

    public override void CommandMethod(Tracer tracer, List<string> values, List<string> options)
    {
        if (values == null || values.Count == 0) { return; }

        if (values.Count == 1)
        {
            Active = true;
            AddMessage("���͑ҋ@��ԂɂȂ�܂����D���ׂ����L�[�������Ă��������D", Tracer.MessageLevel.emphasis, tracer, options);
        }

        else
        {
            AddMessage(ERROR_OverValues(0), Tracer.MessageLevel.error, tracer, options);
        }
    }

    static public void EchoInputKey(string keyString)
    {
        Active = false;
        ConsoleMessage.WriteLog("<color=lime>\t���͂��ꂽ�L�[�̖��́F" + keyString.ToLower() + "</color>");
    }
}
