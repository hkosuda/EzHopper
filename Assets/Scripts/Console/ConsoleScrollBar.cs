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
        frameBufferRemain = frameBuffer;
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
            Console.ConsoleOpened += ReserveUpdate1;
            ConsoleMessage.LogUpdated += ReserveUpdate2;
        }

        else
        {
            Console.ConsoleOpened -= ReserveUpdate1;
            ConsoleMessage.LogUpdated -= ReserveUpdate2;
        }
    }

    private void Update()
    {
        if (frameBufferRemain > 0)
        {
            frameBufferRemain--;

            if (frameBufferRemain == 0)
            {
                SetBottom();
            }
        }
    }

    static void ReserveUpdate1(object obj, bool mute)
    {
        frameBufferRemain = frameBuffer;
    }

    static void ReserveUpdate2(object obj, bool mute)
    {
        frameBufferRemain = frameBuffer;
    }

    static void SetBottom()
    {
        scrBar.value = 0.0f;
    }
}
