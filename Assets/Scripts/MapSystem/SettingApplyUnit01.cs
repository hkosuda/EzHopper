using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SettingApplyUnit01 : SettingApplyUnit
{
    string description = "あああ ";

    List<string> commandList = new List<string>()
    {
        "invoke add on_course_out \"back -m\"",

        "invoke add on_exit_checkpoint \"recorder start -f\"",
        "invoke add on_enter_checkpoint \"recorder end -f\"",
        "invoke add on_enter_another_checkpoint \"recorder save %map%_%now% -f\"",
        "invoke add on_course_out \"recorder stop -f\"",

        "invoke add on_exit_checkpoint \"timer start -f\"",
        "invoke add on_enter_checkpoint \"timer stop -m\"",
        "invoke add on_enter_checkpoint \"timer now -f\"",

        "invoke add on_enter_checkpoint \"ghost\" -m",
        "invoke add on_exit_checkpoint \"ghost end -m\"",

        "bind p \"anchor set -echo\"",
        "bind v \"anchor back -echo\"",
        "bind r \"recorder start -echo\"",
        "bind f \"recorder end -echo\"",
        "bind z \"ghost end -echo\"",
    };

    private void Awake()
    {
        Initialize(description, commandList);
    }

    protected override void RunCommands(object obj, RaycastHit hit)
    {
        if (applyButtonBody != hit.collider.gameObject) { Debug.Log("JFOIJEIJROEIJV"); return; }

        base.RunCommands(obj, hit);

        foreach(var command in commandList)
        {
            CommandReceiver.RequestCommand(command);
        }

        ConsoleMessage.WriteLog("<color=lime>コマンドの自動実行を終了しました．</color>");

        var message = "コマンドの自動実行を行いました．実行した内容は次の通りです．\n";
        message += RunCommandText(commandList);

        ChatMessages.SendChat(message, ChatMessages.Sender.system);
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
