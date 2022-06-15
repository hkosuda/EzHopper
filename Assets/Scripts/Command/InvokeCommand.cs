using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvokeCommand : Command
{
    static readonly List<string> availables = new List<string>()
    {
        "add", "insert", "replace", "swap", "remove", "inactivate", "activate", "remove_all"
    };

    public enum GameEvent
    {
        none,

        on_course_out,
        on_enter_next_checkpoint,
        on_enter_checkpoint,
        on_exit_checkpoint,
        on_enter_start,
        on_exit_start,
        on_enter_goal,
        on_map_changed,
    }

    static public Dictionary<GameEvent, List<SwitchCommand>> BindingListList { get; private set; } = new Dictionary<GameEvent, List<SwitchCommand>>();

    public InvokeCommand()
    {
        commandName = "invoke";
        description = "�Q�[�����̓���̃C�x���g�����������Ƃ��ɃR�}���h���Ăяo���@�\��񋟂��܂��D";
        detail = "�V���Ɏ������s����R�[�h��ǉ�����ꍇ��'add'�C�w�肵���ʒu�ɃR�[�h��}������ɂ�'insert'�C" +
            "�w�肵���ԍ��̃R�[�h������������ɂ�'replace'�C�ԍ������ւ���ɂ�'swap'�C�폜����ɂ�'remove'�C" +
            "�w�肵���ԍ��̃R�[�h�̎������s���~����ɂ�'inactivate'�C��~����������ɂ�'activate' ��1�Ԗڂ̒l�Ɏw�肵�܂��D\n" +
            "'add' ���g�p�����ǉ��̕��@�Ƃ��ẮC'invoke add on_course_out \"back 0\"' �̂悤�ɁC'add' �̂��ƂɃC�x���g�̖��̂��w�肵�C" +
            "���̎��Ɏ������s����R�[�h���d���p���ň͂�Ŏw�肵�܂��D\n" +
            "'insert' ���g�p�����R�[�h�̑}�����@�Ƃ��ẮC'invoke insert on_enter_checkpoint 0 \"recorder end\"' �̂悤�� 'insert' �̂��ƂɃC�x���g���C" +
            "���̎��ɑ}������ʒu�������ԍ����w�肵�܂��D�ԍ����m�F����ɂ́C'invoke' �����s���Ĉꗗ���Q�Ƃ��Ă��������D\n" +
            "'replace' ���g�p�����R�[�h�̏����������@�́C'invoke replace on_course_out 0 \"back 0\"' �̂悤�ɁC'replace' �̂��ƂɃC�x���g���C" +
            "���̎��ɏ����������s���R�[�h�̔ԍ����w�肵�܂��D\n" +
            "'swap' ���g�p�����R�[�h�̓���ւ����@�Ƃ��ẮC'invoke swap on_enter_checkpoint 0 1' �̂悤�� 'swap' �̌�ɓ���ւ���ԍ����ӂ��w�肵�܂��D\n" +
            "'remove' ���g�p�����R�[�h�̍폜�̕��@�Ƃ��ẮC'invoke remove on_enter_checkpoint 0' �̂悤�ɁC'remove' �̌�ɃC�x���g���C" +
            "���̎��ɍ폜����R�[�h�̔ԍ����w�肵�܂��D\n" +
            "'inactivate' ���g�p�����R�[�h�̎��s�̒�~���@�Ƃ��ẮC'invoke inactivate on_course_out 0' �̂悤�ɁC'inactivate' �̂��ƂɃC�x���g���C" +
            "���̎��ɔԍ����w�肵�܂��D\n" +
            "'activate' ���g�p�����R�[�h�̒�~�̉������@�Ƃ��ẮC'invoke activate on_course_out 0' �̂悤�ɁC'activate' �̂��ƂɃC�x���g���C" +
            "���̎��ɔԍ����w�肵�܂��D\n" +
            "'remove_all' ���g�p�����R�[�h�̈ꊇ�폜�̕��@�Ƃ��ẮC'invoke remove_all on_enter_checkpoint' �̂悤�� 'remove_all' �̂��ƂɃC�x���g�����w�肵�܂��D" +
            "���邢�́C'remove_all' �̂��Ƃ� 'all' ���w�肷�邱�ƂŁCinovoke �̐ݒ�����ׂč폜�ł��܂��D\n" +
            "�Ȃ��C'on_map_changed' �� 'begin' �R�}���h�̑g�ݍ��킹�͐ݒ�ł��܂���D";

        InitializeList();
        SetEvent(1);
    }

    static void InitializeList()
    {
        BindingListList = new Dictionary<GameEvent, List<SwitchCommand>>();

        foreach (GameEvent igniter in Enum.GetValues(typeof(GameEvent)))
        {
            if (igniter == GameEvent.none) { continue; }

            BindingListList.Add(igniter, new List<SwitchCommand>());
        }
    }

    static void SetEvent(int indicator)
    {
        if (indicator > 0)
        {
            InvalidArea.CourseOut += IgniteCourseOut;
            CheckPoint.EnterAnotherCheckpoint += IgniteEnterAnotherCheckpoint;
            CheckPoint.EnterCheckpoint += IgniteEnterCheckpoint;
            CheckPoint.ExitCheckpoint += IgniteExitCheckpoint;
            CheckPoint.EnterStart += IgniteEnterStart;
            CheckPoint.ExitStart += IgniteExitStart;
            CheckPoint.EnterGoal += IgniteEnterGoal;
            MapsManager.Initialized += IgniteMapChanged;
        }

        else
        {
            InvalidArea.CourseOut -= IgniteCourseOut;
            CheckPoint.EnterAnotherCheckpoint -= IgniteEnterAnotherCheckpoint;
            CheckPoint.EnterCheckpoint -= IgniteEnterCheckpoint;
            CheckPoint.ExitCheckpoint -= IgniteExitCheckpoint;
            CheckPoint.EnterStart -= IgniteEnterStart;
            CheckPoint.ExitStart -= IgniteExitStart;
            CheckPoint.EnterGoal -= IgniteEnterGoal;
            MapsManager.Initialized -= IgniteMapChanged;
        }
    }

    static void IgniteCourseOut(object obj, Vector3 pos)
    {
        Ignite(GameEvent.on_course_out);
    }

    static void IgniteEnterAnotherCheckpoint(object obj, int index)
    {
        Ignite(GameEvent.on_enter_next_checkpoint);
    }

    static void IgniteEnterCheckpoint(object obj, Vector3 pos)
    {
        Ignite(GameEvent.on_enter_checkpoint);
    }

    static void IgniteExitCheckpoint(object obj, Vector3 pos)
    {
        Ignite(GameEvent.on_exit_checkpoint);
    }

    static void IgniteEnterStart(object obj, Vector3 pos)
    {
        Ignite(GameEvent.on_enter_start);
    }

    static void IgniteExitStart(object obj, Vector3 pos)
    {
        Ignite(GameEvent.on_exit_start);
    }

    static void IgniteEnterGoal(object obj, Vector3 pos)
    {
        Ignite(GameEvent.on_enter_goal);
    }

    static void IgniteMapChanged(object obj, bool mute)
    {
        Ignite(GameEvent.on_map_changed);
    }

    static void Ignite(GameEvent igniter)
    {
        var commandList = BindingListList[igniter];
        if (commandList == null || commandList.Count == 0) { return; }

        foreach (var command in commandList)
        {
            if (!command.active) { continue; }
            if (igniter == GameEvent.on_map_changed && command.command.StartsWith("begin")) { continue; }

            CommandReceiver.RequestCommand(command.command);
        }
    }

    public override List<string> AvailableValues(List<string> values)
    {
        if (values == null || values.Count == 0) { return new List<string>(); }

        else if (values.Count < 3)
        {
            return availables;
        }

        else if (values.Count < 4)
        {
            var available = new List<string>();

            foreach(GameEvent igniter in Enum.GetValues(typeof(GameEvent)))
            {
                if (igniter == GameEvent.none) { continue; }
                available.Add(igniter.ToString());
            }

            return available;
        }

        else
        {
            return new List<string>();
        }
    }

    public override void CommandMethod(Tracer tracer, List<string> values, List<string> options)
    {
        if (values == null || values.Count == 0) { return; }

        if (values.Count == 1)
        {
            CurrentBinding(tracer, options);
            return;
        }

        var action = values[1];

        // ex) ignite(0) add(1) on_course_out(2) /timer/stop/(3)
        if (action == "add")
        {
            if (values.Count == 2)
            {
                AddMessage(ERROR_TriggerIsNeccesary(), Tracer.MessageLevel.error, tracer, options);
            }

            else if (values.Count == 3)
            {
                AddMessage("���s����R�}���h���w�肵�Ă��������D", Tracer.MessageLevel.error, tracer, options);
            }

            else if (values.Count == 4)
            {
                var igniterString = values[2];
                var igniter = GetIgniter(igniterString);

                if (igniter == GameEvent.none)
                {
                    AddMessage(ERROR_UnknownIgniter(igniterString), Tracer.MessageLevel.error, tracer, options);
                    return;
                }

                var group = values[3];
                var command = CommandReceiver.UnpackGrouping(group);

                var commandValues = CommandReceiver.GetValues(command);

                if (commandValues == null || commandValues.Count == 0)
                {
                    AddMessage(ERROR_VoidCode(), Tracer.MessageLevel.error, tracer, options);
                    return;
                }

                if (CheckDuplication(command, igniter))
                {
                    AddMessage(ERROR_Duplication(), Tracer.MessageLevel.error, tracer, options);
                    AddMessage(ERROR_DuplicationAlert(), Tracer.MessageLevel.warning, tracer, options);
                }

                else
                {
                    if (igniter == GameEvent.on_map_changed && commandValues[0] == "begin")
                    {
                        AddMessage(ERROR_BeginOnMapChanged(), Tracer.MessageLevel.error, tracer, options);
                    }

                    else
                    {
                        BindingListList[igniter].Add(new SwitchCommand(command));
                        AddMessage(igniter.ToString() + "�ɃR�}���h��ǉ����܂����F" + command, Tracer.MessageLevel.normal, tracer, options);
                    }
                }
            }

            else
            {
                AddMessage(ERROR_OverValues(4), Tracer.MessageLevel.error, tracer, options);
            }
        }

        // ex) ignite(0) insert(1) on_course_out(2) 2(3) /timer/stop/(4)
        else if (action == "insert")
        {
            if (values.Count == 2)
            {
                AddMessage(ERROR_TriggerIsNeccesary(), Tracer.MessageLevel.error, tracer, options);
            }

            else if (values.Count == 3)
            {
                AddMessage("�}������ʒu�������C���f�b�N�X���w�肵�Ă��������D", Tracer.MessageLevel.error, tracer, options);
            }

            else if (values.Count == 4)
            {
                AddMessage("�}������R�}���h���w�肵�Ă��������D", Tracer.MessageLevel.error, tracer, options);
            }

            else if (values.Count == 5)
            {
                var igniterString = values[2];
                var indexString = values[3];

                var igniter = GetIgniter(igniterString);

                if (igniter == GameEvent.none)
                {
                    AddMessage(ERROR_UnknownIgniter(igniterString), Tracer.MessageLevel.error, tracer, options);
                }

                else if (int.TryParse(indexString, out var index))
                {
                    var command = CommandReceiver.UnpackGrouping(values[4]);
                    var commandValues = CommandReceiver.GetValues(command);

                    if (commandValues == null || commandValues.Count == 0)
                    {
                        AddMessage(ERROR_VoidCode(), Tracer.MessageLevel.error, tracer, options);
                        return;
                    }

                    if (CheckDuplication(command, igniter))
                    {
                        AddMessage(ERROR_Duplication(), Tracer.MessageLevel.error, tracer, options);
                        AddMessage(ERROR_DuplicationAlert(), Tracer.MessageLevel.warning, tracer, options);
                    }

                    else
                    {
                        TryInsert(index, igniter, command, tracer, options);
                    }
                }

                else
                {
                    AddMessage(ERROR_NotInteger(indexString), Tracer.MessageLevel.error, tracer, options);
                }
            }
        }

        // ex) ignite(0) replace(1) on_course_out(2) 1(3) /recorder/start/(4)
        else if (action == "replace")
        {
            if (values.Count == 2)
            {
                AddMessage(ERROR_TriggerIsNeccesary(), Tracer.MessageLevel.error, tracer, options);
            }

            else if (values.Count == 3)
            {
                AddMessage("�u���ΏۂƂȂ�C���f�b�N�X���w�肵�Ă��������D", Tracer.MessageLevel.error, tracer, options);
            }

            else if (values.Count == 4)
            {
                AddMessage("�u������R�}���h����͂��Ă��������D", Tracer.MessageLevel.error, tracer, options);
            }

            else if (values.Count == 5)
            {
                var igniterString = values[2];
                var indexString = values[3];

                var igniter = GetIgniter(igniterString);

                if(igniter == GameEvent.none)
                {
                    AddMessage(ERROR_UnknownIgniter(igniterString), Tracer.MessageLevel.error, tracer, options);
                }

                else if (int.TryParse(indexString, out var index))
                {
                    var command = CommandReceiver.UnpackGrouping(values[4]);
                    var commandValues = CommandReceiver.GetValues(command);

                    if (commandValues == null || commandValues.Count == 0)
                    {
                        AddMessage(ERROR_VoidCode(), Tracer.MessageLevel.error, tracer, options);
                        return;
                    }

                    if (CheckDuplication(command, igniter, index))
                    {
                        AddMessage(ERROR_Duplication(), Tracer.MessageLevel.error, tracer, options);
                        AddMessage(ERROR_DuplicationAlert(), Tracer.MessageLevel.warning, tracer, options);
                    }

                    else
                    {
                        TryReplace(index, igniter, command, tracer, options);
                    }
                }

                else
                {
                    AddMessage(indexString + "�𐮐��ɕϊ��ł��܂���D", Tracer.MessageLevel.error, tracer, options);
                }
            }

            else
            {
                AddMessage(ERROR_OverValues(4), Tracer.MessageLevel.error, tracer, options);
            }
        }

        // ex) ignite(0) swap(1) on_course_out(2) 0(3) 1(4)
        else if (action == "swap")
        {
            if (values.Count == 2)
            {
                AddMessage(ERROR_TriggerIsNeccesary(), Tracer.MessageLevel.error, tracer, options);
            }

            else if (values.Count == 3)
            {
                AddMessage("����ւ����s��2�̃C���f�b�N�X���w�肵�Ă��������D", Tracer.MessageLevel.error, tracer, options);
            }

            else if (values.Count == 4)
            {
                AddMessage("����ւ����s��2�̃C���f�b�N�X���w�肵�Ă��������D", Tracer.MessageLevel.error, tracer, options);
            }

            else if (values.Count == 5)
            {
                var igniterString = values[2];
                var igniter = GetIgniter(igniterString);

                if (igniter == GameEvent.none)
                {
                    AddMessage(ERROR_UnknownIgniter(igniterString), Tracer.MessageLevel.error, tracer, options);
                    return;
                }

                var v1String = values[3];
                var v2String = values[4];

                if (!int.TryParse(v1String, out var v1))
                {
                    AddMessage(ERROR_NotInteger(v1String), Tracer.MessageLevel.error, tracer, options);
                    return;
                }

                if (!int.TryParse(v2String, out var v2))
                {
                    AddMessage(ERROR_NotInteger(v2String), Tracer.MessageLevel.error, tracer, options);
                    return;
                }

                TrySwap(v1, v2, igniter, tracer, options);
            }

            else
            {
                AddMessage(ERROR_OverValues(4), Tracer.MessageLevel.error, tracer, options);
            }
        }

        // ex) invoke(0) suspend(1) on_course_out(2) 0(3) 2(4) ...
        else if (action == "inactivate")
        {
            if (values.Count == 2)
            {
                AddMessage(ERROR_TriggerIsNeccesary(), Tracer.MessageLevel.error, tracer, options);
            }

            else if (values.Count == 3)
            {
                AddMessage("��~����R�}���h�̃C���f�b�N�X���w�肵�Ă��������D", Tracer.MessageLevel.normal, tracer, options);
            }

            else
            {
                var igniterString = values[2];
                var igniter = GetIgniter(igniterString);

                if (igniter == GameEvent.none)
                {
                    AddMessage(ERROR_UnknownIgniter(igniterString), Tracer.MessageLevel.error, tracer, options);
                    return;
                }

                var indexes = UnbindCommand.GetIndexes(values, 3, tracer, options);
                if (indexes == null) { return; }

                TryChangeActiveStatus(false, indexes, igniter, tracer, options);
            }
        }

        // ex) invoke(0) suspend(1) on_course_out(2) 0(3) 2(4) ...
        else if (action == "activate")
        {
            if (values.Count == 2)
            {
                AddMessage(ERROR_TriggerIsNeccesary(), Tracer.MessageLevel.error, tracer, options);
            }

            else if (values.Count == 3)
            {
                AddMessage("�A�N�e�B�u�ɂ���R�}���h�̃C���f�b�N�X���w�肵�Ă��������D", Tracer.MessageLevel.normal, tracer, options);
            }

            else
            {
                var igniterString = values[2];
                var igniter = GetIgniter(igniterString);

                if (igniter == GameEvent.none)
                {
                    AddMessage(ERROR_UnknownIgniter(igniterString), Tracer.MessageLevel.error, tracer, options);
                    return;
                }

                var indexes = UnbindCommand.GetIndexes(values, 3, tracer, options);
                if (indexes == null) { return; }

                TryChangeActiveStatus(true, indexes, igniter, tracer, options);
            }
        }

        // ex) ignite(0) remove(1) on_course_out(2) 0(3) 1(4) 4(5) 8(6) ... 
        else if (action == "remove")
        {
            if (values.Count == 2)
            {
                AddMessage(ERROR_TriggerIsNeccesary(), Tracer.MessageLevel.error, tracer, options);
            }

            else if (values.Count == 3)
            {
                AddMessage("�폜����R�}���h�̃C���f�b�N�X���w�肵�Ă��������D", Tracer.MessageLevel.error, tracer, options);
            }

            else
            {
                var igniterString = values[2];
                var igniter = GetIgniter(igniterString);

                if (igniter == GameEvent.none)
                {
                    AddMessage(ERROR_UnknownIgniter(igniterString), Tracer.MessageLevel.error, tracer, options);
                    return;
                }

                var indexes = UnbindCommand.GetIndexes(values, 3, tracer, options);
                if (indexes == null) { return; }

                TryRemove(indexes, igniter, tracer, options);
            }
        }

        // ex) ignite(0)
        else if (action == "remove_all")
        {
            if (values.Count == 2)
            {
                AddMessage(ERROR_TriggerIsNeccesary(), Tracer.MessageLevel.error, tracer, options);
            }

            else
            {
                var igniterString = values[2];

                if (igniterString == "all")
                {
                    InitializeList();
                    AddMessage("���ׂẴR�}���h���폜���܂����D", Tracer.MessageLevel.normal, tracer, options);
                    return;
                }


                var igniter = GetIgniter(igniterString);

                if (igniter == GameEvent.none)
                {
                    AddMessage(ERROR_UnknownIgniter(igniterString), Tracer.MessageLevel.error, tracer, options);
                    return;
                }

                BindingListList[igniter] = new List<SwitchCommand>();
                AddMessage(igniter.ToString() + "�Ɍ��т���ꂽ�R�}���h�����ׂč폜���܂����D", Tracer.MessageLevel.normal, tracer, options);
            }
        }

        else
        {
            AddMessage(ERROR_AvailableOnly(1, availables), Tracer.MessageLevel.error, tracer, options);
        }

        // - inner function
        static string ERROR_UnknownIgniter(string igniterString)
        {
            return igniterString + "���g���K�[�ƂȂ�C�x���g���ɕϊ��ł��܂���D";
        }

        // - inner function
        static string ERROR_TriggerIsNeccesary()
        {
            return "�g���K�[�ƂȂ�C�x���g�����w�肵�Ă��������D";
        }

        // - inner function
        static string ERROR_Duplication()
        {
            return "�d������R�}���h�����邽�߁C�����Ɏ��s���܂����D";
        }

        // - inner function
        static string ERROR_DuplicationAlert()
        {
            return "�� �d���̔���́C�I�v�V�����̗L���𖳎����čs���܂��D";
        }

        // - inner function
        static string ERROR_VoidCode()
        {
            return "�R�[�h����ł��D";
        }
    }

    // - inner function
    static string ERROR_BeginOnMapChanged()
    {
        return "'on_map_changed' �� 'begin' �R�}���h�̑g�ݍ��킹�͖����ł��D";
    }

    static GameEvent GetIgniter(string name)
    {
        foreach(GameEvent igniter in Enum.GetValues(typeof(GameEvent)))
        {
            if (igniter.ToString().ToLower() == name.ToLower())
            {
                return igniter;
            }
        }

        return GameEvent.none;
    }

    static void TryReplace(int index, GameEvent igniter, string command, Tracer tracer, List<string> options)
    {
        var list = BindingListList[igniter];

        if (index < 0 || index > list.Count - 1)
        {
            AddMessage(ERROR_OutOfRange(index, list.Count - 1), Tracer.MessageLevel.error, tracer, options);
            return;
        }

        var commandValues = CommandReceiver.GetValues(command);

        if (igniter == GameEvent.on_map_changed && commandValues[0] == "begin")
        {
            AddMessage(ERROR_BeginOnMapChanged(), Tracer.MessageLevel.error, tracer, options);
        }

        else
        {
            list[index].command = command;
            AddMessage(index.ToString() + "�Ԗڂ̃R�}���h��" + command + "�ɒu�����܂����D", Tracer.MessageLevel.normal, tracer, options);
        }
    }

    static void TryInsert(int index, GameEvent igniter, string command, Tracer tracer, List<string> options)
    {
        var list = BindingListList[igniter];

        if (index < 0 || index > list.Count - 1) 
        {
            AddMessage(ERROR_OutOfRange(index, list.Count - 1), Tracer.MessageLevel.error, tracer, options);
            return;
        }

        var commandValues = CommandReceiver.GetValues(command);

        if (igniter == GameEvent.on_map_changed && commandValues[0] == "begin")
        {
            AddMessage(ERROR_BeginOnMapChanged(), Tracer.MessageLevel.error, tracer, options);
        }

        else
        {
            BindingListList[igniter] = NewList(index, command, list);
            AddMessage(igniter.ToString() + "��" + index + "�Ԃ�" + command + "��}�����܂����D", Tracer.MessageLevel.normal, tracer, options);
        }
        
        // - inner function
        static List<SwitchCommand> NewList(int index, string insertContent, List<SwitchCommand> originalList)
        {
            var newList = new List<SwitchCommand>();

            for(var n = 0; n < originalList.Count; n++)
            {
                if (n == index)
                {
                    newList.Add(new SwitchCommand(insertContent));
                }

                newList.Add(new SwitchCommand(originalList[n].command, originalList[n].active));
            }

            return newList;
        }
    }

    static void TrySwap(int i1, int i2, GameEvent igniter, Tracer tracer, List<string> options)
    {
        var list = BindingListList[igniter];
        var indexLim = list.Count - 1;

        var flag = false;

        if (i1 < 0 || i1 > indexLim) { AddMessage(ERROR_OutOfRange(i1, indexLim), Tracer.MessageLevel.error, tracer, options); flag = true; }
        if (i2 < 0 || i2 > indexLim) { AddMessage(ERROR_OutOfRange(i2, indexLim), Tracer.MessageLevel.error, tracer, options); flag = true; }

        if (flag) { return; }

        var tmp = list[i1];
        list[i1] = list[i2];
        list[i2] = tmp;

        AddMessage(i1.ToString() + "�Ԗڂ̃R�}���h��" + i2.ToString() + "�Ԗڂ̃R�}���h�̎��s���������ւ��܂����D", Tracer.MessageLevel.normal, tracer, options);
    }

    static void TryChangeActiveStatus(bool active, List<int> indexes, GameEvent igniter, Tracer tracer, List<string> options)
    {
        var list = BindingListList[igniter];
        var indexLim = list.Count - 1;
        
        foreach(var index in indexes)
        {
            if (index < 0 || index > indexLim)
            {
                AddMessage(ERROR_OutOfRange(index, indexLim), Tracer.MessageLevel.error, tracer, options);
            }
        }

        if (!tracer.NoError) { return; }

        foreach(var index in indexes)
        {
            list[index].active = active;
            AddMessage(index.ToString() + "�Ԗڂ̃R�}���h���~���܂���", Tracer.MessageLevel.normal, tracer, options);
        }
    }

    static void TryRemove(List<int> indexes, GameEvent igniter, Tracer tracer, List<string> options)
    {
        var list = BindingListList[igniter];
        var indexLim = list.Count - 1;

        foreach (var index in indexes)
        {
            if (index < 0 || index > indexLim)
            {
                AddMessage(ERROR_OutOfRange(index, indexLim), Tracer.MessageLevel.error, tracer, options);
            }
        }

        if (!tracer.NoError) { return; }

        foreach (var index in indexes)
        {
            list.RemoveAt(index);
            AddMessage(index.ToString() + "�Ԗڂ̃R�}���h���폜���܂���", Tracer.MessageLevel.normal, tracer, options);
        }
    }

    static bool CheckDuplication(string command, GameEvent igniter, int ignoreIndex = -1)
    {
        var values = CommandReceiver.GetValues(command);
        var corrected = CorrectValues(values);

        var list = BindingListList[igniter];

        for(var n = 0; n < list.Count; n++)
        {
            if (n == ignoreIndex) { continue; }

            var _values = CommandReceiver.GetValues(list[n].command);
            var _corrected = CorrectValues(_values);

            if (corrected == _corrected)
            {
                return true;
            }
        }

        return false;

        // - inner function
        static string CorrectValues(List<string> values)
        {
            var corrected = "";

            foreach(var v in values)
            {
                corrected += v + " ";
            }

            return corrected;
        }
    }

    static void CurrentBinding(Tracer tracer, List<string> options)
    {
        var message = "";

        foreach(var pair in BindingListList)
        {
            if (pair.Value.Count == 0) { continue; }
            message += "\t" + pair.Key.ToString() + " : \n";

            var counter = 0;

            foreach(var command in pair.Value)
            {
                message += "\t\t�y" + counter.ToString() + "�z " + command.command + "\n";
                counter++;
            }
        }

        if (message == "")
        {
            AddMessage("���݊��蓖�Ă͑��݂��܂���D", Tracer.MessageLevel.normal, tracer, options);
            return;
        }

        AddMessage("���݂̊��蓖�Ă͎��̒ʂ�ł�\n" + message, Tracer.MessageLevel.normal, tracer, options);
    }

    public class SwitchCommand
    {
        public string command;
        public bool active;

        public SwitchCommand(string command, bool active = true)
        {
            this.command = command;
            this.active = active;
        }
    }
}