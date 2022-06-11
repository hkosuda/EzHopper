using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    static public EventHandler<Vector3> ExitCheckpoint { get; set; }
    static public EventHandler<Vector3> EnterCheckpoint { get; set; }
    static public EventHandler<Vector3> ExitStart { get; set; }
    static public EventHandler<Vector3> EnterStart { get; set; }
    static public EventHandler<Vector3> EnterGoal { get; set; }

    [SerializeField] Map map;
    [SerializeField] int index = 0;
    [SerializeField] bool isGoal = false;

    private void Start()
    {
#if UNITY_EDITOR
        if (map == null)
        {
            Debug.LogError("No map set");
            return;
        }

        if (index < 0 || index > map.respawnPositions.Length)
        {
            Debug.LogError("Index is out of range.");
            return;
        }
#endif
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Timer.Paused) { return; }
        if (map == null) { return; }

        EnterCheckpoint?.Invoke(null, other.gameObject.transform.position);

        if (isGoal)
        {
            EnterGoal?.Invoke(null, other.gameObject.transform.position);
        }

        else if (index == 0)
        {
            EnterStart?.Invoke(null, other.gameObject.transform.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (Timer.Paused) { return; }
        if (map == null) { return; }

        ExitCheckpoint?.Invoke(null, other.gameObject.transform.position);

        if (index == 0 && !isGoal)
        {
            ExitStart?.Invoke(null, other.gameObject.transform.position);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (Timer.Paused) { return; }
        if (map == null) { return; }
        if (isGoal) { return; }

        map.SetIndex(index);
    }
}
