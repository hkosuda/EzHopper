using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoUiStartButton : MonoBehaviour
{
    static Button button;

    private void Awake()
    {
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(ToStart);
    }

    private void Update()
    {
        if (DemoManager.PastTime == 0.0f)
        {
            button.interactable = false;
        }

        else
        {
            button.interactable = true;
        }
    }

    static void ToStart()
    {
        DemoManager.ChangePastTime(-DemoManager.Duration);
    }
}
