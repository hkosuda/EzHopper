using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugKeyDisplay : MonoBehaviour
{
    static Dictionary<ReplayKey.Key, ReplayKey> keyList;

    private void Awake()
    {
        keyList = new Dictionary<ReplayKey.Key, ReplayKey>()
        {
            { ReplayKey.Key.f, GetKey(1) },
            { ReplayKey.Key.b, GetKey(2) },
            { ReplayKey.Key.r, GetKey(3) },
            { ReplayKey.Key.l, GetKey(4) },
        };

        // - inner function
        ReplayKey GetKey(int n)
        {
            return gameObject.transform.GetChild(n).gameObject.GetComponent<ReplayKey>();
        }
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
            InGameTimer.Updated += UpdateMethod;
        }

        else
        {
            InGameTimer.Updated -= UpdateMethod;
        }
    }

    static void UpdateMethod(object obj, float dt)
    {
        var values = GetValues();
        SetValues(values);
    }

    static float[] GetValues()
    {
        var f = InputSystem.CheckInput(Keyconfig.KeybindList[Keyconfig.KeyAction.forward], false);
        var b = InputSystem.CheckInput(Keyconfig.KeybindList[Keyconfig.KeyAction.backward], false);
        var r = InputSystem.CheckInput(Keyconfig.KeybindList[Keyconfig.KeyAction.right], false);
        var l = InputSystem.CheckInput(Keyconfig.KeybindList[Keyconfig.KeyAction.left], false);

        return new float[4] { F(f), F(b), F(r), F(l) };

        // 
        static float F(bool v)
        {
            if (v) { return 1.0f; }
            return 0.0f;
        }
    }

    static public void SetValues(float[] values)
    {
        keyList[ReplayKey.Key.f].SetStatus(values[0] > 0.5f);
        keyList[ReplayKey.Key.b].SetStatus(values[1] > 0.5f);
        keyList[ReplayKey.Key.r].SetStatus(values[2] > 0.5f);
        keyList[ReplayKey.Key.l].SetStatus(values[3] > 0.5f);
    }
}
