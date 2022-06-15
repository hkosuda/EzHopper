using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverrideCommand : Command
{
    static readonly List<string> available = new List<string>()
    {
        "echo", "flash", "mute", "silent", "none"
    };

    enum OverrideOption
    {
        echo,
        flash,
        mute,
        silent,
        none,
    }

    static OverrideOption overrideOption = OverrideOption.none;

    public OverrideCommand()
    {
        commandName = "override";
        description = "�V�X�e���������I�Ɏ��s����R�[�h���܂߁C���s�����R�[�h�̃I�v�V���������ׂď���������@�\��񋟂��܂��D";
        detail = "���b�Z�[�W���ꊇ�� '-mute' �ɂ���ꍇ��C'bind' �ȂǂŎ������s�Ɏw�肵�Ă���R�[�h���������@�\���Ȃ��ۂɁC�m�F���邽�߂̋@�\�Ƃ��Ďg�p���邱�Ƃ��ł��܂��D" +
            "���Ƃ��΁C'override flash' �����s���邱�ƂŃI�v�V������ '-mute' ���w�肵�Ă���R�[�h�� '-flash' �Ŏ��s����邽�߁C" +
            "�������@�\���Ă��Ȃ��R�}���h�̃��b�Z�[�W�����Ă��̌����𒲂ׂ邱�Ƃ��ł���悤�ɂȂ�܂��D\n" +
            "���p�ł���I�[�o�[���C�h�͈ȉ��̒ʂ�ł��D\n" +
            "echo  : ���ׂẴR�[�h�� '-echo' ���Ŏ��s���܂��D�I�v�V������ '-echo' ��t���邱�ƂŁC�R���\�[���ƃ`���b�g�̗����Ƀ��b�Z�[�W���\������܂��D\n" +
            "flash : ���ׂẴR�[�h�� '-flash' ���Ŏ��s���܂��D�I�v�V������ '-flash' ��t���邱�ƂŁC�`���b�g�ɂ̂݃��b�Z�[�W���\������܂��D\n" +
            "mute  : ���ׂẴR�[�h�� '-mute' ���Ŏ��s���܂��D�I�v�V������ '-mute' ��t���邱�ƂŁC�R���\�[���C�`���b�g�̂ǂ���ɂ����b�Z�[�W���\������܂���D\n" +
            "silent: ���ׂẴR�[�h�̃��b�Z�[�W���R���\�[���ɂ̂ݕ\�����܂��D\n" +
            "none  : �I�[�o�[���C�h�̗��p���~���܂��D";
    }

    public override List<string> AvailableValues(List<string> values)
    {
        if (values == null || values.Count == 0) { return new List<string>(); }

        else if (values.Count < 3)
        {
            return new List<string>() { "echo", "flash", "mute", "silent", "none" };
        }

        else
        {
            return new List<string>();
        }
    }

    public override void CommandMethod(Tracer tracer, List<string> values, List<string> options)
    {
        if (values == null || values.Count == 0) { return; }

        else if (values.Count == 1)
        {
            AddMessage("�I�v�V�������w�肵�Ă��������D", Tracer.MessageLevel.error, tracer, options);
        }

        else if (values.Count == 2)
        {
            var value = values[1];

            if (value == "echo")
            {
                overrideOption = OverrideOption.echo;
                _AddMessage(OverrideOption.echo, tracer, options);
            }

            else if (value == "flash")
            {
                overrideOption = OverrideOption.flash;
                _AddMessage(OverrideOption.flash, tracer, options);
            }

            else if (value == "mute")
            {
                overrideOption = OverrideOption.mute;
                _AddMessage(OverrideOption.mute, tracer, options);
            }

            else if (value == "silent")
            {
                overrideOption = OverrideOption.silent;
                _AddMessage(OverrideOption.silent, tracer, options);
            }

            else if (value == "none")
            {
                overrideOption = OverrideOption.none;
                _AddMessage(OverrideOption.none, tracer, options);
            }

            else
            {
                AddMessage(ERROR_AvailableOnly(1, available), Tracer.MessageLevel.error, tracer, options);
            }
        }

        else
        {
            AddMessage(ERROR_OverValues(1), Tracer.MessageLevel.error, tracer, options);
        }

        // - inner function
        static void _AddMessage(OverrideOption option, Tracer tracer, List<string> options)
        {
            if (option == OverrideOption.none)
            {
                AddMessage("�I�[�o�[���C�h���������܂����D", Tracer.MessageLevel.normal, tracer, options);
            }

            else
            {
                AddMessage("�I�[�o�[���C�h�����������܂����D�R�}���h�͂��ׂ�" + option.ToString() + "�Ŏ��s����܂��D", Tracer.MessageLevel.normal, tracer, options);
            }
        }
    }

    static public string OverrideSentence(string sentence)
    {
        if (overrideOption == OverrideOption.none)
        {
            return sentence;
        }

        if (overrideOption == OverrideOption.echo)
        {
            return sentence + " -e";
        }

        if (overrideOption == OverrideOption.flash)
        {
            return sentence + " -f";
        }

        if (overrideOption == OverrideOption.mute)
        {
            return sentence + " -m";
        }

        // silent
        else
        {
            var values = CommandReceiver.GetValues(sentence);
            var overrided = "";

            if (values == null) { return overrided; }

            foreach(var value in values)
            {
                overrided += value + " ";
            }

            return overrided;
        }
    }
}
