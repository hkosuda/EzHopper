using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoUiCurrentTime : MonoBehaviour
{
    static Text text;
    static float currentTime;

    private void Awake()
    {
        text = gameObject.GetComponent<Text>();
    }

    void Start()
    {
        currentTime = DemoManager.PastTime;
        UpdateText();
    }

    void Update()
    {
        if (currentTime != DemoManager.PastTime)
        {
            currentTime = DemoManager.PastTime;
            UpdateText();
        }
    }

    static void UpdateText()
    {
        text.text = Utils.TimeText(currentTime);
    }
}
