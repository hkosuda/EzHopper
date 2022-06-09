using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Sq2HrzBarHeight : MonoBehaviour
{
#if UNITY_EDITOR
    public float distance = 20.0f;

    private void OnDrawGizmos()
    {
        if (Vector3.Distance(transform.position, SceneView.currentDrawingSceneView.camera.transform.position) > distance) { return; }

        var rotY = gameObject.transform.eulerAngles.y * Mathf.Deg2Rad;

        var child = gameObject.transform.GetChild(0);
        var len = child.localPosition.z + child.localScale.z * 0.5f;

        var dx = len * Mathf.Sin(rotY);
        var dz = len * Mathf.Cos(rotY);

        var p0 = gameObject.transform.position + new Vector3(dx, 0.0f, dz);
        var p1 = new Vector3(p0.x, 0.0f, p0.z);

        Handles.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        Handles.Label((p0 + p1) * 0.5f, gameObject.transform.position.y.ToString());
        Handles.DrawLine(p0, p1);
    }
#endif 
}
