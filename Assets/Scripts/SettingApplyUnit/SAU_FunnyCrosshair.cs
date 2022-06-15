using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SAU_FunnyCrosshair : SettingApplyUnit
{
    string description = "�@��̐ݒ�i���̂Q�j";

    List<string> commandList = new List<string>()
    {
        "toggle t \"crosshair_length 6 -m\" \"crosshair_length 999 -m\" -e",
    };

    private void Awake()
    {
        Initialize(description, commandList);
    }

    protected override void OnShot(object obj, RaycastHit hit)
    {
        if (applyButtonBody != hit.collider.gameObject) { return; }

        ConsoleMessage.WriteLog("<color=lime>�R�}���h�̎������s���J�n���܂����D</color>");

        foreach (var command in commandList)
        {
            CommandReceiver.RequestCommand(command);
        }

        ConsoleMessage.WriteLog("<color=lime>�R�}���h�̎������s���I�����܂����D</color>");
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
