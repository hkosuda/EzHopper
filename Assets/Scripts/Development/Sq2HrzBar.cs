using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Sq2HrzBar : MonoBehaviour
{
#if UNITY_EDITOR
    public float pileSize = 0.5f;
    public float barSize = 0.5f;
    public float barLength = 1.0f;
    public bool noOffset = false;

    private void OnDrawGizmos()
    {
        var child = gameObject.transform.GetChild(0);
        child.localScale = new Vector3(barSize, barSize, barLength);

        var p = child.localPosition;
        var s = child.localScale;

        if (noOffset)
        {
            child.localPosition = new Vector3(p.x, -s.y * 0.5f, s.z * 0.5f - pileSize * 0.5f);
        }

        else
        {
            child.localPosition = new Vector3(p.x, -s.y * 0.5f, s.z * 0.5f + pileSize * 0.5f);
        }
        
    }
#endif
}
