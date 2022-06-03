using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PM_Main : MonoBehaviour
{
    static public EventHandler<bool> Initialized { get; set; }

    static public readonly float centerY = 0.9f;
    static public readonly float playerRadius = 0.5f;

    static public GameObject Myself { get; private set; }
    static public Rigidbody Rb { get; private set; }

    static List<ControllerComponent> ControllerComponents;
    static ControllerComponent pmDemo;

    static public bool Suspend { get; set; }

    private void Awake()
    {
        Myself = gameObject;
        Rb = gameObject.GetComponent<Rigidbody>();

        ControllerComponents = new List<ControllerComponent>()
        {
            new PM_Demo(),
            new PM_Camera(),

            new PM_Landing(),
            new PM_Jumping(),

            new PM_InputVector(),
            new PM_PlaneVector(),
            new PM_ClipVector(),

            new PM_PostProcessor(),
        };

        pmDemo = new PM_Demo();
        
        foreach (var component in ControllerComponents)
        {
            component.Initialize();
        }
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
            Timer.Updated += UpdateMethod;
            Timer.LateUpdated += LateUpdateMethod;

            DemoTimer.Updated += DemoUpdateMethod;
        }

        else
        {
            Timer.Updated -= UpdateMethod;
            Timer.LateUpdated -= LateUpdateMethod;

            DemoTimer.Updated -= DemoUpdateMethod;
        }
    }

    static void UpdateMethod(object obj, float dt)
    {
        foreach (var component in ControllerComponents)
        {
            component.Update(dt);
        }
    }

    static void LateUpdateMethod(object obj, bool mute)
    {
        foreach (var component in ControllerComponents)
        {
            component.LateUpdate();
        }
    }

    static void DemoUpdateMethod(object obj, float dt)
    {
        pmDemo.Update(dt);
    }

    static public void RotVelocity(float degRotY)
    {
        var rot = degRotY * Mathf.Deg2Rad;
        var v = Rb.velocity;

        var vz = v.z * Mathf.Cos(rot) - v.x * Mathf.Sin(rot);
        var vx = v.z * Mathf.Sin(rot) + v.x * Mathf.Cos(rot);

        Rb.velocity = new Vector3(vx, v.y, vz);
    }

    static public void Initialize(Vector3 position, float degRotY = 0.0f)
    {
        Myself.transform.position = position;
        PM_Camera.SetEulerAngles(new Vector3(0.0f, degRotY, 0.0f));

        PM_Jumping.InactivateAutoJump();

        Rb.velocity = Vector3.zero;
        Initialized?.Invoke(null, false);
    }

    static public void ResetPosition(Vector3 position, float degRotY = 0.0f)
    {
        Myself.transform.position = position;
        PM_Camera.SetEulerAngles(new Vector3(0.0f, degRotY, 0.0f));

        PM_Jumping.InactivateAutoJump();
        Rb.velocity = Vector3.zero;
    }
}

