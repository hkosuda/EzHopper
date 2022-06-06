using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapName
{
    ez_athletic,
    ez_square,
    ez_nostalgia,
    ez_horizon,
    ez_training,
    ez_freefall,
    ez_flyer,
}

public abstract class Map : MonoBehaviour
{
    int index = 0;

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
        index = (index + 1) % respawnPositions.Length;

        var pos = respawnPositions[index].transform.position;
        var rot = respawnPositions[index].transform.eulerAngles;

        PM_Main.Initialize(pos, rot.y);

    }

    public void Prev()
    {
        index -= 1;
        if (index < 0) { index = respawnPositions.Length - 1; }

        var pos = respawnPositions[index].transform.position;
        var rot = respawnPositions[index].transform.eulerAngles;

        PM_Main.Initialize(pos, rot.y);
    }
}
