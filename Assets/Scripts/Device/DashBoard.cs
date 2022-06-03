using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashBoard : MonoBehaviour
{
    public Vector3 velocity;

    private void OnTriggerExit(Collider other)
    {
        PM_Main.Rb.velocity = velocity;
    }
}
