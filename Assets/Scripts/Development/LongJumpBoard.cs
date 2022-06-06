using UnityEngine;
using UnityEditor;

public class LongJumpBoard : MonoBehaviour
{
#if UNITY_EDITOR
    public GameObject baseTile;

    private void OnDrawGizmos()
    {
        if (Vector3.Distance(transform.position, SceneView.currentDrawingSceneView.camera.transform.position) > 50.0f) { return; }
        if (baseTile == null) { return; }

        var p0 = gameObject.transform.position;
        var p1 = baseTile.transform.position;

        var pp0 = p0;
        var pp1 = new Vector3(p1.x, p0.y, p0.z);

        Handles.DrawLine(pp0, pp1, 1.0f);
        Handles.Label((pp0 + pp1) * 0.5f, (pp0 - pp1).magnitude.ToString("f3"));

        // rename
        var parent = gameObject.transform.parent;
        if (parent == null) { return; }

        var idx = gameObject.transform.GetSiblingIndex();
        gameObject.name = "LongJumpTile_" + idx.ToString();
    }
#endif
}
