using UnityEngine;
using UnityEditor;

public class LiftingTile : MonoBehaviour
{
#if UNITY_EDITOR
    public GameObject targetTile;
    public float liftingRate = 0.0f;

    private void OnDrawGizmos()
    { 
        if (targetTile == null) { return; }

        var p0 = gameObject.transform.position;
        var p1 = targetTile.transform.position;
        var pp = new Vector3(p0.x, p1.y, p0.z);

        var len = (new Vector2(p0.x, p0.z) - new Vector2(p1.x, p1.z)).magnitude;
        var y = p1.y + liftingRate * len;

        gameObject.transform.position = new Vector3(p0.x, y, p0.z);

        Handles.color = new Color(1.0f, 0.0f, 0.0f);
        Handles.DrawLine(p1, pp, 1.0f);
        Handles.DrawLine(pp, p0, 1.0f);
    }
#endif
}
