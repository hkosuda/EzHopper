using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EZ_Rotation : MonoBehaviour
{
    static readonly float rotationSpeed = 45.0f;
    static readonly float speedReduction = 1000.0f;

    static float rotY;
    static float currentSpeed;

    static GameObject myself;

    private void Awake()
    {
        myself = gameObject;
    }

    void Start()
    {
        SetEvent(1);
    }

    private void OnDestroy()
    {
        SetEvent(-1);
    }

    static void SetEvent(int indicator)
    {
        if (indicator > 0)
        {
            InGameTimer.Updated += UpdateMethod;
            DE_Shooter.ShootingHit += OnShot;
        }

        else
        {
            InGameTimer.Updated -= UpdateMethod;
            DE_Shooter.ShootingHit -= OnShot;
        }
    }

    static void UpdateMethod(object obj, float dt)
    {
        currentSpeed -= speedReduction * dt;
        if (currentSpeed < rotationSpeed) { currentSpeed = rotationSpeed; }

        rotY += currentSpeed * dt;
        myself.transform.eulerAngles = new Vector3(0.0f, rotY, 0.0f);
    }

    static void OnShot(object obj, RaycastHit hit)
    {
        if (hit.collider == null) { return; }

        if (hit.collider.gameObject == myself)
        {
            currentSpeed = rotationSpeed * 50.0f;

            var ez = GetEz();
            CommandReceiver.RequestCommand("delayedchat 0.5 1.5 " + ez + " -m");
        }
    }

    static string GetEz()
    {
        var times = UnityEngine.Random.Range(1, 10);

        var ez = "";

        for(var n = 0; n < times; n++)
        {
            ez += "ez";
        }

        return ez;
    }
}
