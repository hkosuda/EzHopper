using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    static public EventHandler<float> Updated;
    static public EventHandler<bool> LateUpdated;

    static public bool Paused { get; private set; }

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
        Paused = true;
    }

    static public void Resume()
    {
        Paused = false;
    }
}
