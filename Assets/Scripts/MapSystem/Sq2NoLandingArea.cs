using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sq2NoLandingArea : MonoBehaviour
{
    static readonly int counterLimit = 2;

    [SerializeField] GameObject respawnPosition;
    int counter = 0;

    private void Awake()
    {
        if (respawnPosition == null)
        {
            Debug.LogWarning("No Respawn Position");
        }
    }

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
        if (PM_Landing.LandingIndicator > 0)
        {
            counter++;

            if (counter > counterLimit)
            {
                var position = PM_Main.Myself.transform.position;
                var respawn = respawnPosition;

                PM_Main.ResetPosition(respawn.transform.position, respawn.transform.eulerAngles.y);
                InvalidArea.CourseOut?.Invoke(null, position);
            }
        }
    }
}
