using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoUiEndButton : MonoBehaviour
{
    static Button button;

    private void Awake()
    {
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(ToEnd);
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

    static void ToEnd()
    {
        DemoManager.ChangePastTime(DemoManager.Duration);
    }
}
