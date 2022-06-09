using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LiftPile : MonoBehaviour
{
#if UNITY_EDITOR
    public GameObject targetPile;
    public float lifting = 0.0f;
    public float distance = 100.0f;

    private void OnDrawGizmos()
    {
        if (Vector3.Distance(transform.position, SceneView.currentDrawingSceneView.camera.transform.position) > distance) { return; }
        if (targetPile == null) { return; }

        var p0 = gameObject.transform.position;
        var p1 = targetPile.transform.position;

        var s0 = gameObject.transform.GetChild(0).localScale;
        var s1 = targetPile.transform.GetChild(0).localScale;

        var dy = lifting;

        var y0 = p0.y + s0.y;
        var y1 = p1.y + s1.y;

        var s = y1 + dy - p0.y;

        gameObject.transform.localScale = new Vector3(s0.x, s, s0.z);

        var pp0 = new Vector3(p0.x, y0, p0.z);
        var pp1 = new Vector3(p1.x, y1, p1.z);
        var ppp = new Vector3(p0.x, y1, p0.z);

        Handles.color = new Color(1.0f, 0.0f, 0.0f);
        Handles.DrawLine(pp1, ppp, 1.0f);
        Handles.DrawLine(ppp, pp0, 1.0f);

        var mid = (ppp + pp1) * 0.5f;
        var info = "dL : " + (ppp - p1).magnitude.ToString("f2") + "\ndY : " + dy.ToString("f2");

        Handles.Label(mid, info);
    }
#endif
}
