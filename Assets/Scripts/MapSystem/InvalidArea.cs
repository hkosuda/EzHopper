using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvalidArea : MonoBehaviour
{
    [SerializeField] bool active = true;
    [SerializeField] GameObject respawnPosition;

    private void Start()
    {
#if UNITY_EDITOR
        if (respawnPosition == null)
        {
            Debug.LogError("No Respawn Position");
        }
#endif 
    }

    private void OnTriggerStay(Collider other)
    {
        if (active)
        {
            PM_Camera.SetEulerAngles(respawnPosition.transform.eulerAngles);
            PM_Main.Myself.transform.position = respawnPosition.transform.position;
            PM_Main.Rb.velocity = Vector3.zero;
        }
    }

    public void SetRespawnPosition(GameObject _respawnPosition)
    {
        respawnPosition = _respawnPosition;
    }
}
