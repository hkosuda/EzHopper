using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecorderCommand : Command
{
    static List<string> availables = new List<string>()
    {
        "start", "end", "stop", "save", "remove", "remove_last"
    };

    public RecorderCommand()
    {
        commandName = "recorder";
        description = "プレイヤーの動きを記録する機能（レコーダー）を提供します．\n";
        detail = "'recorder start' で記録を開始し，'recorder end' で記録を停止します．記録したデータは，次の記録が終了するまで一時的に保存されます．" + 
            "'ghost' や 'replay' の実行時に利用されるデータは，この一時的に保存されたデータです．\n" +
            "'recorder stop' を実行すると，一時的な保存データを書き換えることなくレコーダーを停止できます．" + 
            "一時的に保存されている間に'recorder save <name>'を実行すると，ゲームを起動している間だけ名前付きでデータを保持し続けます（<name>の部分に任意の名前を入力します）．" +
            "ここで作成した名前付きデータは，'replay' コマンドや 'ghost' コマンドで利用可能となります．\n" +
            "レコーダーは，" + Floats.Item.recorder_limit_time.ToString() + "で指定された時間が経過すると自動で停止します．\n" +
            "保存したデータを削除するには，'recorder remove <name>'を実行してください．また，'remove_last' で最後に保存が行われたデータ名を持つデータを削除することができます．";
    }

    public override List<string> AvailableValues(List<string> values)
    {
        if (values == null || values.Count == 0) { return new List<string>(); }

        if (values.Count < 3)
        {
            return availables;
        }

        return new List<string>();
    }

    public override void CommandMethod(Tracer tracer, List<string> values, List<string> options)
    {
        if (values == null || values.Count == 0) { return; }

        if (values.Count == 1)
        {
            AddMessage(AvailabeDataList(), Tracer.MessageLevel.normal, tracer, options);
        }

        else if (values.Count == 2)
        {
            var value = values[1];

            if (value == "start")
            {
                PlayerRecorder.BeginRecording();
                AddMessage("レコーダーを起動しました．", Tracer.MessageLevel.normal, tracer, options);
            }

            else if (value == "end")
            {
                PlayerRecorder.FinishRecording(true);
                AddMessage("レコーダーを停止しました．", Tracer.MessageLevel.normal, tracer, options);
            }

            else if (value == "stop")
            {
                PlayerRecorder.FinishRecording(false);
                AddMessage("レコーダーによる記録を中断しました．", Tracer.MessageLevel.normal, tracer, options);
            }

            else if (value == "remove_last")
            {
                RecordCacheSystem.RemoveLast(tracer, options);
            }

            else if (value == "save")
            {
                AddMessage("データを保存するには，名前を指定してください．", Tracer.MessageLevel.error, tracer, options);
            }

            else if (value == "remove")
            {
                AddMessage("データを削除するには，名前を指定してください．", Tracer.MessageLevel.error, tracer, options);
            }

            else
            {
                AddMessage(ERROR_AvailableOnly(1, availables), Tracer.MessageLevel.error, tracer, options);
            }
        }
        
        else if (values.Count == 3)
        {
            var value = values[1];
            var name = values[2];

            if (value == "save")
            {
                RecordCacheSystem.CacheData(name, tracer, options);
            }

            else if (value == "remove")
            {
                RecordCacheSystem.RemoveData(name, tracer, options);
            }

            else
            {
                AddMessage("値を2個指定する場合，" + ERROR_AvailableOnly(1, new List<string>() { "save", "remove" }), Tracer.MessageLevel.error, tracer, options);
            }
        }

        else
        {
            AddMessage("3個以上の値を指定することはできません．", Tracer.MessageLevel.error, tracer, options);
        }
    }

    static string AvailabeDataList()
    {
        var text = "利用可能なデータの一覧（データ名|マップ名|データサイズ|継続時間）\n";

        if (RecordCacheSystem.CachedDataList == null || RecordCacheSystem.CachedDataList.Count == 0)
        {
            text += "\t\t（現在利用可能なデータはありません）";
            return text;
        }

        foreach(var  data in RecordCacheSystem.CachedDataList)
        {
            text += "\t\t| " + data.Key + "\t | " + data.Value.mapName + "\t | " + data.Value.dataList.Count 
                + "\t | " + data.Value.dataList.Last()[0].ToString("f1") + "\n";
        }

        return text.TrimEnd(new char[1] { '\n' });
    }
}
