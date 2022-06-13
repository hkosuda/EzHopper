using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class InvalidArea : MonoBehaviour
{
    static public EventHandler<Vector3> CourseOut { get; set; }

    private void Awake()
    {
        var collider = gameObject.GetComponent<BoxCollider>();
        collider.isTrigger = true;

        // ignore laycast
        gameObject.layer = 2;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (InGameTimer.Paused) { return; }

        if (Bools.Get(Bools.Item.write_events))
        {
            CheckPoint.WriteToLog(InvokeCommand.GameEvent.on_course_out);
        }

        var position = PM_Main.Myself.transform.position;
        CourseOut?.Invoke(null, position);
    }
}
