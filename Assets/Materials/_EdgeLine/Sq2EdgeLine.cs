using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sq2EdgeLine : MonoBehaviour
{
    public Color edgeColor = new Color(0.0f, 1.0f, 1.0f, 1.0f);
    public float distance = 100.0f;

    static readonly float edgeWidth = 0.04f;
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
        var material = gameObject.GetComponent<MeshRenderer>().material;

        material.SetColor("_EdgeColor", edgeColor);
        material.SetColor("_LineColor", lineColor);
        material.SetColor("_MainColor", mainColor);

        material.SetFloat("_EdgeWidth", edgeWidth);
        material.SetFloat("_LineWidth", lineWidth);
        material.SetFloat("_LatticeSize", latticeSize);

        material.SetFloat("_X", gameObject.transform.position.x);
        material.SetFloat("_Y", gameObject.transform.position.y + gameObject.transform.lossyScale.y);
        material.SetFloat("_Z", gameObject.transform.position.z);

        material.SetFloat("_TileSizeX", gameObject.transform.lossyScale.x * 0.5f);
        material.SetFloat("_TileSizeZ", gameObject.transform.lossyScale.z * 0.5f);

        material.SetFloat("_RotY", gameObject.transform.eulerAngles.y * Mathf.Deg2Rad);
    }
}
