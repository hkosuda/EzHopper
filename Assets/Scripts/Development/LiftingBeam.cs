using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LiftingBeam : MonoBehaviour
{
    public bool xDirection;
    public float distance = 50.0f;

    public float lifting;
    public GameObject targetBeam;

    private void OnDrawGizmos()
    {
        if (Vector3.Distance(transform.position, SceneView.currentDrawingSceneView.camera.transform.position) > distance) { return; }
        if (targetBeam == null) { return; }

        var p0 = gameObject.transform.position;
        var p1 = targetBeam.transform.position;
        var pp = new Vector3(p0.x, p1.y, p0.z);

        Vector3 pp1;

        if (xDirection)
        {
            pp1 = new Vector3(p1.x, p1.y, p0.z);
        }

        else
        {
            pp1 = new Vector3(p0.x, p1.y, p1.z);
        }

        gameObject.transform.position = new Vector3(p0.x, p1.y + lifting, p0.z);

        Handles.color = new Color(1.0f, 0.0f, 0.0f);
        Handles.DrawLine(pp1, pp, 1.0f);
        Handles.DrawLine(pp, p0, 1.0f);
    }
}
