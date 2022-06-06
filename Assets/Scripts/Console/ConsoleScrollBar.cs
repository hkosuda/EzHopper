using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleScrollBar : MonoBehaviour
{
    static Scrollbar scrBar;

    static readonly int frameBuffer = 3;
    static int frameBufferRemain;

    private void Awake()
    {
        scrBar = gameObject.GetComponent<Scrollbar>();
    }

    void Start()
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
            Console.ConsoleOpened += SetBottom;
            ConsoleMessage.LogUpdated += ReserveUpdate;
        }

        else
        {
            Console.ConsoleOpened -= SetBottom;
            ConsoleMessage.LogUpdated -= ReserveUpdate;
        }
    }

    private void Update()
    {
        if (frameBufferRemain > 0)
        {
            frameBufferRemain--;

            if (frameBufferRemain <= 0)
            {
                SetBottom(null, false);
            }
        }
    }

    static void SetBottom(object obj, bool mute)
    {
        scrBar.value = 0.0f;
    }

    static void ReserveUpdate(object obj, bool mute)
    {
        frameBufferRemain = frameBuffer;
    }
}
