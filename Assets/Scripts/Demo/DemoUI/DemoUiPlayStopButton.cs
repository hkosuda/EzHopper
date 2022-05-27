using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoUiPlayStopButton : MonoBehaviour
{
    static bool paused;
    static Text buttonText;

    private void Awake()
    {
        var button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(StartStop);

        buttonText = gameObject.transform.GetChild(0).gameObject.GetComponent<Text>();
    }

    private void Start()
    {
        paused = DemoTimer.Paused;
        UpdateText();
    }

    private void Update()
    {
        if (paused != DemoTimer.Paused)
        {
            paused = DemoTimer.Paused;
            UpdateText();
        }
    }

    static void StartStop()
    {
        if (DemoTimer.Paused)
        {
            DemoTimer.Resume();
        }

        else
        {
            DemoTimer.Pause();
        }
    }

    static void UpdateText()
    {
        if (paused)
        {
            buttonText.text = ">";
        }

        else
        {
            buttonText.text = "||";
        }
    }
}
