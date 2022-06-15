using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SAU_Default : SettingApplyUnit
{
    readonly string description = "�@�f�t�H���g�� invoke �ݒ�D\n" +
        "�@���̃`�F�b�N�|�C���g�ɓ��B���邽�тɃ^�C�}�[�ƃ��R�[�_�[���~���C�f�[�^���Z�[�u���܂��D\n" +
        "�@����ɁC���Ƃ̃`�F�b�N�|�C���g�ɕ��A�������͎��̃`�F�b�N�|�C���g�ɓ��B���邽�тɃS�[�X�g�������ōĐ����܂��D";

    static public readonly List<string> commandList = new List<string>()
    {
        "invoke remove_all all -e",

        "invoke add on_course_out \"back -m\" -e",

        "invoke add on_exit_checkpoint \"recorder start -f\" -e",
        "invoke add on_enter_checkpoint \"recorder end -f\"",
        "invoke add on_enter_next_checkpoint \"recorder save %map%_%now% -f\" -e",
        "invoke add on_course_out \"recorder stop -f\" -e",

        "invoke add on_exit_checkpoint \"timer start -f\" -e",
        "invoke add on_enter_checkpoint \"timer stop -m\" -e",
        "invoke add on_enter_next_checkpoint \"timer now -f\" -e",

        "invoke add on_enter_checkpoint \"ghost -m\" -e",
        "invoke add on_exit_checkpoint \"ghost end -m\" -e",
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
