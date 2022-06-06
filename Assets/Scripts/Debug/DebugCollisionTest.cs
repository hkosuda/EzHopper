using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCollisionTest : MonoBehaviour
{
    Material mat;

    private void Start()
    {
        mat = gameObject.GetComponent<MeshRenderer>().material;
    }

    private void OnCollisionStay(Collision collision)
    {
        mat.color = new Color(1.0f, 1.0f, 1.0f);
    }
}
