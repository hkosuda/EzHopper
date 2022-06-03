using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DE_Potensial : ControllerComponent
{
    static public readonly float maxPotential = 0.8f;
    static public readonly float potentialIncrease = 0.3f;

    static public float Potential { get; private set; }

    public override void Initialize()
    {
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
            DE_Shooter.Shot += IncreasePotential;
        }

        else
        {
            DE_Shooter.Shot -= IncreasePotential;
        }
    }

    static void IncreasePotential(object obj, Vector3 direction)
    {
        Potential += DE_Availability.shootingInterval + potentialIncrease;
        if (Potential > maxPotential) { Potential = maxPotential; }
    }

    public override void Update(float dt)
    {
        Potential -= dt;
        if (Potential < 0.0f) { Potential = 0.0f; }
    }
}
