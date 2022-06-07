using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] Map map;
    [SerializeField] int index;

    private void Start()
    {
#if UNITY_EDITOR
        if (map == null)
        {
            Debug.LogWarning("No map set");
        }
#endif
    }

    private void OnTriggerStay(Collider other)
    {
        if (map == null) { return; }

        map.SetIndex(index);
    }
}
