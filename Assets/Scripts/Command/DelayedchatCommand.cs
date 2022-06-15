using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedchatCommand : Command
{
    public DelayedchatCommand()
    {
        commandName = "delayedchat";
        description = "�x�������ă`���b�g�𑗐M���܂��D";
        detail = "���b�Z�[�W�𑗐M����ɂ́Cdelayedchat 0.5 1.5 \"hello\"' �̂悤�ɁC1�Ԗڂ̒l�ɒx�������鎞�Ԃ̍ŏ��l�C" +
            "2�Ԗڂ̒l�ɒx�������鎞�Ԃ̍ő�l���w�肵�܂��D3�Ԗڂ̒l�ɑ��M���郁�b�Z�[�W���w�肵�܂��D\n" +
            "���b�Z�[�W�́C�}�b�v���؂�ւ��Ɣj������܂��D";
    }

    // ex) delayedchat(0) 0.8(1) 2.0(2) "noob"(3)
    public override void CommandMethod(Tracer tracer, List<string> values, List<string> options)
    {
        if (values == null || values.Count == 0) { return; }

        else if (values.Count < 3)
        {
            AddMessage("�x�������鎞�Ԃ̍ŏ��l�C�ő�l���w�肵�Ă��������D", Tracer.MessageLevel.error, tracer, options);
        }

        else if (values.Count == 3)
        {
            AddMessage("���M���郁�b�Z�[�W���w�肵�Ă��������D", Tracer.MessageLevel.error, tracer, options);
        }

        else if (values.Count == 4)
        {
            var minString = values[1];
            var maxString = values[2];

            if (!float.TryParse(minString, out var min)) { AddMessage(minString + "�𐔒l�ɕϊ��ł��܂���D", Tracer.MessageLevel.error, tracer, options); return; }
            if (!float.TryParse(maxString, out var max)) { AddMessage(maxString + "�𐔒l�ɕϊ��ł��܂���D", Tracer.MessageLevel.error, tracer, options); return; }

            if (min < 0 || max < 0)
            {
                AddMessage("���̒l���w�肷�邱�Ƃ͂ł��܂���D", Tracer.MessageLevel.error, tracer, options);
                return;
            }

            if (min > max)
            {
                AddMessage("2�Ԗڂ̒l�ɂ́C1�Ԗڂ̒l�ȏ�̒l���w�肵�Ă��������D", Tracer.MessageLevel.error, tracer, options);
                return;
            }

            var group = values[3];
            var message = ChatCommand.UnpackGroup(group);

            ToxicSystem.SendDelayedToxicChat(message, min, max);
        }

        else
        {
            AddMessage(ERROR_OverValues(3), Tracer.MessageLevel.error, tracer, options);
        }
    }
}
