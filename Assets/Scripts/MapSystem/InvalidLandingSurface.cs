using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvalidLandingSurface : MonoBehaviour
{
    static readonly int counterLimit = 5;

    [SerializeField] InvalidArea invalidArea;

    int counter = 0;

    private void Awake()
    {
        if (invalidArea == null)
        {
            Debug.LogWarning("No Invalid Area");
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
                var respawn = invalidArea.respawnPosition;

                PM_Main.ResetPosition(respawn.transform.position, respawn.transform.eulerAngles.y);
                InvalidArea.CourseOut?.Invoke(null, position);
            }   
        } 
    }
}
