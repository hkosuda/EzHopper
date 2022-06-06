using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpTexts : MonoBehaviour
{
    static readonly Bools.Item item = Bools.Item.show_help;

    static Text text;

    private void Awake()
    {
        text = gameObject.transform.GetChild(0).gameObject.GetComponent<Text>();
    }

    void Start()
    {
        UpdateVisibility(null, false);
        UpdateContent(null, Keyconfig.KeyAction.none);

        SetEvent(1);
    }

    private void OnDestroy()
    {
        SetEvent(-1);
    }

    static void SetEvent(int indicator)
    {
        if (indicator > 0)
        {
            Bools.Settings[item].ValueUpdated += UpdateVisibility;
            Keyconfig.KeyUpdated += UpdateContent;
        }

        else
        {
            Bools.Settings[item].ValueUpdated -= UpdateVisibility;
            Keyconfig.KeyUpdated -= UpdateContent;
        }
    }

    static void UpdateVisibility(object obj, bool prev)
    {
        text.gameObject.SetActive(Bools.Get(item));
    }

    static void UpdateContent(object obj, Keyconfig.KeyAction action)
    {
        var content = "";
        content += "�W�����v�@�@�@�@�F" + Keyconfig.KeybindList[Keyconfig.KeyAction.jump].GetKeyString() + "\n";
        content += "�I�[�g�W�����v�@�F" + Keyconfig.KeybindList[Keyconfig.KeyAction.autoJump].GetKeyString() + "\n";
        content += "�O�i�@�@�@�@�@�@�F" + Keyconfig.KeybindList[Keyconfig.KeyAction.forward].GetKeyString() + "\n";
        content += "��ށ@�@�@�@�@�@�F" + Keyconfig.KeybindList[Keyconfig.KeyAction.backward].GetKeyString() + "\n";
        content += "�E�Ɉړ��@�@�@�@�F" + Keyconfig.KeybindList[Keyconfig.KeyAction.right].GetKeyString() + "\n";
        content += "���Ɉړ��@�@�@�@�F" + Keyconfig.KeybindList[Keyconfig.KeyAction.left].GetKeyString() + "\n";
        content += "�ˌ��@�@�@�@�@�@�F" + Keyconfig.KeybindList[Keyconfig.KeyAction.shot].GetKeyString() + "\n";
        content += "���j���[���J���@�F" + Keyconfig.KeybindList[Keyconfig.KeyAction.menu].GetKeyString() + "\n";
        content += "�R���\�[�����J���F" + Keyconfig.KeybindList[Keyconfig.KeyAction.console].GetKeyString();

        text.text = content;
    }
}
