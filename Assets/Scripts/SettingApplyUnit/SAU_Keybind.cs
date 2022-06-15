using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SAU_Keybind : SettingApplyUnit
{
    readonly string description = "　デフォルトの bind, toggle 設定．\n" +
        "Pで座標を記録，Vで記録した座標に戻ります．\n" +
        "ゴーストの削除をZ，リプレイをGに設定しています．" +
        "Oで神視点モードのオン，オフを行います．\n" +
        "Rでレコーダーの起動，停止を行います．\n" +
        "Qでタイマーの起動，停止を行います．";

    static public readonly List<string> commandList = new List<string>()
    {
        "bind p \"anchor set -f\" -e",
        "bind v \"anchor back -m\" -e",
        "bind z \"ghost end -m\" -e",
        "bind g \"replay -f\" -e",
        "toggle o \"observer start -f\" \"observer end -f\" -e",
        "toggle r \"recorder start -f\" \"recorder end -f\" -e",
        "toggle q \"timer start -f\" \"timer stop -f\" -e",
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
