using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoUiDurationText : MonoBehaviour
{
    static Text text;
    static float currentDuration;

    private void Awake()
    {
        text = gameObject.GetComponent<Text>();
    }

    void Start()
    {
        currentDuration = DemoManager.Duration;
        UpdateContent();
    }

    void Update()
    {
        if (currentDuration != DemoManager.Duration)
        {
            currentDuration = DemoManager.Duration;
            UpdateContent();
        }
    }

    static void UpdateContent()
    {
        var time = Utils.TimeText(currentDuration, false);
        text.text = time;
    }
}
