using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleCommand : Command
{
    static public EventHandler<bool> ToggleUpdated { get; set; }
    static public List<ToggleGroup> ToggleGroupList = new List<ToggleGroup>();

    public ToggleCommand()
    {
        commandName = "toggle";
        description = "�ӂ��̃R�}���h���C�g�O���Ŏ��s����@�\��񋟂��܂��D";
        detail = "�g�p���@�Ƃ��ẮC'toggle r \"recorder start\" \"recorder end\"' �̂悤�� 'toggle' �̌�ɃL�[�̖��O�C" +
            "���̂��ƂɃg�O���Ŏ��s����R�[�h���ӂ��w�肵�܂��D\n" +
            "��L�����s���邱�ƂŁCR�L�[���������Ƃ� 'recorder start' �� 'recorder end' �����݂Ɏ��s���邱�Ƃ��ł��܂��D\n" +
            "�g�O���̐ݒ���폜����ɂ́C'toggle remove 0' �̂悤�� 'toggle remove' �̌�ɍ폜�������ݒ�̔ԍ����w�肵�܂��D" +
            "�ԍ�����уg�O���̐ݒ���m�F����ɂ́C'toggle' �����s���Ă��������D";

        SetEvent(1);
    }

    static void SetEvent(int indicator)
    {
        if (indicator > 0)
        {
            InGameTimer.Updated += UpdateMethod;
        }

        else
        {
            InGameTimer.Updated -= UpdateMethod;
        }
    }

    static void UpdateMethod(object obj, float dt)
    {
        if (ToggleGroupList == null) { return; }
        if (!Input.anyKeyDown && Input.mouseScrollDelta.y == 0.0f) { return; }

        foreach (var group in ToggleGroupList)
        {
            // key
            if (InputSystem.CheckInput(group.key, true))
            {
                group.Exec();
            }
        }
    }

    public override List<string> AvailableValues(List<string> values)
    {
        if (values == null || values.Count == 0) { return new List<string>(); }

        if (values.Count < 3)
        {
            var available = new List<string>();

            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                if (keyCode == KeyCode.None) { continue; }
                available.Add(keyCode.ToString().ToLower());
            }

            available.Add("1");
            available.Add("-1");
            available.Add("remove");

            return available;
        }

        else if (values.Count < 4)
        {
            if (values[1] == "remove")
            {
                var list = new List<string>();

                for(var n = 0; n < ToggleGroupList.Count - 1; n++)
                {
                    list.Add(n.ToString());
                }

                list.Add("all");
                return list;
            }

            else
            {
                return new List<string>();
            }
        }

        else
        {
            return new List<string>();
        }
    }

    // ex) toggle(0) t(1) /observer/start/(2) /observer/end/(3)
    // ex) toggle(0) remove(1) 3(2)
    public override void CommandMethod(Tracer tracer, List<string> values, List<string> options)
    {
        if (values == null || values.Count == 0) { return; }

        if (values.Count == 1)
        {
            CurrentBindingMessage(tracer, options);
        }

        else if (values.Count == 2)
        {
            AddMessage(ERROR_InvalidValues(), Tracer.MessageLevel.error, tracer, options);
        }

        else if (values.Count == 3)
        {
            var action = values[1];

            if (action == "remove")
            {
                var indexString = values[2];

                if (indexString == "all")
                {
                    ToggleGroupList = new List<ToggleGroup>();
                    AddMessage("�g�O���̐ݒ�����ׂč폜���܂����D", Tracer.MessageLevel.normal, tracer, options);
                }

                else if (int.TryParse(indexString, out var index))
                {
                    TryRemove(index, tracer, options);
                }

                else
                {
                    AddMessage(ERROR_NotInteger(indexString), Tracer.MessageLevel.error, tracer, options);
                }
            }

            else
            {
                AddMessage(ERROR_InvalidValues(), Tracer.MessageLevel.error, tracer, options);
            }
        }

        else if (values.Count == 4)
        {
            var keyString = values[1];
            var key = Keyconfig.StringToKey(keyString);

            if (key == null)
            {
                AddMessage(ERROR_InvalidKey(keyString), Tracer.MessageLevel.error, tracer, options);
                AddMessage(ERROR_InvalidKeyAlert(), Tracer.MessageLevel.warning, tracer, options);
                return;
            }

            var command1 = CommandReceiver.UnpackGrouping(values[2]);
            var command2 = CommandReceiver.UnpackGrouping(values[3]);

            TryAddToggleGroup(key, command1, command2, tracer, options);
        }

        else
        {
            AddMessage(ERROR_OverValues(3), Tracer.MessageLevel.error, tracer, options);
        }

        ToggleUpdated?.Invoke(null, false);

        // - inner function
        static string ERROR_InvalidValues()
        {
            return "�L�[�̂��ƂɃg�O���Ŏ��s����R�}���h���w�肷�邩�C'remove'�̂��Ƃɍ폜����g�O���ݒ�̃C���f�b�N�X��������'all'���w�肵�Ă��������D";
        }
    }

    static void CurrentBindingMessage(Tracer tracer, List<string> options)
    {
        var info = "";

        foreach(var group in ToggleGroupList)
        {
            info += "\t" + group.key.GetKeyString() + "\t| " + group.command1 + "\t| " + group.command2 + "\n";
        }

        if (info == "")
        {
            AddMessage("���݁C�g�O���ݒ�͑��݂��܂���D", Tracer.MessageLevel.normal, tracer, options);
        }

        else
        {
            info = "���݂̃g�O���ݒ�͈ȉ��̒ʂ�ł��D\n" + info;
            AddMessage(info, Tracer.MessageLevel.normal, tracer, options);
        }
    }

    static void TryRemove(int index, Tracer tracer, List<string> options)
    {
        var indexLim = ToggleGroupList.Count - 1;

        if (index < 0 || index > indexLim)
        {
            AddMessage(ERROR_OutOfRange(index, indexLim), Tracer.MessageLevel.error, tracer, options);
        }

        else
        {
            var group = ToggleGroupList[index];

            ToggleGroupList.RemoveAt(index);
            AddMessage("�g�O���̐ݒ���폜���܂����D", Tracer.MessageLevel.normal, tracer, options);
            AddMessage("�폜�����g�O���ݒ�F" + group.Info(), Tracer.MessageLevel.normal, tracer, options);
        }
    }

    static void TryAddToggleGroup(Keyconfig.Key key, string command1, string command2, Tracer tracer, List<string> options)
    {
        var keyString = key.GetKeyString();

        foreach(var group in ToggleGroupList)
        {
            if (group.key.GetKeyString() == keyString)
            {
                AddMessage("�����L�[�ɁC�����̃g�O���ݒ�����蓖�Ă邱�Ƃ͂ł��܂���D", Tracer.MessageLevel.error, tracer, options);
                return;
            }
        }

        var g = new ToggleGroup(key, command1, command2);

        ToggleGroupList.Add(g);
        AddMessage("�g�O���ݒ���쐬���܂����D", Tracer.MessageLevel.normal, tracer, options);
        AddMessage("�쐬���ꂽ�g�O���ݒ�F" + g.Info(), Tracer.MessageLevel.normal, tracer, options);
    }

    public class ToggleGroup
    {
        public Keyconfig.Key key;

        public string command1;
        public string command2;

        bool toggleSwitch;

        public ToggleGroup(Keyconfig.Key key, string command1, string command2)
        {
            this.key = key;
            this.command1 = command1;
            this.command2 = command2;

            toggleSwitch = true;
        }

        public void Exec()
        {
            if (toggleSwitch)
            {
                CommandReceiver.RequestCommand(command1);
            }

            else
            {
                CommandReceiver.RequestCommand(command2);
            }

            toggleSwitch = !toggleSwitch;
        }

        public string Info()
        {
            return key.GetKeyString() + "\t| " + command1 + "\t| " + command2;
        }
    }
}
