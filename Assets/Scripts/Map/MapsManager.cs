using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapsManager : MonoBehaviour
{
    static public Dictionary<MapName, Map> MapList { get; private set; }

    static public Map CurrentMap { get; private set; }

    private void Awake()
    {
        MapList = new Dictionary<MapName, Map>()
        {
            { MapName.ez_athletic, GetMap(0) },
            { MapName.ez_nostalgia, GetMap(1) },
            { MapName.ez_training, GetMap(2) },
            { MapName.ez_freefall, GetMap(3) },
            { MapName.ez_flyer, GetMap(4) },
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

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    static public void Begin(MapName mapName)
    {
        if (CurrentMap != null) { CurrentMap.Shutdown(); }
        CurrentMap = MapList[mapName];

        InactivateAll();

        CurrentMap.gameObject.SetActive(true);
        CurrentMap.Initialize();
    }

    static void InactivateAll()
    {
        foreach (var map in MapList.Values)
        {
            map.gameObject.SetActive(false);
        }
    }
}