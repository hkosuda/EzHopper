using System;
using UnityEngine;

public class InvalidArea : MonoBehaviour
{
    static public EventHandler<Vector3> CourseOut { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        if (Timer.Paused) { return; }

        var position = PM_Main.Myself.transform.position;
        CourseOut?.Invoke(null, position);
    }
}
