using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SAU_S2GCounter : SettingApplyUnit
{
    string description = "�@�X�^�[�g����S�[���ɓ��B����܂łɉ��x�g���C��������ʒm���܂��D";

    List<string> commandList = new List<string>()
    {
        "invoke add on_course_out \"back 0\" -e",

        "invoke add on_map_chaged \"counter set 0 -m\" -e",
        "invoke add on_enter_start \"counter add -m\" -e",
        "invoke add on_enter_start \"chat ���݁F%counter%��ڂ̒��� -m\" -e",
        "invoke add on_enter_goal \"chat �N���A�ɗv�����񐔁F%counter% -m\" -e",
        "invoke add on_enter_goal \"counter set 0 -m\" -e",
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
