using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    public delegate void OnShotAction();

    OnShotAction action;

    void Start()
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
            DE_Shooter.ShootingHit += React;
        }

        else
        {
            DE_Shooter.ShootingHit -= React;
        }
    }

    void React(object obj, RaycastHit hit)
    {
        if (gameObject != hit.collider.gameObject) { return; }

        action?.Invoke();
    }

    public void SetAction(OnShotAction action)
    {
        this.action = action;
    }
}
