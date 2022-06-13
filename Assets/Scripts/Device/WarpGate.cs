using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class WarpGate : MonoBehaviour
{
    public MapName targetMapName;

    private void Awake()
    {
        var collider = gameObject.GetComponent<BoxCollider>();
        collider.isTrigger = true;
    }

    private void OnTriggerStay(Collider other)
    {
        CommandReceiver.RequestCommand("begin " + targetMapName.ToString() + " -mute");
    }
}
