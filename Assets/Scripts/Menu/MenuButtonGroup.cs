using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtonGroup : MonoBehaviour
{
    static GameObject _howToPlayWindow;
    static GameObject _settingWindow;

    private void Awake()
    {
        if (_howToPlayWindow == null) { _howToPlayWindow = Resources.Load<GameObject>("UI/HowToPlayWindow"); }
        if (_settingWindow == null) { _settingWindow = Resources.Load<GameObject>("UI/SettingWindow"); }
    }

    void Start()
    {
        var howToPlayButton = GetButton(0);
        howToPlayButton.onClick.AddListener(OpenHowToPlayWindow);

        var settingButton = GetButton(1);
        settingButton.onClick.AddListener(OpenSettingWindow);

        var restartButton = GetButton(2);
        restartButton.onClick.AddListener(Restart);
    }

    // - inner function
    Button GetButton(int n)
    {
        return gameObject.transform.GetChild(n).gameObject.GetComponent<Button>();
    }

    void OpenHowToPlayWindow()
    {
        Instantiate(_howToPlayWindow);
    }

    void OpenSettingWindow()
    {
        Instantiate(_settingWindow);
    }

    void Restart()
    {
        CommandReceiver.RequestCommand("", true);
    }
}
