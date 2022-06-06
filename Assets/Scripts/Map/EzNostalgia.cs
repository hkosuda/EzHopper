using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EzNostalgia : Map 
{
    private void Awake()
    {
        MapName = MapName.ez_nostalgia;
    }

#if UNITY_EDITOR
    public override void Initialize()
    {
        var tr = respawnPositions[6].transform;
        PM_Main.Initialize(tr.position, tr.eulerAngles.y);
    }
#endif
}
