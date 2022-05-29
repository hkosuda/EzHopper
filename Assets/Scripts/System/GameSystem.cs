using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    static bool autoJump;

    private void Awake()
    {
        Kernel.Initialize();
    }

    void Start()
    {
        Kernel.Reset();
        
    }

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            PM_Jumping.AutoJump = true;
        }
    }
}
