using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapName
{
    ez_athletic,
    ez_nostalgia,
    ez_training,
    ez_freefall,
    ez_flyer,
}

public abstract class Map : MonoBehaviour
{
    public GameObject[] respawnPositions = new GameObject[1];

    public MapName MapName { get; protected set; }

    public virtual void Initialize() 
    {
        var tr = respawnPositions[0].transform;
        PM_Main.Initialize(tr.position, tr.eulerAngles.y);
    }

    public virtual void Shutdown() { }

    static bool CheckRespawnPositions(GameObject[] respawnPositions)
    {
        if (respawnPositions == null || respawnPositions.Length == 0)
        {
            return false;
        }

        return true;
    }

    public void Next()
    {

    }

    public void Prev()
    {

    }
}
