using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPlayerManager : MonoBehaviour
{
    [SerializeField] bool active;

    static GameObject player;

    void Start()
    {
        player = PM_Main.Myself;
    }

    void Update()
    {
        if (!active) { return; }

        if (player.transform.position.y < 0.0f)
        {
            player.transform.position = new Vector3(-15.0f, 0.9f, 15.0f);
        }
    }
}
