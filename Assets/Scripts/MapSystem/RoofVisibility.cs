using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class RoofVisibility : MonoBehaviour
{
    static readonly Bools.Item item = Bools.Item.show_roof;

    MeshRenderer renderer;

    private void Awake()
    {
        renderer = gameObject.GetComponent<MeshRenderer>();
    }

    void Start()
    {
        UpdateVisibility(null, false);
        SetEvent(1);
    }

    private void OnDestroy()
    {
        SetEvent(-1);
    }

    void SetEvent(int indicator)
    {
        if (indicator > 0)
        {
            Bools.Settings[item].ValueUpdated += UpdateVisibility;
        }

        else
        {
            Bools.Settings[item].ValueUpdated -= UpdateVisibility;
        }
    }

    void UpdateVisibility(object obj, bool prev)
    {
        renderer.enabled = Bools.Get(item);
    }
}
