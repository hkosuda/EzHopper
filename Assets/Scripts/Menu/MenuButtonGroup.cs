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

        var backToStartButton = GetButton(3);
        backToStartButton.onClick.AddListener(BackToStart);

        var finishGameButton = GetButton(4);
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
