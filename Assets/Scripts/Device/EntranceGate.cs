using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceGate : MonoBehaviour
{
    [SerializeField] GameObject exitGate;

    GameObject parent;

    private void Awake()
    {
        parent = gameObject.transform.parent.parent.gameObject;
    }

    private void Start()
    {
        if (exitGate == null)
        {
            Debug.LogError("No Exit Gate");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        var entranceRotY = parent.transform.eulerAngles.y;
        var exitRotY = exitGate.transform.eulerAngles.y;

        var deltaRotY = exitRotY - entranceRotY + 180.0f;

        PM_Camera.Rotate(deltaRotY);
        PM_Main.RotVelocity(deltaRotY);

        var dy = other.transform.position.y - parent.transform.position.y;
        other.transform.position = ExitPosition(exitGate, dy);
    }

    static Vector3 ExitPosition(GameObject exitGate, float dy)
    {
        var rotY = exitGate.transform.eulerAngles.y * Mathf.Deg2Rad;

        var dx = Mathf.Sin(rotY);
        var dz = Mathf.Cos(rotY);

        return exitGate.transform.position + new Vector3(dx, dy, dz);
    }
}
