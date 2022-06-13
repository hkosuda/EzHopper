using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameTimer : MonoBehaviour
{
    static public EventHandler<float> Updated { get; set; }
    static public EventHandler<bool> LateUpdated { get; set; }

    static public EventHandler<bool> TimerResumed { get; set; }
    static public EventHandler<bool> TimerPaused { get; set; }

    static public float ActiveTime { get; private set; } = 0.0f;
    static public int Frame { get; private set; } = 0;

    static public bool Paused { get; private set; }

    static bool lateUpdate;

    private void Start()
    {
        ActiveTime = 0.0f;
    }

    void Update()
    {
        if (Paused) { return; }

        lateUpdate = true;

        var dt = Time.deltaTime;
        ActiveTime += dt;
        Frame++;

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
        var prev = Paused;

        Time.timeScale = 0.0f;
        Paused = true;

        if (prev != Paused)
        {
            TimerPaused?.Invoke(null, false);
        }
    }

    static public void Resume()
    {
        var prev = Paused;

        Time.timeScale = 1.0f;
        Paused = false;

        if (prev != Paused)
        {
            TimerResumed?.Invoke(null, false);
        }
    }
}
