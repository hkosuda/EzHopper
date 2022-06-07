using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSystem : MonoBehaviour
{
    static public bool Active { get; private set; }

    private void Awake()
    {
        Active = true;
    }

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
        if (indicator > 0)
        {
            Timer.TimerPaused += InactivateOnTimerPaused;
            Timer.TimerResumed += ActivateOnTimerResumed;
        }

        else
        {
            Timer.TimerPaused -= InactivateOnTimerPaused;
            Timer.TimerResumed -= ActivateOnTimerResumed;
        }
    }

    private void Update()
    {
        if (Timer.Paused)
        {
            Active = false;
        }
    }

    static void InactivateOnTimerPaused(object obj, bool mute)
    {
        Inactivate();
    }

    static void ActivateOnTimerResumed(object obj, bool mute)
    {
        Activate();
    }


    static public bool CheckInput(Keyconfig.Key key, bool getKeyDown)
    {
        if (!Active) { return false; }

        
        if (key.keyCode != KeyCode.None)
        {
            if (getKeyDown)
            {
                if (Input.GetKeyDown(key.keyCode))
                {
                    return true;
                }
            }

            else
            {
                if (Input.GetKey(key.keyCode))
                {
                    return true;
                }
            }
        }

        // keybind.keyCode == KeyCode.None
        else
        {
            float wheelDelta = key.wheelDelta;

            if (wheelDelta > 0)
            {
                if (Input.mouseScrollDelta.y > 0)
                {
                    return true;
                }
            }

            else
            {
                if (Input.mouseScrollDelta.y < 0)
                {
                    return true;
                }
            }
        }

        return false;
    }

    static public void Activate()
    {
        Active = true;
    }

    static public void Inactivate()
    {
        Active = false;
    }
}
