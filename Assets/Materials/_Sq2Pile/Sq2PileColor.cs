using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sq2PileColor : MonoBehaviour
{
    static readonly List<Color> colorList = new List<Color>()
    {
        new Color(1.0f, 0.0f, 0.0f),
        new Color(0.0f, 1.0f, 0.0f),
        new Color(0.0f, 0.0f, 1.0f),
        new Color(1.0f, 1.0f, 0.0f),
        new Color(0.0f, 1.0f, 1.0f),
        new Color(1.0f, 0.0f, 1.0f),
    };

    static public readonly Color mainColor = new Color(0.2f, 0.2f, 0.2f, 1.0f);
    static public readonly Color lineColor = new Color(0.4f, 0.4f, 0.4f, 1.0f);

    static public readonly float edgeWidth = 0.04f;
    static public readonly float lineWidth = 0.01f;
    static public readonly float latticeSize = 0.5f;

    public Color EdgeColor { get; private set; } = new Color(1.0f, 1.0f, 1.0f, 1.0f);

    private void Awake()
    {
        var edgeColor = GetColor(gameObject);
        EdgeColor = edgeColor;

        SetMaterial();
    }

    static Color GetColor(GameObject gameObject)
    {
        var index = gameObject.transform.GetSiblingIndex();
        var colorIndex = index % colorList.Count;

        return colorList[colorIndex];
    }

    void SetMaterial()
    {
        var body = gameObject.transform.GetChild(0);
        var mat = body.gameObject.GetComponent<MeshRenderer>().material;

        var lineColor = new Color(0.2f, 0.2f, 0.2f, 1.0f) + EdgeColor * 0.5f;

        mat.SetColor("_EdgeColor", EdgeColor);
        mat.SetColor("_LineColor", lineColor);
        mat.SetColor("_MainColor", mainColor);

        mat.SetFloat("_EdgeWidth", edgeWidth);
        mat.SetFloat("_LineWidth", lineWidth);
        mat.SetFloat("_LatticeSize", latticeSize);

        mat.SetFloat("_X", body.position.x);
        mat.SetFloat("_Z", body.position.z);

        mat.SetFloat("_PileSizeX", body.lossyScale.x * 0.5f);
        mat.SetFloat("_PileSizeZ", body.lossyScale.z * 0.5f);
    }
}
