using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SAU_Toxic : SettingApplyUnit
{
    string description = "　謎の設定";

    List<string> commandList = new List<string>()
    {
        "invoke add on_course_out \"delayedchat 1.0 3.5 ... -m\" -e",
        "invoke add on_course_out \"delayedchat 1.0 3.5 ? -m\" -e",
        "invoke add on_course_out \"delayedchat 1.0 3.5 noob -m\" -e",
        "invoke add on_course_out \"delayedchat 1.0 3.5 idiot -m\" -e",
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
