using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EZ_Rotation : MonoBehaviour
{
    static readonly float rotationSpeed = 45.0f;

    float rotY;

    void Start()
    {
        SetEvent(1);
    }

    private void OnDestroy()
    {
        SetEvent(-1);
    }

    void SetEvent(int indicator)
    {
        if (indicator > 0)
        {
            InGameTimer.Updated += UpdateMethod;
        }

        else
        {
            InGameTimer.Updated -= UpdateMethod;
        }
    }

    void UpdateMethod(object obj, float dt)
    {
        rotY += rotationSpeed * dt;

        gameObject.transform.eulerAngles = new Vector3(0.0f, rotY, 0.0f);
    }
}
