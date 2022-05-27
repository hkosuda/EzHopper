using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoUiBackwardButton : MonoBehaviour
{
    static Button button;

    private void Awake()
    {
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(Backward);
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

    static void Backward()
    {
        DemoManager.ChangePastTime(-2.0f);
    }
}
