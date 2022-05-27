using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoUiKeys : MonoBehaviour
{
    static Dictionary<ReplayKey.Key, ReplayKey> keyList;

    private void Awake()
    {
        keyList = new Dictionary<ReplayKey.Key, ReplayKey>()
        {
            { ReplayKey.Key.f, GetKey(0) },
            { ReplayKey.Key.b, GetKey(1) },
            { ReplayKey.Key.r, GetKey(2) },
            { ReplayKey.Key.l, GetKey(3) },
        };

        // - inner function
        ReplayKey GetKey(int n)
        {
            return gameObject.transform.GetChild(n).gameObject.GetComponent<ReplayKey>();
        }
    }

    private void Update()
    {
        var interpolated = DemoManager.InterpolatedData;

        SetValues(interpolated);
    }

    static public void SetValues(float[] interpolated)
    {
        keyList[ReplayKey.Key.f].SetStatus(interpolated[6] > 0.5f);
        keyList[ReplayKey.Key.b].SetStatus(interpolated[7] > 0.5f);
        keyList[ReplayKey.Key.r].SetStatus(interpolated[8] > 0.5f);
        keyList[ReplayKey.Key.l].SetStatus(interpolated[9] > 0.5f);
    }
}
