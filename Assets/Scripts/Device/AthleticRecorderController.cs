using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AthleticRecorderController : MonoBehaviour
{
    bool active = false;
    Vector3 lastLandingPoint;

    string tag;

    private void Awake()
    {
        tag = gameObject.tag;
    }

    private void Start()
    {
        SetEvent(1);
    }

    private void OnDestroy()
    {
        SetEvent(-1);
    }

    void SetEvent(int indicator)
    {
        if (indicator > 0)
        {
            Timer.Updated += UpdateMethod;
            MapsManager.Initialized += StopRecorder;

            PM_Landing.Landed += UpdateLandingPoint;
            PM_Landing.Landed += CheckLandingObjectTag;
        }

        else
        {
            Timer.Updated -= UpdateMethod;
            MapsManager.Initialized -= StopRecorder;

            PM_Landing.Landed -= UpdateLandingPoint;
            PM_Landing.Landed -= CheckLandingObjectTag;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        PlayerRecorder.BeginRecording();
        active = true;

        lastLandingPoint = PM_Main.Myself.transform.position - new Vector3(0.0f, PM_Main.centerY, 0.0f);
    }

    void UpdateLandingPoint(object obj, RaycastHit hit)
    {
        if (!active) { return; }
        if (hit.collider == null) { return; }

        lastLandingPoint = hit.point;
    }

    void CheckLandingObjectTag(object obj, RaycastHit hit)
    {
        if (!active) { return; }
        if (hit.collider == null) { return; }

        if (tag != hit.collider.gameObject.tag)
        {
            active = false;
            PlayerRecorder.FinishRecording(true);
        }
    }

    void UpdateMethod(object obj, float dt)
    {
        if (!active) { return; }

        var y = PM_Main.Myself.transform.position.y - PM_Main.centerY;

        if (y < lastLandingPoint.y - 0.5f)
        {
            active = false;
            PlayerRecorder.FinishRecording(true);
        }
    }

    void StopRecorder(object obj, bool mute)
    {
        active = false;
    }
}
