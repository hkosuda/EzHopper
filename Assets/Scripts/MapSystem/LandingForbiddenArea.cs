using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingForbiddenArea : MonoBehaviour
{
    readonly int counterLimit = 2;
    int counter = 0;

    private void Start()
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
            InvalidArea.CourseOut += ResetCounter;
        }

        else
        {
            InvalidArea.CourseOut -= ResetCounter;
        }
    }

    void ResetCounter(object obj, Vector3 pos)
    {
        counter = 0;
    }

    private void OnTriggerStay(Collider other)
    {
        if (Timer.Paused) { return; }

        if (PM_Landing.LandingIndicator > 0)
        {
            counter++;

            if (counter > counterLimit)
            {
                var position = PM_Main.Myself.transform.position;
                InvalidArea.CourseOut?.Invoke(null, position);
            }
        }
    }
}
