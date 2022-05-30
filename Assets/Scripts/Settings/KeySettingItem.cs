using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeySettingItem : MonoBehaviour
{
    static readonly Dictionary<Keyconfig.KeyAction, string> keyText = new Dictionary<Keyconfig.KeyAction, string>()
    {
        { Keyconfig.KeyAction.shot, "�ˌ�" },
        { Keyconfig.KeyAction.forward, "�O�i" },
        { Keyconfig.KeyAction.backward, "���" },
        { Keyconfig.KeyAction.right, "�E�Ɉړ�" },
        { Keyconfig.KeyAction.left, "���Ɉړ�" },
        { Keyconfig.KeyAction.jump, "�W�����v" },
        { Keyconfig.KeyAction.menu, "���j���[���J��" },
        { Keyconfig.KeyAction.console, "�R���\�[�����J��" },
    };

    static public EventHandler<Keyconfig.KeyAction> SettingBegin { get; set; }
    static public EventHandler<Keyconfig.KeyAction> SettingEnd { get; set; }

    Keyconfig.KeyAction keyAction;

    Text titleText;
    Text buttonText;

    Button button;

    private void Awake()
    {
        titleText = gameObject.transform.GetChild(0).gameObject.GetComponent<Text>();
        button = gameObject.transform.GetChild(1).gameObject.GetComponent<Button>();
        buttonText = button.transform.GetChild(0).gameObject.GetComponent<Text>();

        button.onClick.AddListener(BeginKeySetting);
    }

    void Start()
    {
        SetEvent(1);
    }

    private void OnDestroy()
    {
        SetEvent(-1);
    }

    void SetEvent(int indicator)
    {
        if (indicator > 0)
        {
            SettingBegin += InactivateButton;
            SettingEnd += ActivateButton;
        }

        else
        {
            SettingBegin -= InactivateButton;
            SettingEnd -= ActivateButton;
        }
    }

    void BeginKeySetting()
    {
        SettingBegin?.Invoke(null, keyAction);
    }

    void InactivateButton(object obj, Keyconfig.KeyAction keyAction)
    {
        button.interactable = false;
    }

    void ActivateButton(object obj, Keyconfig.KeyAction keyAction)
    {
        button.interactable = true;

        UpdateContent();
    }

    void UpdateContent()
    {
        var key = Keyconfig.KeybindList[keyAction];

        titleText.text = keyAction.ToString();
        buttonText.text = key.GetKeyString();
    }

    public void Initialize(Keyconfig.KeyAction keyAction)
    {
        this.keyAction = keyAction;
        
        UpdateContent();
    }
}
