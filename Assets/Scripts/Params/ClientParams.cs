using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientParams
{
    static public EventHandler<bool> ParamsUdpated { get; set; }

    public enum DeColorTheme
    {
        none = 0,
        vivid = 1,
        matt = 2,
        normal = 3,
    }

    static public float MouseSensi { get; private set; } = 1.5f;
    static public DeColorTheme DeTheme { get; private set; }


    static public void SetSensi(float sensi)
    {
        MouseSensi = sensi;
        ParamsUdpated?.Invoke(null, false);
    }

    // used in setting window
    static public string DeNextTheme()
    {
        return NextDeTheme().ToString();
    }

    static public void ChangeDeTheme()
    {
        DeTheme = NextDeTheme();
        ParamsUdpated?.Invoke(null, false);
    }

    static DeColorTheme NextDeTheme()
    {
        var currentIdx = (int)DeTheme;
        var nextIdx = (currentIdx + 1) % 4;

        return (DeColorTheme)nextIdx;
    }
}
