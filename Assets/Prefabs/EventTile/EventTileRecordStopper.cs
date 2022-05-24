using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTileRecordStopper : MonoBehaviour
{
    void Start()
    {
        var eventTile = gameObject.GetComponent<EventTile>();
        eventTile.SetReaction(FinishRecording);
    }

    void FinishRecording()
    {
        PlayerRecorder.FinishRecording(true, false);
    }
}
