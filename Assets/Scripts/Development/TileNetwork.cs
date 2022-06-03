using UnityEngine;
using UnityEditor;

public class TileNetwork : MonoBehaviour
{
#if UNITY_EDITOR
    public GameObject[] targetTiles = new GameObject[1];
    public float distance = 50.0f;

    private void OnDrawGizmos()
    {
        if (Vector3.Distance(transform.position, SceneView.currentDrawingSceneView.camera.transform.position) > distance) { return; }
        if (targetTiles == null) { return; }

        foreach(var tile in targetTiles)
        {
            if (tile == null) { continue; }

            var p0 = gameObject.transform.position;
            var p1 = tile.transform.position;

            var len = (new Vector2(p0.x, p0.z) - new Vector2(p1.x, p1.z)).magnitude;
            var dy = Mathf.Abs(p0.y - p1.y);
            var mid = (p1 + p0) * 0.5f;

            Handles.color = new Color(0.0f, 1.0f, 0.0f);
            Handles.Label(mid, "dX : " + len.ToString("f2") + "\ndY : " + dy.ToString("f2"));
            Handles.DrawLine(p0, p1, 1.0f);
        }
    }
#endif
}
