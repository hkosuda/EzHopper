using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecorderCommand : Command
{
    public RecorderCommand()
    {
        commandName = "recorder";
        description = "プレイヤーの動きを記録する機能（レコーダー）を提供します．\n" +
            "レコーダーは，無効なエリアに侵入したとき，もしくは" + ((int)PlayerRecorder.limitTime).ToString() + "秒経過すると自動で停止します．\n" +
            "'recorder begin'で記録を開始し，'recorder end'で記録を停止します．記録したデータは，次の記録が終了するまで一時的に保存されます．\n" +
            "一時的に保存されている間に'recorder save <name>'を実行すると，ゲームを起動している間だけ名前付きでデータを保持し続けます（<name>の部分に任意の名前を入力します．" +
            "ここで作成した名前付きデータは，demoコマンドやghostコマンドで利用可能となります．\n" +
            "保存したデータを削除するには，'recorder begin <name>'を実行してください．";
    }

    public override List<string> AvailableValues(List<string> values)
    {
        if (values == null || values.Count == 0) { return new List<string>(); }

        if (values.Count < 3)
        {
            return new List<string>() { "begin", "end", "stop", "save", "remove" };
        }

        return new List<string>();
    }

    public override void CommandMethod(Tracer tracer, List<string> values)
    {
        if (values == null || values.Count == 0) { return; }

        if (values.Count == 1)
        {
            tracer.AddMessage(AvailabeDataList(), Tracer.MessageLevel.normal);
            PlayerRecorder.CurrentRecorderStatus(tracer);
            return;
        }

        if (values.Count == 2)
        {
            var value = values[1];

            if (value == "begin")
            {
                PlayerRecorder.BeginRecording();

                tracer.AddMessage("レコーダーを起動しました．", Tracer.MessageLevel.normal);
                return;
            }

            if (value == "end")
            {
                PlayerRecorder.FinishRecording(true);

                tracer.AddMessage("レコーダーを停止しました．", Tracer.MessageLevel.normal);
                return;
            }

            if (value == "stop")
            {
                PlayerRecorder.FinishRecording(false, true);

                tracer.AddMessage("レコーダーによる記録を中断しました．", Tracer.MessageLevel.normal);
                return;
            }

            if (value == "save")
            {
                tracer.AddMessage("データを保存するには，名前を指定してください．", Tracer.MessageLevel.error);
                return;
            }

            if (value == "remove")
            {
                tracer.AddMessage("データを削除するには，名前を指定してください．", Tracer.MessageLevel.error);
                return;
            }

            tracer.AddMessage("一番目の値としては，'begin', 'end', 'stop', 'save', 'remove' のみ指定可能です．", Tracer.MessageLevel.error);
            return;
        }
        
        if (values.Count == 3)
        {
            if (values[1] == "save")
            {
                RecordCacheSystem.CacheData(values[2], tracer);
                return;
            }

            if (values[1] == "remove")
            {
                RecordCacheSystem.RemoveData(values[2], tracer);
                return;
            }

            else
            {
                tracer.AddMessage("一番目の値としては，'begin', 'end', 'stop', 'save', 'remove' のみ指定可能です．", Tracer.MessageLevel.error);
                return;
            }
        }

        tracer.AddMessage("3個以上の値を指定することはできません．", Tracer.MessageLevel.error);
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
