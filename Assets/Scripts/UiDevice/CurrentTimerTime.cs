using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentTimerTime : MonoBehaviour
{
    static Bools.Item item = Bools.Item.show_timer;

    static Text text;
    static float currentTime = 0.0f;

    private void Awake()
    {
        text = gameObject.GetComponent<Text>();
    }

    void Start()
    {
        UpdateText();
    }

    void Update()
    {
        if (!Bools.Get(item)) { text.text = ""; currentTime = -1.0f; return; }

        if (currentTime != TimerCommand.PastTime)
        {
            currentTime = TimerCommand.PastTime;
            UpdateText();
        }
    }

    static void UpdateText()
    {
        var info = TimerCommand.TimeString(currentTime);
        text.text = info;
    }
}
