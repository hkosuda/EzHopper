using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    static public EventHandler<float> Updated;
    static public EventHandler<bool> LateUpdated;

    static public EventHandler<bool> TimerResumed { get; set; }
    static public EventHandler<bool> TimerPaused { get; set; }

    static public bool Paused { get; private set; }
    static public bool FirstFrame { get; private set; }

    static bool lateUpdate;

    void Update()
    {
        if (Paused) { return; }

        lateUpdate = true;

        var dt = Time.deltaTime;
        Updated?.Invoke(null, dt);
    }

    private void LateUpdate()
    {
        if (Paused) { return; }
        if (!lateUpdate) { return; }

        lateUpdate = false;
        LateUpdated?.Invoke(null, false);
    }

    static public void Pause()
    {
        Time.timeScale = 0.0f;
        Paused = true;

        TimerPaused?.Invoke(null, false);
    }

    static public void Resume()
    {
        Time.timeScale = 1.0f;
        Paused = false;

        TimerResumed?.Invoke(null, false);
    }
}
