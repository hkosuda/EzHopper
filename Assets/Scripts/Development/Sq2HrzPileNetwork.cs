using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Sq2HrzPileNetwork : MonoBehaviour
{
#if UNITY_EDITOR
    public Color lineColor = new Color(0.0f, 1.0f, 0.0f, 1.0f);

    public GameObject[] targetPiles = new GameObject[1];
    public float distance = 50.0f;

    private void OnDrawGizmos()
    {
        if (Vector3.Distance(transform.position, SceneView.currentDrawingSceneView.camera.transform.position) > distance) { return; }
        if (targetPiles == null) { return; }

        foreach (var pile in targetPiles)
        {
            if (pile == null) { continue; }

            var p0 = gameObject.transform.GetChild(0).position;
            var p1 = pile.transform.GetChild(0).position;
            var pp = new Vector3(p0.x, p1.y, p0.z);

            var len = (new Vector2(p0.x, p0.z) - new Vector2(p1.x, p1.z)).magnitude;
            var dy = Mathf.Abs(p0.y - p1.y);
            var mid = (pp + p1) * 0.5f;

            Handles.color = lineColor;
            Handles.Label(pp, "dX : " + len.ToString("f2") + "\ndY : " + dy.ToString("f2"));
            Handles.DrawLine(p0, pp, 1.0f);
            Handles.DrawLine(pp, p1, 1.0f);
        }
    }

#endif
}
