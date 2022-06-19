using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingItemsManager : MonoBehaviour
{
    static GameObject myself;
    static GameObject _keySettingItem;
    static GameObject _settingTitle;
    static GameObject _settingItem;

    static bool settingMode;
    static Keyconfig.KeyAction keyAction;

    private void Awake()
    {
        myself = gameObject;
        if (_keySettingItem == null) { _keySettingItem = Resources.Load<GameObject>("UiComponent/KeySettingItem"); }
        if (_settingTitle == null) { _keySettingItem = Resources.Load<GameObject>("UiComponent/SettingTitle"); }
        if (_settingItem == null) { _keySettingItem = Resources.Load<GameObject>("UiComponent/SettingItem"); }

        DeployItems();
    }

    void Start()
    {
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
            KeySettingItem.SettingBegin += BeginSettingMode;
            
        }

        else
        {
            KeySettingItem.SettingBegin -= BeginSettingMode;
        }
    }

    static void BeginSettingMode(object obj, Keyconfig.KeyAction _keyAction)
    {
        settingMode = true;
        keyAction = _keyAction;
    }

    private void Update()
    {
        InGameTimer.Pause();
        if (!settingMode) { return; }

        if (Input.anyKey)
        {
            foreach (KeyCode code in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(code))
                {
                    Keyconfig.SetKey(keyAction, keyCode: code);

                    settingMode = false;
                    KeySettingItem.SettingEnd?.Invoke(null, keyAction);
                }
            }
        }

        else if (Input.mouseScrollDelta.y < 0)
        {
            settingMode = false;
            Keyconfig.SetKey(keyAction, wheelDelta: -1.0f);

            settingMode = false;
            KeySettingItem.SettingEnd?.Invoke(null, keyAction);
        }

        else if (Input.mouseScrollDelta.y > 0)
        {
            settingMode = false;
            Keyconfig.SetKey(keyAction, wheelDelta: 1.0f);

            settingMode = false;
            KeySettingItem.SettingEnd?.Invoke(null, keyAction);
        }
    }

    static void DeployItems()
    {
        foreach(var keybind in Keyconfig.KeybindList)
        {
            var item = Instantiate(_keySettingItem);
            item.transform.SetParent(myself.transform);

            var component = item.GetComponent<KeySettingItem>();
            component.Initialize(keybind.Key);
        }
    }
}
