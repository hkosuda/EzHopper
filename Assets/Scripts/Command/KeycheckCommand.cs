using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeycheckCommand : Command
{
    static public bool Active { get; private set; } = false;

    public KeycheckCommand()
    {
        commandName = "keycheck";
        description = "���͂����L�[���C�Q�[�����łǂ̂悤�ȕ�����Ƃ��Ĉ����邩���m�F����@�\��񋟂��܂��D";
        detail = "�g�����Ƃ��ẮC'keycheck' �����s�������ƂŔC�ӂ̃L�[�������܂��D�Q�[�������͂��󂯕t����ƁC" +
            "�R���\�[���ɉ������L�[�ɑΉ����镶���񂪕\������܂��D'bind' �Ȃǂ��g�p���������̂̃L�[�̖��̂��킩��Ȃ��Ƃ��Ɏg�p���܂��傤�D\n" +
            "�Ȃ��C�L�[�̖��̂̑�����Unity�i���̃Q�[���̍쐬�Ɏg�p�����Q�[���G���W���̖��́j��KeyCode�����ׂď������ɂȂ��������̂ƂȂ��Ă��܂��D" +
            "���̂��߁Ckeycheck�R�}���h�ɂ��m�F�ȊO�ɂ�Unity�̃X�N���v�g���t�@�����X�����ɗ���������܂���D";
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
