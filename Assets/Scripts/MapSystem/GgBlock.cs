using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GgBlock : MonoBehaviour
{
    static bool ggFlag = true;
    static bool glhfFlag = true;

    static bool initialized = false;

    private void Start()
    {
        SetEvent(1);
    }

    private void OnDestroy()
    {
        SetEvent(-1);
    }

    static void SetEvent(int indicator)
    {
        if (initialized) { return; }
        initialized = true;

        if (indicator > 0)
        {
            MapsManager.Initialized += SetFlag;
            InGameTimer.Updated += UpdateMethod;
        }

        else
        {
            MapsManager.Initialized -= SetFlag;
            InGameTimer.Updated -= UpdateMethod;
        }
    }

    static void UpdateMethod(object obj, float dt)
    {
        if (glhfFlag)
        {
            glhfFlag = false;
            CommandReceiver.RequestCommand("delayedchat 0.5 1.0 glhf -m");
        }
    }

    static void SetFlag(object obj, bool mute)
    {
        ggFlag = true;
        glhfFlag = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (ggFlag)
        {
            ggFlag = false;
            CommandReceiver.RequestCommand("delayedchat 0.5 1.0 gg -m");
        }
    }
}