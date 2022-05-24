using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    static bool autoJump;

    void Start()
    {
        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 45;
    }

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            PM_Jumping.AutoJump = true;
        }
    }
}
