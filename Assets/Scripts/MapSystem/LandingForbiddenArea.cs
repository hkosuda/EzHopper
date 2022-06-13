using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class LandingForbiddenArea : MonoBehaviour
{
    readonly int counterLimit = 2;
    bool flag;
    
    private void Awake()
    {
        gameObject.layer = 2;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (InGameTimer.Paused) { return; }
        flag = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (InGameTimer.Paused) { return; }
        flag = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (InGameTimer.Paused) { return; }

        if (PM_Landing.LandingIndicator > 0)
        {
            if (flag)
            {
                flag = false;

                if (Bools.Get(Bools.Item.write_events))
                {
                    CheckPoint.WriteToLog(InvokeCommand.GameEvent.on_course_out);
                }

                var position = PM_Main.Myself.transform.position;
                InvalidArea.CourseOut?.Invoke(null, position);
            }
        }
    }
}
