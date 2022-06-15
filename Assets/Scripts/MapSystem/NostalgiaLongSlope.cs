using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NostalgiaLongSlope : MonoBehaviour
{
    static readonly Bools.Item item = Bools.Item.show_roof;

    GameObject body1;
    GameObject body2;

    private void Awake()
    {
        body1 = gameObject.transform.GetChild(0).gameObject;
        body2 = gameObject.transform.GetChild(1).gameObject;
    }

    void Start()
    {
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
        var active = Bools.Get(item);

        body1.SetActive(active);
        body2.SetActive(active);
    }
}
