using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EzSquare : Map
{
    private void Awake()
    {
        MapName = MapName.ez_square;
    }

#if UNITY_EDITOR
    public override void Initialize()
    {
        var tr = respawnPositions[0].transform;
        PM_Main.Initialize(tr.position, tr.eulerAngles.y);
    }
#endif
}
