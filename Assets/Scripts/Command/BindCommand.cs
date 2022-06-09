using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindCommand : Command
{
    static public List<Binding> KeyBindingList { get; private set; }

    public BindCommand()
    {
        commandName = "bind";
        description = "����̃L�[�ɃR�}���h�����蓖�Ă�@�\��񋟂��܂��D\n" +
            "���Ƃ��΁C'bind c anchor back'�ƃR���\�[���œ��͂���ƁCC�L�[���������ۂ�'begin ez_athletic'�����s�����悤�ɂȂ�܂��D" +
            "����ɂ��C�L�^���ꂽ���W�ɑf�����߂邱�Ƃ��ł���悤�ɂȂ�܂��D\n" +
            "�o�C���h����������ɂ́Cunbind�R�}���h���g�p���܂��D\n" +
            "�l���w�肹���ɁC�����P��'bind'�Ɠ��͂���ƌ��݂̃o�C���h�̐ݒ���݂邱�Ƃ��ł��܂��D" +
            "unbind�R�}���h�Ńo�C���h���폜����܂��Ɋm�F����悤�ɂ��܂��傤�D";

        // initialize method
        KeyBindingList = new List<Binding>();
        Timer.Updated += UpdateMethod;
    }

    public override List<string> AvailableValues(List<string> values)
    {
        if (values == null || values.Count == 0) { return new List<string>(); }

        if (values.Count < 3)
        {
            var available = new List<string>();

            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                available.Add(keyCode.ToString().ToLower());
            }

            available.Add("1");
            available.Add("-1");

            return available;
        }

        return new List<string>();
    }

    public override void CommandMethod(Tracer tracer, List<string> values)
    {
        if (values == null || values.Count == 0) { return; }

        if (values.Count == 1)
        {
            tracer.AddMessage(CurrentBindings(), Tracer.MessageLevel.normal);
            return;
        }

        else if (values.Count == 2)
        {
            tracer.AddMessage("�L�[�̂��ƂɁC�o�C���h����R�}���h���w�肵�Ă��������D", Tracer.MessageLevel.error);
            return;
        }

        else if (values.Count > 2)
        {
            if (KeyBindingList == null) { KeyBindingList = new List<Binding>(); }

            var keyString = values[1];
            var key = GetKey(keyString, tracer);

            if (key == null) { return; }

            var command = GetCommand(values);
            KeyBindingList.Add(new Binding(key.keyCode, key.wheelDelta, command));

            tracer.AddMessage("�o�C���h��ǉ����܂����F" + BindingInfo(KeyBindingList.Last()), Tracer.MessageLevel.normal);
        }

        // - inner function
        static string CurrentBindings()
        {
            if (KeyBindingList == null || KeyBindingList.Count == 0)
            {
                return "���݃o�C���h����Ă���R�}���h�͂���܂���D";
            }

            var message = "���݃o�C���h����Ă���R�}���h�͈ȉ��̒ʂ�ł��D\n";

            for(var n = 0; n < KeyBindingList.Count; n++)
            {
                var binding = KeyBindingList[n];

                message += "\t\t[" + n.ToString() + "] " + binding.key.GetKeyString() + " : " + binding.command + "\n";
            }

            return message.TrimEnd(new char[1] { '\n' });
        }

        // - inner function
        static string GetCommand(List<string> values)
        {
            var command = "";

            for(var n = 2; n < values.Count; n++)
            {
                command += values[n] + " ";
            }

            return command.TrimEnd();
        }

        // - inner function
        static Keyconfig.Key GetKey(string keyString, Tracer tracer)
        {
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                if (keyCode.ToString().ToLower() == keyString)
                {
                    return new Keyconfig.Key(keyCode);
                }
            }

            if (float.TryParse(keyString, out var num))
            {
                if (num > 0.0f)
                {
                    return new Keyconfig.Key(KeyCode.None, 1.0f);
                }

                else if (num < 0.0f)
                {
                    return new Keyconfig.Key(KeyCode.None, -1.0f);
                }

                else
                {
                    tracer.AddMessage("�}�E�X�z�C�[���̏㉺�ɃR�}���h�����蓖�Ă����ꍇ�́C0�łȂ��l���w�肵�Ă��������D", Tracer.MessageLevel.error);
                    return null;
                }
            }

            tracer.AddMessage(keyString + "��L���ȃL�[�ɕϊ��ł��܂���D", Tracer.MessageLevel.error);
            return null;
        }
    }

    // update method
    static void UpdateMethod(object obj, float dt)
    {
        if(KeyBindingList == null) { return; }
        if (!Input.anyKeyDown && Input.mouseScrollDelta.y == 0.0f) { return; }

        foreach(var keybind in KeyBindingList)
        {
            // key
            if (InputSystem.CheckInput(keybind.key, true))
            {
                CommandReceiver.RequestCommand(keybind.command, false);
            }
        }
    }

    static public void RemoveKeybind(int n, Tracer tracer)
    {
        if (KeyBindingList == null || KeyBindingList.Count == 0)
        {
            tracer.AddMessage("���݃o�C���h�͍쐬����Ă��܂���D", Tracer.MessageLevel.error);
            return;
        }

        if (0 <= n && n < KeyBindingList.Count)
        {
            var bind = KeyBindingList[n];
            
            KeyBindingList.RemoveAt(n);
            tracer.AddMessage("�o�C���h���폜���܂����F" + BindingInfo(bind), Tracer.MessageLevel.normal);
        }

        else
        {
            var message = n.ToString() + "�͗L���Ȓl�͈̔͊O�ł��D�L���Ȓl�͈̔͂�0����" + (KeyBindingList.Count - 1).ToString() + "�܂łł��D";
            tracer.AddMessage(message, Tracer.MessageLevel.error);
        }
    }

    static string BindingInfo(BindCommand.Binding binding)
    {
        return "key : " + binding.key.GetKeyString() + ", command : " + binding.command;
    }

    public class Binding
    {
        public Keyconfig.Key key;
        public string command;

        public Binding(KeyCode keyCode, float wheelDelta, string command)
        {
            key = new Keyconfig.Key(keyCode, wheelDelta);
            this.command = command;
        }
    }
}
