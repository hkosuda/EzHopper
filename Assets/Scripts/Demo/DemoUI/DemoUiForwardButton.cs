using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoUiForwardButton : MonoBehaviour
{
    static Button button;

    private void Awake()
    {
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(Forward);
    }

    private void Update()
    {
        if (DemoManager.PastTime == DemoManager.Duration)
        {
            button.interactable = false;
        }

        else
        {
            button.interactable = true;
        }
    }

    static void Forward()
    {
        DemoManager.ChangePastTime(2.0f);
    }
}
