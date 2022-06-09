using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square2InvalidArea : MonoBehaviour
{
    [SerializeField] Map map;

#if UNITY_EDITOR
    private void Start()
    {
        if (map == null)
        {
            Debug.LogError("No Map");
        }
    }
#endif

    private void OnTriggerStay(Collider other)
    {
        if (Timer.Paused) { return; }

        if (other.gameObject == PM_Main.Myself)
        {
            var origin = PM_Main.Myself.transform.position;

            var respawn = map.respawnPositions[map.Index];

            var pos = respawn.transform.position;
            var rot = respawn.transform.eulerAngles;

            PM_Main.ResetPosition(pos, rot.y);
            InvalidArea.CourseOut?.Invoke(null, origin);
        }
    }
}
