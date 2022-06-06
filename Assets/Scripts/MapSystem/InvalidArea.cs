using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class InvalidArea : MonoBehaviour
{
    static public EventHandler<Vector3> CourseOut { get; set; }

    [SerializeField] bool active = true;
    public GameObject respawnPosition;

    private void Start()
    {
#if UNITY_EDITOR
        if (respawnPosition == null)
        {
            Debug.LogError("No Respawn Position");
        }
#endif 
    }

    private void OnTriggerStay(Collider other)
    {
        if (active)
        {
            var position = PM_Main.Myself.transform.position;

            PM_Main.ResetPosition(respawnPosition.transform.position, respawnPosition.transform.eulerAngles.y);
            CourseOut?.Invoke(null, position);
        }
    }

    public void SetRespawnPosition(GameObject _respawnPosition)
    {
        respawnPosition = _respawnPosition;
    }

    // - inner function
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        var disp = Vector3.Distance(transform.position, SceneView.currentDrawingSceneView.camera.transform.position);

        if (disp < 150.0f) { return; }
        if (!active) { return; }
        if (respawnPosition == null) { return; }

        var p0 = gameObject.transform.position;
        var p1 = respawnPosition.transform.position;

        Handles.color = new Color(0.0f, 1.0f, 0.0f);
        Handles.DrawLine(p0, p1);
    }
#endif
}
