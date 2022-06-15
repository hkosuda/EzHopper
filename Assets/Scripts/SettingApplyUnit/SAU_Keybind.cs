using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SAU_Keybind : SettingApplyUnit
{
    readonly string description = "�@�f�t�H���g�� bind, toggle �ݒ�D\n" +
        "P�ō��W���L�^�CV�ŋL�^�������W�ɖ߂�܂��D\n" +
        "�S�[�X�g�̍폜��Z�C���v���C��G�ɐݒ肵�Ă��܂��D" +
        "O�Ő_���_���[�h�̃I���C�I�t���s���܂��D\n" +
        "R�Ń��R�[�_�[�̋N���C��~���s���܂��D\n" +
        "Q�Ń^�C�}�[�̋N���C��~���s���܂��D";

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
