using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Timer.Paused)
        {
            Cursor.lockState = CursorLockMode.None;
        }

        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
