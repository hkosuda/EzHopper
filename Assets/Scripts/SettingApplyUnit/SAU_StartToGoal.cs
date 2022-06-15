using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SAU_StartToGoal : SettingApplyUnit
{
    string description = "　スタートからゴールまでの計測を行います．\n" +
        "　コースアウトするとスタート位置までもどされ，" +
        "　次のチェックポイントに到達するたびにタイマーとレコーダーを停止し，データをセーブします．\n" +
        "　さらに，もとのチェックポイントに復帰もしくは次のチェックポイントに到達するたびにゴーストを自動で再生します．";

    List<string> commandList = new List<string>()
    {
        "invoke remove_all all -e",

        "invoke add on_course_out \"back 0 -m\" -e",

        "invoke add on_exit_start \"recorder start -f\" -e",
        "invoke add on_enter_start \"recorder end -f\" -e",
        "invoke add on_enter_goal \"recorder save %map%_%now% -f\" -e",
        "invoke add on_course_out \"recorder stop -f\" -e",

        "invoke add on_exit_start \"timer start -f\" -e",
        "invoke add on_enter_goal \"timer stop -m\" -e",
        "invoke add on_enter_goal \"chat スタートからゴールまでに経過した時間：%time% -m\" -e",
        "invoke add on_enter_next_checkpoint \"timer now -f\" -e",

        "invoke add on_enter_goal \"ghost -m\" -e",
        "invoke add on_exit_checkpoint \"ghost end -m\" -e",
    };

    private void Awake()
    {
        Initialize(description, commandList);
    }

    protected override void OnShot(object obj, RaycastHit hit)
    {
        if (applyButtonBody != hit.collider.gameObject) { return; }

        ConsoleMessage.WriteLog("<color=lime>コマンドの自動実行を開始しました．</color>");

        foreach (var command in commandList)
        {
            CommandReceiver.RequestCommand(command);
        }

        ConsoleMessage.WriteLog("<color=lime>コマンドの自動実行を終了しました．</color>");
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (Vector3.Distance(transform.position, SceneView.currentDrawingSceneView.camera.transform.position) > 50.0f)
        {
            return;
        }

        Initialize(description, commandList);
    }
#endif
}
