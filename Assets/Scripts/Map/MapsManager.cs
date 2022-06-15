using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapsManager : MonoBehaviour
{
    static public EventHandler<bool> Initialized { get; set; }

    static public Dictionary<MapName, Map> MapList { get; private set; }

    static public Map CurrentMap { get; private set; }

    private void Awake()
    {
        MapList = new Dictionary<MapName, Map>()
        {
            { MapName.ez_athletic, GetMap(0) },
            { MapName.ez_square, GetMap(1) },
            { MapName.ez_square2, GetMap(2) },
            { MapName.ez_nostalgia, GetMap(3) },
            { MapName.ez_horizon, GetMap(4) },
            { MapName.ez_training, GetMap(5) },
            { MapName.ez_flyer, GetMap(6) },
            { MapName.ez_settingroom, GetMap(7) },
        };

        InactivateAll();

        // check method
        foreach(var map in MapList.Values)
        {
            if (map == null) { Debug.LogError("No map manager"); }
        }

        // - inner function
        Map GetMap(int n)
        {
            return gameObject.transform.GetChild(n).gameObject.GetComponent<Map>();
        }
    }

    static public void Begin(MapName mapName)
    {
        if (CurrentMap != null) { CurrentMap.Shutdown(); }
        if (!MapList.ContainsKey(mapName)) { return; }

        CurrentMap = MapList[mapName];

        InactivateAll();

        CurrentMap.gameObject.SetActive(true);
        CurrentMap.Initialize();

        if (Bools.Get(Bools.Item.write_events)) { CheckPoint.WriteToLog(InvokeCommand.GameEvent.on_map_changed); }
        Initialized?.Invoke(null, false);
    }

    static void InactivateAll()
    {
        foreach (var map in MapList.Values)
        {
            map.gameObject.SetActive(false);
        }
    }
}
