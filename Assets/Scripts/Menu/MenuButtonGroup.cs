using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtonGroup : MonoBehaviour
{
    static GameObject _howToPlayWindow;
    static GameObject _commandWindow;
    static GameObject _commandIndexWindow;
    static GameObject _settingWindow;

    private void Awake()
    {
        if (_howToPlayWindow == null) { _howToPlayWindow = Resources.Load<GameObject>("UI/HowToPlayWindow"); }
        if (_commandWindow == null) { _commandWindow = Resources.Load<GameObject>("UI/CommandWindow"); }
        if (_commandIndexWindow == null) { _commandIndexWindow = Resources.Load<GameObject>("UI/CommandIndexWindow"); }
        if (_settingWindow == null) { _settingWindow = Resources.Load<GameObject>("UI/SettingWindow"); }
    }

    void Start()
    {
        var howToPlayButton = GetButton(0);
        howToPlayButton.onClick.AddListener(OpenHowToPlayWindow);

        var settingButton = GetButton(1);
        settingButton.onClick.AddListener(OpenSettingWindow);

        var commandButton = GetButton(2);
        commandButton.onClick.AddListener(OpenCommandWindow);

        var commandIndexWindow = GetButton(3);
        commandIndexWindow.onClick.AddListener(OpenCommandIndexWindow);

        var restartButton = GetButton(4);
        restartButton.onClick.AddListener(Restart);

        var backToStartButton = GetButton(5);
        backToStartButton.onClick.AddListener(BackToStart);

        var finishGameButton = GetButton(6);
        finishGameButton.onClick.AddListener(FinishTheGame);
    }

    // - inner function
    Button GetButton(int n)
    {
        return gameObject.transform.GetChild(n).gameObject.GetComponent<Button>();
    }

    static void OpenHowToPlayWindow()
    {
        Instantiate(_howToPlayWindow);
    }

    static void OpenSettingWindow()
    {
        Instantiate(_settingWindow);
    }

    static void OpenCommandWindow()
    {
        Instantiate(_commandWindow);
    }

    static void OpenCommandIndexWindow()
    {
        Instantiate(_commandIndexWindow);
    }

    static void Restart()
    {
        Confirmation.BeginConfirmation("再スタートしますか？", RequestRestart);
    }

    static void RequestRestart()
    {
        CommandReceiver.RequestCommand("begin " + MapsManager.CurrentMap.MapName.ToString().ToLower() + " -mute");
    }

    static void BackToStart()
    {
        Confirmation.BeginConfirmation("最初のマップに戻りますか？", RequestBeginAthletic);
    }

    static void RequestBeginAthletic()
    {
        CommandReceiver.RequestCommand("begin " + MapName.ez_athletic.ToString() + " -mute");
    }

    static void FinishTheGame()
    {
        Confirmation.BeginConfirmation("ゲームを終了しますか？", FinishTheGameMethod);
    }

    static void FinishTheGameMethod()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        return;
#endif 

        Application.Quit();
    }
}
