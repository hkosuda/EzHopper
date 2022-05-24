using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    void Awake()
    {
        var mat = gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material;

        mat.SetColor("_LineColor", new Color(0.0f, 0.5f, 1.0f));
        mat.SetColor("_MainColor", new Color(0.5f, 0.5f, 0.5f));

        mat.SetFloat("_X", gameObject.transform.position.x);
        mat.SetFloat("_Z", gameObject.transform.position.z);

        mat.SetFloat("_Width", 0.2f);
        mat.SetFloat("_Size", gameObject.transform.localScale.x * 0.5f);
    }
}
