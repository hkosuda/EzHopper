using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Sq2Pile : MonoBehaviour
{
#if UNITY_EDITOR
    public float floorHeight = -1.0f;
    public float roofHeight = 10.0f;

    private void OnDrawGizmos()
    {
        var pos = gameObject.transform.position;
        gameObject.transform.position = new Vector3(pos.x, floorHeight, pos.z);

        var child = gameObject.transform.GetChild(0);
        var y = child.transform.localScale.y;

        var p = child.localPosition;
        child.localPosition = new Vector3(p.x, y * 0.5f, p.z);
        child.localScale = new Vector3(0.5f, roofHeight, 0.5f);
    }
#endif
}
