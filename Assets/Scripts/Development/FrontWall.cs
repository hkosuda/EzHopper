using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FrontWall : MonoBehaviour
{
#if UNITY_EDITOR
    public GameObject pile1;
    public GameObject pile2;

    private void OnDrawGizmos()
    {
        if (pile1 == null || pile2 == null) { return; }

        var midZ = (pile1.transform.position.z + pile2.transform.position.z) * 0.5f;

        var p = gameObject.transform.position;
        gameObject.transform.position = new Vector3(p.x, p.y, midZ);

        Handles.color = new Color(1.0f, 1.0f, 0.0f);
        Handles.DrawLine(gameObject.transform.position, pile1.transform.position);
        Handles.DrawLine(gameObject.transform.position, pile2.transform.position);
    }
#endif
}
