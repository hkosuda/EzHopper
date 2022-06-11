using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractivePilesDemoSwitch : MonoBehaviour
{
    void Start()
    {
        var interactive = gameObject.GetComponent<InteractiveObject>();
        interactive.SetAction(RequestCommand);
    }

    void RequestCommand()
    {
        CommandReceiver.RequestCommand("demo ez_athletic_piles -mute");
    }
}
