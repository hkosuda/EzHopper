using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DE_Availability : ControllerComponent
{
    static readonly float preparingTime = 0.67f;
    static readonly float shootingInterval = 0.14f;

    static public EventHandler<bool> PreparingBegin { get; set; }

    static public bool Availability { get; private set; }

    static float shootingIntervalRemain;
    static float preparingTimeRemain;

    public override void Initialize()
    {
        StartPreparing();

        SetEvent(1);
    }

    public override void Shutdown()
    {
        SetEvent(-1);
    }

    static void SetEvent(int indicator)
    {
        if (indicator > 0)
        {
            DeAnimator.PreparingEnd += PreparingEnd;
            DE_Shooter.Shot += InitializeShootingIntervalRemain;
        }

        else
        {
            DeAnimator.PreparingEnd -= PreparingEnd;
            DE_Shooter.Shot -= InitializeShootingIntervalRemain;
        }
    }

    static void PreparingEnd(object obj, bool mute)
    {
        preparingTimeRemain = 0.0f;
    }

    public override void Update(float dt)
    {
        preparingTimeRemain -= dt;
        shootingIntervalRemain -= dt;

        if (preparingTimeRemain < 0.0f) { preparingTimeRemain = 0.0f; }
        if (shootingIntervalRemain < 0.0f) { shootingIntervalRemain = 0.0f; }

        if (DE_Main.Suspend) { Availability = false; return; }

        if (preparingTimeRemain > 0.0f || shootingIntervalRemain > 0.0f)
        {
            Availability = false;
        }

        else
        {
            Availability = true;
        }
    }

    static public void StartPreparing()
    {
        preparingTimeRemain = preparingTime;

        PreparingBegin?.Invoke(null, false);
    }

    static void InitializeShootingIntervalRemain(object obj, Vector3 direction)
    {
        shootingIntervalRemain = shootingInterval;
    }
}
