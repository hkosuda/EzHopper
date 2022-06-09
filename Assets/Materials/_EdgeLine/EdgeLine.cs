using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EdgeLine : MonoBehaviour
{
    public Color edgeColor = new Color(0.0f, 1.0f, 1.0f, 1.0f);
    public float distance = 100.0f;
    public MeshRenderer mesh;

    static readonly float edgeWidth = 0.1f;
    static readonly float lineWidth = 0.01f;
    static readonly float latticeSize = 0.5f;

    static readonly Color lineColor = new Color(0.5f, 0.5f, 0.5f, 1.0f);
    static readonly Color mainColor = new Color(0.1f, 0.1f, 0.1f, 1.0f);

    private void Start()
    {
        InitializeMaterial();
    }

    void InitializeMaterial()
    {
        var material = mesh.material;

        material.SetColor("_EdgeColor", edgeColor);
        material.SetColor("_LineColor", lineColor);
        material.SetColor("_MainColor", mainColor);

        material.SetFloat("_EdgeWidth", edgeWidth);
        material.SetFloat("_LineWidth", lineWidth);
        material.SetFloat("_LatticeSize", latticeSize);

        material.SetFloat("_X", gameObject.transform.position.x);
        material.SetFloat("_Y", gameObject.transform.position.y + gameObject.transform.localScale.y);
        material.SetFloat("_Z", gameObject.transform.position.z);

        material.SetFloat("_TileSizeX", gameObject.transform.localScale.x * 0.5f);
        material.SetFloat("_TileSizeZ", gameObject.transform.localScale.z * 0.5f);

        material.SetFloat("_RotY", gameObject.transform.eulerAngles.y * Mathf.Deg2Rad);
    }
}
