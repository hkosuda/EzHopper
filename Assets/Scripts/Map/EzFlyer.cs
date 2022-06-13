using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EzFlyer : Map
{
    static GameObject _ui;
    static GameObject ui;

    public override void Initialize()
    {
        base.Initialize();

        SetUI();
        SetEvent(1);
    }

    public override void Shutdown()
    {
        base.Shutdown();

        DeleteUI();
        SetEvent(-1);
    }

    void SetEvent(int indicator)
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

    void UpdateMethod(object obj, float dt)
    {
        if (PM_Main.Myself.transform.position.magnitude > 2000.0f)
        {
            PM_Main.ResetPosition(respawnPositions[0].transform.position);
        }
    }

    static void SetUI()
    {
        if (_ui == null) { _ui = Resources.Load<GameObject>("UI/UiFlyer"); }
        ui = Instantiate(_ui);
    }

    static void DeleteUI()
    {
        if (ui != null)
        {
            Destroy(ui);
        }
    }
}
