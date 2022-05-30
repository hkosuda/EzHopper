using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    static MapName initialMap = MapName.ez_athletic; 

    private void Awake()
    {
        Kernel.Initialize();
    }

    void Start()
    {
        Kernel.Reset();
        MapsManager.Begin(initialMap);
    }

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            //PM_Jumping.AutoJump = true;
        }
    }
}
