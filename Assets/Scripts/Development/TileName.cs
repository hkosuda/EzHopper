using UnityEngine;
using UnityEditor;

public class TileName : MonoBehaviour
{
#if UNITY_EDITOR
    public float distance = 50.0f;

    private void OnDrawGizmos()
    {
        if (Vector3.Distance(transform.position, SceneView.currentDrawingSceneView.camera.transform.position) > distance) { return; }

        var parent = gameObject.transform.parent;
        if (parent == null) { return; }

        var index = gameObject.transform.GetSiblingIndex();
        var name = "Tile_" + PaddingZero(index);

        gameObject.name = name;

        Handles.color = new Color(0.0f, 0.0f, 1.0f);
        Handles.Label(gameObject.transform.position, name);
        Handles.color = new Color(0.0f, 0.0f, 1.0f);

        // - inner function
        static string PaddingZero(int num)
        {
            if (num < 10) { return "00" + num.ToString(); }
            if (num < 100) { return "0" + num.ToString(); }

            return num.ToString();
        }
    }
#endif
}

