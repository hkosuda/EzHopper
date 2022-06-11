using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgniteCommand : Command
{
    static readonly List<string> availables = new List<string>()
    {
        "add", "insert", "replace", "swap", "remove", "remove_all"
    };

    public enum Igniter
    {
        none,

        on_course_out,
        on_enter_checkpoint,
        on_exit_checkpoint,
        on_enter_start,
        on_exit_start,
        on_enter_goal,
    }

    static public Dictionary<Igniter, List<string>> BindingListList { get; private set; } = new Dictionary<Igniter, List<string>>();

    public IgniteCommand()
    {
        commandName = "ignite";
        description = "ゲーム内の特定のイベントが発生したときにコマンドを呼び出す機能を提供します．\n" +
            "";

        InitializeList();
        SetEvent(1);

        // - inner function
        static void InitializeList()
        {
            BindingListList = new Dictionary<Igniter, List<string>>();

            foreach (Igniter igniter in Enum.GetValues(typeof(Igniter)))
            {
                if (igniter == Igniter.none) { continue; }

                BindingListList.Add(igniter, new List<string>());
            }
        }
    }

    static void SetEvent(int indicator)
    {
        if (indicator > 0)
        {
            InvalidArea.CourseOut += IgniteCourseOut;
            CheckPoint.EnterCheckpoint += IgniteEnterCheckpoint;
            CheckPoint.ExitCheckpoint += IgniteExitCheckpoint;
            CheckPoint.EnterStart += IgniteEnterStart;
            CheckPoint.ExitStart += IgniteExitStart;
            CheckPoint.EnterGoal += IgniteEnterGoal;
        }

        else
        {
            InvalidArea.CourseOut -= IgniteCourseOut;
            CheckPoint.EnterCheckpoint -= IgniteEnterCheckpoint;
            CheckPoint.ExitCheckpoint -= IgniteExitCheckpoint;
            CheckPoint.EnterStart -= IgniteEnterStart;
            CheckPoint.ExitStart -= IgniteExitStart;
            CheckPoint.EnterGoal -= IgniteEnterGoal;
        }
    }

    static void IgniteCourseOut(object obj, Vector3 pos)
    {
        Ignite(Igniter.on_course_out);
    }

    static void IgniteEnterCheckpoint(object obj, Vector3 pos)
    {
        Ignite(Igniter.on_enter_checkpoint);
    }

    static void IgniteExitCheckpoint(object obj, Vector3 pos)
    {
        Ignite(Igniter.on_exit_checkpoint);
    }

    static void IgniteEnterStart(object obj, Vector3 pos)
    {
        Ignite(Igniter.on_enter_start);
    }

    static void IgniteExitStart(object obj, Vector3 pos)
    {
        Ignite(Igniter.on_exit_start);
    }

    static void IgniteEnterGoal(object obj, Vector3 pos)
    {
        Ignite(Igniter.on_enter_goal);
    }

    static void Ignite(Igniter igniter)
    {
        var commandList = BindingListList[igniter];
        if (commandList == null || commandList.Count == 0) { return; }

        foreach (var command in commandList)
        {
            CommandReceiver.RequestCommand(command);
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

            foreach(Igniter igniter in Enum.GetValues(typeof(Igniter)))
            {
                if (igniter == Igniter.none) { continue; }
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
                AddMessage("実行するコマンドを指定してください．", Tracer.MessageLevel.error, tracer, options);
            }

            else if (values.Count == 4)
            {
                var igniterString = values[2];
                var igniter = GetIgniter(igniterString);

                if (igniter == Igniter.none)
                {
                    AddMessage(ERROR_UnknownIgniter(igniterString), Tracer.MessageLevel.error, tracer, options);
                    return;
                }

                var group = values[3];
                var command = CommandReceiver.UnpackGrouping(group);

                if (CheckDuplication(command, igniter))
                {
                    AddMessage(ERROR_DuplicationAlert(), Tracer.MessageLevel.warning, tracer, options);
                    AddMessage(ERROR_Duplication(), Tracer.MessageLevel.error, tracer, options);
                }

                else
                {
                    BindingListList[igniter].Add(command);
                    AddMessage("コマンドを追加しました．", Tracer.MessageLevel.normal, tracer, options);
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
                AddMessage("挿入する位置を示すインデックスを指定してください．", Tracer.MessageLevel.error, tracer, options);
            }

            else if (values.Count == 4)
            {
                AddMessage("挿入するコマンドを指定してください．", Tracer.MessageLevel.error, tracer, options);
            }

            else if (values.Count == 5)
            {
                var igniterString = values[2];
                var indexString = values[3];

                var igniter = GetIgniter(igniterString);

                if (igniter == Igniter.none)
                {
                    AddMessage(ERROR_UnknownIgniter(igniterString), Tracer.MessageLevel.error, tracer, options);
                }

                else if (int.TryParse(indexString, out var index))
                {
                    var command = CommandReceiver.UnpackGrouping(values[4]);

                    if (CheckDuplication(command, igniter))
                    {
                        AddMessage(ERROR_DuplicationAlert(), Tracer.MessageLevel.warning, tracer, options);
                        AddMessage(ERROR_Duplication(), Tracer.MessageLevel.error, tracer, options);
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
                AddMessage("置換対象となるインデックスを指定してください．", Tracer.MessageLevel.error, tracer, options);
            }

            else if (values.Count == 4)
            {
                AddMessage("置換するコマンドを入力してください．", Tracer.MessageLevel.error, tracer, options);
            }

            else if (values.Count == 5)
            {
                var igniterString = values[2];
                var indexString = values[3];

                var igniter = GetIgniter(igniterString);

                if(igniter == Igniter.none)
                {
                    AddMessage(ERROR_UnknownIgniter(igniterString), Tracer.MessageLevel.error, tracer, options);
                }

                else if (int.TryParse(indexString, out var index))
                {
                    var command = CommandReceiver.UnpackGrouping(values[4]);

                    if (CheckDuplication(command, igniter, index))
                    {
                        AddMessage(ERROR_DuplicationAlert(), Tracer.MessageLevel.warning, tracer, options);
                        AddMessage(ERROR_Duplication(), Tracer.MessageLevel.error, tracer, options);
                    }

                    else
                    {
                        TryReplace(index, igniter, command, tracer, options);
                    }
                }

                else
                {
                    AddMessage(indexString + "を整数に変換できません．", Tracer.MessageLevel.error, tracer, options);
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
                AddMessage("入れ替えを行う2つのインデックスを指定してください．", Tracer.MessageLevel.error, tracer, options);
            }

            else if (values.Count == 4)
            {
                AddMessage("入れ替えを行う2つのインデックスを指定してください．", Tracer.MessageLevel.error, tracer, options);
            }

            else if (values.Count == 5)
            {
                var igniterString = values[2];
                var igniter = GetIgniter(igniterString);

                if (igniter == Igniter.none)
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

        // ex) ignite(0) remove(1) on_course_out(2) 0(3) 1(4) 4(5) 8(6) ... 
        else if (action == "remove")
        {
            if (values.Count == 2)
            {
                AddMessage(ERROR_TriggerIsNeccesary(), Tracer.MessageLevel.error, tracer, options);
            }

            else if (values.Count == 3)
            {
                AddMessage("削除するコマンドのインデックスを指定してください．", Tracer.MessageLevel.error, tracer, options);
            }

            else
            {
                var igniterString = values[2];
                var igniter = GetIgniter(igniterString);

                if (igniter == Igniter.none)
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
                var igniter = GetIgniter(igniterString);

                if (igniter == Igniter.none)
                {
                    AddMessage(ERROR_UnknownIgniter(igniterString), Tracer.MessageLevel.error, tracer, options);
                    return;
                }

                BindingListList[igniter] = new List<string>();
                AddMessage(igniter + "に結びつけられたコマンドをすべて削除しました．", Tracer.MessageLevel.normal, tracer, options);
            }
        }

        else
        {
            Debug.Log(action);
            AddMessage(ERROR_AvailableOnly(1, availables), Tracer.MessageLevel.error, tracer, options);
        }

        // - inner function
        static string ERROR_UnknownIgniter(string igniterString)
        {
            return igniterString + "をトリガーとなるイベント名に変換できません．";
        }

        // - inner function
        static string ERROR_TriggerIsNeccesary()
        {
            return "トリガーとなるイベント名を指定してください．";
        }

        // - inner function
        static string ERROR_Duplication()
        {
            return "重複するコマンドがあるため，処理に失敗しました．";
        }

        // - inner function
        static string ERROR_DuplicationAlert()
        {
            return "重複の判定は，オプションの有無を無視して行われます．";
        }
    }

    static Igniter GetIgniter(string name)
    {
        foreach(Igniter igniter in Enum.GetValues(typeof(Igniter)))
        {
            if (igniter.ToString().ToLower() == name.ToLower())
            {
                return igniter;
            }
        }

        return Igniter.none;
    }

    static void TryReplace(int index, Igniter igniter, string command, Tracer tracer, List<string> options)
    {
        var list = BindingListList[igniter];

        if (index < 0 || index > list.Count - 1)
        {
            AddMessage(ERROR_OutOfRange(index, list.Count - 1), Tracer.MessageLevel.error, tracer, options);
            return;
        }

        list[index] = command;
        AddMessage(index.ToString() + "番目のコマンドを" + command + "に置換しました．", Tracer.MessageLevel.normal, tracer, options);
    }

    static void TryInsert(int index, Igniter igniter, string command, Tracer tracer, List<string> options)
    {
        var list = BindingListList[igniter];

        if (index < 0 || index > list.Count - 1) 
        {
            AddMessage(ERROR_OutOfRange(index, list.Count - 1), Tracer.MessageLevel.error, tracer, options);
            return;
        }

        BindingListList[igniter] = NewList(index, command, list);
        AddMessage(index + "番に" + command + "を挿入しました．", Tracer.MessageLevel.normal, tracer, options);
        
        // - inner function
        static List<string> NewList(int index, string insertContent, List<string> originalList)
        {
            var newList = new List<string>();

            for(var n = 0; n < originalList.Count; n++)
            {
                if (n == index)
                {
                    newList.Add(insertContent);
                }

                newList.Add(originalList[n]);
            }

            return newList;
        }
    }

    static void TrySwap(int i1, int i2, Igniter igniter, Tracer tracer, List<string> options)
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

        AddMessage(i1.ToString() + "番目のコマンドと" + i2.ToString() + "番目のコマンドの実行順序を入れ替えました．", Tracer.MessageLevel.normal, tracer, options);
    }

    static void TryRemove(List<int> indexes, Igniter igniter, Tracer tracer, List<string> options)
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
            list.RemoveAt(index);
            AddMessage(index.ToString() + "番目のコマンドを削除しました", Tracer.MessageLevel.error, tracer, options);
        }
    }

    static bool CheckDuplication(string command, Igniter igniter, int ignoreIndex = -1)
    {
        var values = CommandReceiver.GetValues(command);
        var corrected = CorrectValues(values);

        var list = BindingListList[igniter];

        for(var n = 0; n < list.Count; n++)
        {
            if (n == ignoreIndex) { continue; }

            var _values = CommandReceiver.GetValues(list[n]);
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
                message += "\t\t【" + counter.ToString() + "】 " + command + "\n";
            }
        }

        if (message == "")
        {
            AddMessage("現在割り当ては存在しません．", Tracer.MessageLevel.normal, tracer, options);
            return;
        }

        AddMessage("現在の割り当ては次の通りです\n" + message, Tracer.MessageLevel.normal, tracer, options);
    }
}
