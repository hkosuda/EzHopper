using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PM_Main : MonoBehaviour
{
    

    static public readonly float centerY = 0.9f;
    static public readonly float playerRadius = 0.5f;

    static public GameObject Myself { get; private set; }
    static public Rigidbody Rb { get; private set; }
    

    static List<ControllerComponent> ControllerComponents;

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

            new PM_PostProcessor(),
        };
    }

    void Start()
    {
        foreach(var component in ControllerComponents)
        {
            component.Initialize();
        }

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
        }

        else
        {
            Timer.Updated -= UpdateMethod;
            Timer.LateUpdated -= LateUpdateMethod;
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

    static public void RotVelocity(float degRotY)
    {
        var rot = degRotY * Mathf.Deg2Rad;
        var v = Rb.velocity;

        var vz = v.z * Mathf.Cos(rot) - v.x * Mathf.Sin(rot);
        var vx = v.z * Mathf.Sin(rot) + v.x * Mathf.Cos(rot);

        Rb.velocity = new Vector3(vx, v.y, vz);
    }
}

