using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCommand : Command
{
    public ClearCommand()
    {
        commandName = "clear";
        description = "�R���\�[���̃��b�Z�[�W�����ׂč폜���܂��D";
        detail = "�R���\�[�����\���ł��镶�����ɂ͐���������܂��D'invoke'�Ɋ��蓖�Ă��R�[�h��'-echo'��I�v�V�����Ȃ��Ŏ��s���Ă���ƁC" +
            "�m��ʊԂɃ��b�Z�[�W�̕����������Ȃ�̐��ɂȂ��Ă��܂��܂��D" +
            "�����������E�܂œ��B����ƃ��b�Z�[�W���\������Ȃ��Ȃ�܂��D�����Ȃ����ꍇ�́Cclear���g�p���ă��b�Z�[�W�����ׂč폜���Ă��������D";
    }

    public override void CommandMethod(Tracer tracer, List<string> values, List<string> options)
    {
        if (values == null || values.Count == 0) { return; }

        if (values.Count == 1)
        {
            ConsoleMessage.ClearLog();
        }

        else
        {
            AddMessage(ERROR_OverValues(0), Tracer.MessageLevel.error, tracer, options);
        }
    }
}
