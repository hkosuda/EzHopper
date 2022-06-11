using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpGate : MonoBehaviour
{
    public MapName targetMapName;

    private void OnTriggerStay(Collider other)
    {
        CommandReceiver.RequestCommand("begin " + targetMapName.ToString() + " -mute");
    }
}
