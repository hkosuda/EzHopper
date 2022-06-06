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
        content += "ジャンプ　　　　：" + Keyconfig.KeybindList[Keyconfig.KeyAction.jump].GetKeyString() + "\n";
        content += "オートジャンプ　：" + Keyconfig.KeybindList[Keyconfig.KeyAction.autoJump].GetKeyString() + "\n";
        content += "前進　　　　　　：" + Keyconfig.KeybindList[Keyconfig.KeyAction.forward].GetKeyString() + "\n";
        content += "後退　　　　　　：" + Keyconfig.KeybindList[Keyconfig.KeyAction.backward].GetKeyString() + "\n";
        content += "右に移動　　　　：" + Keyconfig.KeybindList[Keyconfig.KeyAction.right].GetKeyString() + "\n";
        content += "左に移動　　　　：" + Keyconfig.KeybindList[Keyconfig.KeyAction.left].GetKeyString() + "\n";
        content += "射撃　　　　　　：" + Keyconfig.KeybindList[Keyconfig.KeyAction.shot].GetKeyString() + "\n";
        content += "メニューを開く　：" + Keyconfig.KeybindList[Keyconfig.KeyAction.menu].GetKeyString() + "\n";
        content += "コンソールを開く：" + Keyconfig.KeybindList[Keyconfig.KeyAction.console].GetKeyString();

        text.text = content;
    }
}
