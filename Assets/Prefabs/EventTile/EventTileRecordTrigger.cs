using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTileRecordTrigger : MonoBehaviour
{
    void Start()
    {
        var eventTile = gameObject.GetComponent<EventTile>();
        eventTile.SetReaction(StartRecording);
    }

    void StartRecording()
    {
        PlayerRecorder.BeginRecording();
    }
}
