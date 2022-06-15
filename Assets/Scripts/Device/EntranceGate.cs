using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EntranceGate : MonoBehaviour
{
    [SerializeField] GameObject exitGate;

    GameObject parent;

    private void Awake()
    {
        parent = gameObject.transform.parent.parent.gameObject;
    }

    private void Start()
    {
#if UNITY_EDITOR
        if (exitGate == null)
        {
            Debug.LogError("No Exit Gate");
        }
#endif
    }

    private void OnTriggerStay(Collider other)
    {
        var entranceRotY = parent.transform.eulerAngles.y;
        var exitRotY = exitGate.transform.eulerAngles.y;

        var deltaRotY = exitRotY - entranceRotY + 180.0f;

        PM_Camera.Rotate(deltaRotY);
        PM_Main.RotVelocity(deltaRotY);

        var dy = other.transform.position.y - parent.transform.position.y;
        other.transform.position = ExitPosition(exitGate, dy);
    }

    static Vector3 ExitPosition(GameObject exitGate, float dy)
    {
        var rotY = exitGate.transform.eulerAngles.y * Mathf.Deg2Rad;

        var dx = Mathf.Sin(rotY);
        var dz = Mathf.Cos(rotY);

        return exitGate.transform.position + new Vector3(dx, dy, dz);
    }

    // - unity editor
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (Vector3.Distance(transform.position, SceneView.currentDrawingSceneView.camera.transform.position) > 5000.0f) { return; }
        if (exitGate == null) { return; }

        var p0 = gameObject.transform.position;
        var p1 = exitGate.transform.position;

        Handles.color = new Color(1.0f, 0.0f, 0.0f);
        Handles.DrawLine(p0, p1);
    }
#endif
}
