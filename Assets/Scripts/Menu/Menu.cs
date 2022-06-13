using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    static GameObject canvas;

    void Awake()
    {
        canvas = gameObject.transform.GetChild(0).gameObject;

        var closeButton = gameObject.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<Button>();
        closeButton.onClick.AddListener(CloseMenu);

        canvas.SetActive(false);
    }

    private void Start()
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
            InGameTimer.Updated += UpdateMethod;
        }

        else
        {
            InGameTimer.Updated -= UpdateMethod;
        }
    }

    static void UpdateMethod(object obj, float dt)
    {
        if (Keyconfig.CheckInput(Keyconfig.KeyAction.menu, true))
        {
            OpenMenu();
        }
    }

    static void OpenMenu()
    {
        canvas.SetActive(true);
        InGameTimer.Pause();
    }

    static void CloseMenu()
    {
        canvas.SetActive(false);
        InGameTimer.Resume();
    }
}
