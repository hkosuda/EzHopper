using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoJumpSign : MonoBehaviour
{
    static public GameObject textobject;
    static bool prevValue;

    void Start()
    {
        textobject = gameObject.transform.GetChild(0).gameObject;

        prevValue = PM_Jumping.AutoJump;
        textobject.SetActive(prevValue);
    }

    void Update()
    {
        if (prevValue != PM_Jumping.AutoJump)
        {
            prevValue = PM_Jumping.AutoJump;
            textobject.SetActive(prevValue);
        }
    }
}
