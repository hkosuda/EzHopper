using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapName
{
    ez_athletic,
    ez_square,
    ez_square2,
    ez_nostalgia,
    ez_horizon,
    ez_training,
    ez_flyer,
    ez_settingroom,

    none,
}

public abstract class Map : MonoBehaviour
{
    public int Index { get; private set; }

    public GameObject[] respawnPositions = new GameObject[1];

    public MapName MapName { get; protected set; }

    public virtual void Initialize() 
    {
        var tr = respawnPositions[0].transform;
        PM_Main.Initialize(tr.position, tr.eulerAngles.y);
    }

    public virtual void Shutdown() { }

    public void Next()
    {
        Index = (Index + 1) % respawnPositions.Length;

        var pos = respawnPositions[Index].transform.position;
        var rot = respawnPositions[Index].transform.eulerAngles;

        PM_Main.Initialize(pos, rot.y);

    }

    public void Prev()
    {
        Index -= 1;
        if (Index < 0) { Index = respawnPositions.Length - 1; }

        var pos = respawnPositions[Index].transform.position;
        var rot = respawnPositions[Index].transform.eulerAngles;

        PM_Main.Initialize(pos, rot.y);
    }

    public void Back(int index = -1)
    {
        if (0 <= index && index < respawnPositions.Length)
        {
            Index = index;
        }

        var pos = respawnPositions[Index].transform.position;
        var rot = respawnPositions[Index].transform.eulerAngles;

        PM_Main.ResetPosition(pos, rot.y);
    }

    public void SetIndex(int Index)
    {
        if (Index < 0)
        {
            Index = 0;
        }

        if (Index > respawnPositions.Length - 1)
        {
            Index = respawnPositions.Length - 1;
        }

        this.Index = Index;
    }
}
