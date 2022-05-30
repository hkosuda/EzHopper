using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PileNetwork : MonoBehaviour
{
#if UNITY_EDITOR
    public GameObject[] targetPiles = new GameObject[1];

    private void OnDrawGizmos()
    {
        if (Vector3.Distance(transform.position, SceneView.currentDrawingSceneView.camera.transform.position) > 100.0f) { return; }
        if (targetPiles == null) { return; }

        foreach (var pile in targetPiles)
        {
            if (pile == null) { continue; }

            var p0 = gameObject.transform.position;
            var p1 = pile.transform.position;

            var y0 = p0.y + gameObject.transform.localScale.y;
            var y1 = p1.y + pile.transform.localScale.y;

            var pp0 = new Vector3(p0.x, y0, p0.z);
            var pp1 = new Vector3(p1.x, y1, p1.z);

            var len = (new Vector2(pp0.x, pp0.z) - new Vector2(pp1.x, pp1.z)).magnitude;
            var dy = Mathf.Abs(y1 - y0);
            var mid = (pp1 + pp0) * 0.5f;

            Handles.color = new Color(0.0f, 1.0f, 0.0f);
            Handles.Label(mid, "dX : " + len.ToString("f2") + "\ndY : " + dy.ToString("f2"));
            Handles.DrawLine(pp0, pp1, 1.0f);
        }
    }
#endif
}
