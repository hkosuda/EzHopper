using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrontView : MonoBehaviour
{
    static readonly Bools.Item item = Bools.Item.left_hand;

    static Material material;

    private void Awake()
    {
        material = gameObject.GetComponent<RawImage>().material;
    }

    void Start()
    {
        UpdateMaterial(null, false);
        SetEvent(1);
    }

    private void OnDestroy()
    {
        SetEvent(-1);
    }

    static void SetEvent(int indicator)
    {
        if (indicator > 0)
        {
            Bools.Settings[item].ValueUpdated += UpdateMaterial;
        }

        else
        {
            Bools.Settings[item].ValueUpdated -= UpdateMaterial;
        }
    }

    static void UpdateMaterial(object obj, bool prev)
    {
        if (Bools.Get(item))
        {
            material.SetFloat("_LeftHand", 1.0f);
        }

        else
        {
            material.SetFloat("_LeftHand", -1.0f);
        }
    }
}
