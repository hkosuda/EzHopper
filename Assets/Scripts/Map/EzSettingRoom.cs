using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EzSettingRoom : Map
{
    static GameObject _ui;
    static GameObject ui;

    private void Awake()
    {
        MapName = MapName.ez_settingroom;
    }

    public override void Initialize()
    {
        base.Initialize();

        _ui = Resources.Load<GameObject>("UI/SettingRoomUI");
        ui = Instantiate(_ui);
    }

    public override void Shutdown()
    {
        base.Shutdown();

        if (ui != null) { Destroy(ui); }
    }
}
