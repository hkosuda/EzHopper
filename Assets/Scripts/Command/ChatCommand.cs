using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatCommand : Command
{
    public ChatCommand()
    {
        commandName = "chat";
        description = "�`���b�g�Ń��b�Z�[�W�𑗐M���܂��D";
        detail = "���Ƃ���'chat \"hello\"�����s����ƁC��ʂ̍�����'hello'���\������܂��D\n" +
            "�V���{���Ƒg�ݍ��킹�邱�ƂŁC����̃C�x���g�����������Ƃ��ɃJ�X�^�����b�Z�[�W��\���ł��܂��D" +
            "���Ƃ��΁C'invoke add on_enter_next_checkpoint \"chat �o�ߎ��ԁF%time%\"' �����s����ƁC" +
            "���̃`�F�b�N�|�C���g�ɓ��B�����Ƃ��Ɍo�ߎ��Ԃ�\���ł��܂��D";
    }

    public override void CommandMethod(Tracer tracer, List<string> values, List<string> options)
    {
        if (values == null || values.Count == 0) { return; }

        if (values.Count == 1)
        {
            AddMessage("�`���b�g�ő��M���郁�b�Z�[�W���w�肵�Ă��������D", Tracer.MessageLevel.error, tracer, options);
        }

        else if (values.Count == 2)
        {
            var group = values[1];
            var message = UnpackGroup(group);

            ChatMessages.SendChat(message, ChatMessages.Sender.system);
        }

        else
        {
            AddMessage(ERROR_OverValues(1), Tracer.MessageLevel.error, tracer, options);
        }
    }

    static public string UnpackGroup(string group)
    {
        var message = CommandReceiver.UnpackGrouping(group);
        message = CommandReceiver.ReplaceSymbol(message);

        return message;
    }
}