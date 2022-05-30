using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Console : MonoBehaviour
{
    static public EventHandler<bool> ConsoleOpened { get; set; }
    static public EventHandler<bool> ConsoleClosed { get; set; }

    static GameObject canvas;

    private void Awake()
    {
        canvas = gameObject.transform.GetChild(0).gameObject;
    }

    private void Start()
    {
        SetEvent(1);
        ConsoleMessage.Initialize();

        CloseConsole();
    }

    private void OnDestroy()
    {
        SetEvent(-1);
        ConsoleMessage.Shutdown();
    }

    static void SetEvent(int indicator)
    {
        if (indicator > 0)
        {
            Timer.Updated += UpdateMethod;
        }

        else
        {
            Timer.Updated -= UpdateMethod;
        }
    }


    static void UpdateMethod(object obj, float dt)
    {
        if (!canvas.activeSelf && Keyconfig.CheckInput(Keyconfig.KeyAction.console, true))
        {
            OpenConsole();
        }
    }

    static public void CloseConsole()
    {
        Timer.Resume();
        canvas.SetActive(false);

        ConsoleClosed?.Invoke(null, false);
    }

    static public void OpenConsole()
    {
        Timer.Pause();
        canvas.SetActive(true);

        ConsoleInputField.ActivateInputField();

        ConsoleOpened?.Invoke(null, false);
    }
}
