using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DE_Main : MonoBehaviour
{
    static List<ControllerComponent> controllers;

    private void Awake()
    {
        controllers = new List<ControllerComponent>()
        {
            new DE_Availability(),
            new DE_Shooter(),
        };
    }

    void Start()
    {
        foreach(var controller in controllers)
        {
            controller.Initialize();
        }

        SetEvent(1);
    }

    private void OnDestroy()
    {
        foreach(var controller in controllers)
        {
            controller.Shutdown();
        }

        SetEvent(-1);
    }

    void SetEvent(int indicator)
    {
        if (indicator > 0)
        {
            Timer.Updated += UpdateMethod;
        }

        else
        {
            Timer.Updated -= UpdateMethod;
        }
    }

    void UpdateMethod(object obj, float dt)
    {
        foreach(var controller in controllers)
        {
            controller.Update(dt);
        }
    }
}