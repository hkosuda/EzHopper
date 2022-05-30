using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Pilename : MonoBehaviour
{
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (Vector3.Distance(transform.position, SceneView.currentDrawingSceneView.camera.transform.position) > 100.0f) { return; }

        var parent = gameObject.transform.parent;
        if (parent == null) { return; }

        var index = gameObject.transform.GetSiblingIndex();
        var name = "Pile_" + PaddingZero(index);

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
