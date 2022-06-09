using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sq2BarColor : MonoBehaviour
{
    static readonly Dictionary<Direction, float> directionToIndicator = new Dictionary<Direction, float>()
    {
        { Direction.z_positive, 0.0f },
        { Direction.x_positive, 1.0f },
        { Direction.z_negative, 2.0f },
        { Direction.x_negative, 3.0f },
        { Direction.all, 4.0f },
    };

    enum Direction
    {
        z_positive,
        x_positive,
        z_negative,
        x_negative,
        all,
    }

    [SerializeField] Direction direction = Direction.all;

    private void Start()
    {
        SetMaterial();
    }

    void SetMaterial()
    {
        var body = gameObject.transform.GetChild(0);
        var mat = body.gameObject.GetComponent<MeshRenderer>().material;

        var parent = gameObject.transform.parent.gameObject;
        var edgeColor = parent.GetComponent<Sq2PileColor>().EdgeColor;

        var lineColor = new Color(0.2f, 0.2f, 0.2f, 1.0f) + edgeColor * 0.5f;

        mat.SetColor("_EdgeColor", edgeColor);
        mat.SetColor("_LineColor", lineColor);
        mat.SetColor("_MainColor", Sq2PileColor.mainColor);

        mat.SetFloat("_EdgeWidth", Sq2PileColor.edgeWidth);
        mat.SetFloat("_LineWidth", Sq2PileColor.lineWidth);
        mat.SetFloat("_LatticeSize", Sq2PileColor.latticeSize);

        mat.SetFloat("_X", body.position.x);
        mat.SetFloat("_Y", body.position.y);
        mat.SetFloat("_Z", body.position.z);

        var size = GetSize(body);

        mat.SetFloat("_PileSizeX", size.x * 0.5f);
        mat.SetFloat("_PileSizeY", size.y * 0.5f);
        mat.SetFloat("_PileSizeZ", size.z * 0.5f);

        mat.SetFloat("_Direction", directionToIndicator[direction]);

        Vector3 GetSize(Transform body)
        {
            var lx = body.lossyScale.x;
            var ly = body.lossyScale.y;
            var lz = body.lossyScale.z;

            var rot = gameObject.transform.eulerAngles.y * Mathf.Deg2Rad;

            var z = Mathf.Abs(lz * Mathf.Cos(rot) - lx * Mathf.Sin(rot));
            var x = Mathf.Abs(lz * Mathf.Sin(rot) + lx * Mathf.Cos(rot));

            return new Vector3(x, ly, z);
        }
    }
}
