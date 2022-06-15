using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class CheckPoint : MonoBehaviour
{
    static float sizeOffset = 0.3f;

    static public EventHandler<int> EnterAnotherCheckpoint { get; set; }
    static public EventHandler<Vector3> EnterCheckpoint { get; set; }
    static public EventHandler<Vector3> ExitCheckpoint { get; set; }
    static public EventHandler<Vector3> ExitStart { get; set; }
    static public EventHandler<Vector3> EnterStart { get; set; }
    static public EventHandler<Vector3> EnterGoal { get; set; }

    static MapName currentMapName;

    [SerializeField] Map map;
    [SerializeField] int index = 0;
    [SerializeField] bool isGoal = false;

    float floorHeight = 0.0f;
    bool indexChecked = false;

    bool enterFlag = true;

    private void Awake()
    {
        var collider = gameObject.GetComponent<BoxCollider>();
        collider.isTrigger = true;

        // 2 : ignore laycast
        gameObject.layer = 2;

        var parent = gameObject.transform.parent;

        if (parent != null)
        {
            var size = parent.transform.localScale;
            parent.transform.localScale = new Vector3(size.x - sizeOffset, size.y, size.z - sizeOffset);

            floorHeight = parent.transform.position.y;
        }

        var mat = gameObject.GetComponent<MeshRenderer>().material;
        mat.SetFloat("_Y", floorHeight);
        mat.SetFloat("_InScene", -1.0f);
    }

    private void Start()
    {
        SetEvent(1);

#if UNITY_EDITOR
        if (map == null)
        {
            Debug.LogError("No map set");
        }

        if (index < 0 || index > map.respawnPositions.Length)
        {
            Debug.LogError("Index is out of range.");
        }

        if (map != MapsManager.CurrentMap)
        {
            Debug.LogError("Current map is not same.");
        }
#endif
    }

    private void SetEvent(int indicator)
    {
        if (indicator > 0)
        {
            MapsManager.Initialized += SetEnterFlag;
        }

        else
        {
            MapsManager.Initialized -= SetEnterFlag;
        }
    }

    void SetEnterFlag(object obj, bool mute)
    {
        enterFlag = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (InGameTimer.Paused) { return; }
        if (map == null) { return; }
        if (!enterFlag) { return; }

        enterFlag = !enterFlag;

        var log = Bools.Get(Bools.Item.write_events);

        if (log) { WriteToLog(InvokeCommand.GameEvent.on_enter_checkpoint); }
        EnterCheckpoint?.Invoke(null, other.gameObject.transform.position);

        if (isGoal)
        {
            if (log) { WriteToLog(InvokeCommand.GameEvent.on_enter_goal); }
            EnterGoal?.Invoke(null, other.gameObject.transform.position);
        }

        else if (index == 0)
        {
            if (log) { WriteToLog(InvokeCommand.GameEvent.on_enter_start); }
            EnterStart?.Invoke(null, other.gameObject.transform.position);
        }

        if (index > map.Index && currentMapName == map.MapName)
        {
            if (log) { WriteToLog(InvokeCommand.GameEvent.on_enter_next_checkpoint); }
            EnterAnotherCheckpoint?.Invoke(null, index);
        }

        indexChecked = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (InGameTimer.Paused) { return; }
        if (map == null) { return; }
        if (enterFlag) { return; }

        enterFlag = !enterFlag;

        var log = Bools.Get(Bools.Item.write_events);

        if (log) { WriteToLog(InvokeCommand.GameEvent.on_exit_checkpoint); }
        ExitCheckpoint?.Invoke(null, other.gameObject.transform.position);

        if (index == 0 && !isGoal)
        {
            if (log) { WriteToLog(InvokeCommand.GameEvent.on_exit_start); }
            ExitStart?.Invoke(null, other.gameObject.transform.position);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (InGameTimer.Paused) { return; }
        if (map == null) { return; }
        if (isGoal) { return; }
        if (!indexChecked) { return; }

        indexChecked = false;
        enterFlag = false;

        currentMapName = map.MapName;
        map.SetIndex(index);
    }

    static public void WriteToLog(InvokeCommand.GameEvent gameEvent)
    {
        var time = TimerCommand.TimeString(InGameTimer.ActiveTime, mSec: true);
        ConsoleMessage.WriteLog("<color=cyan>Åy" + time + "Åz" + gameEvent.ToString() + "\t (frame : " + InGameTimer.Frame.ToString() + ")</color>");
    }
}
