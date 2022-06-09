using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Sq2PileHeight : MonoBehaviour
{
    [SerializeField] float distance = 30.0f;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (Vector3.Distance(transform.position, SceneView.currentDrawingSceneView.camera.transform.position) > distance) { return; }

        Handles.color = new Color(0.0f, 1.0f, 1.0f);

        var child = gameObject.transform.GetChild(0);
        var y = child.lossyScale.y;

        var p0 = gameObject.transform.position;
        var p1 = p0 + new Vector3(0.0f, y, 0.0f);

        Handles.Label((p0 + p1) * 0.5f, "Y:" + y.ToString("f1"));
        Handles.DrawLine(p0, p1);
    }
#endif
}
