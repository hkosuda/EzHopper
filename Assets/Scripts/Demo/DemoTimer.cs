using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoTimer : MonoBehaviour
{
    static public EventHandler<float> Updated { get; set; }
    static public EventHandler<bool> LateUpdated { get; set; }

    static public bool Paused { get; private set; }

    static bool lateUpdate;

    void Update()
    {
        if (!Timer.Paused) { return; }
        if (Paused) { return; }

        lateUpdate = true;

        var dt = Time.deltaTime;
        Updated?.Invoke(null, dt);
    }

    private void LateUpdate()
    {
        if (!Timer.Paused) { return; }
        if (Paused) { return; }

        if (!lateUpdate) { return; }

        lateUpdate = false;
        LateUpdated?.Invoke(null, false);
    }

    static public void Pause()
    {
        Paused = true;
        Time.timeScale = 0.0f;
    }

    static public void Resume()
    {
        Timer.Pause();
        Paused = false;

        Time.timeScale = 1.0f;
    }
}
