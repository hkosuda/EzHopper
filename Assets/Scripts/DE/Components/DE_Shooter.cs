using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DE_Shooter : ControllerComponent
{
    static public EventHandler<Vector3> Shot { get; set; }
    static public EventHandler<RaycastHit> ShootingHit { get; set; }

    public override void Initialize()
    {
        base.Initialize();
    }

    public override void Shutdown()
    {
        base.Shutdown();
    }

    public override bool Update(float dt)
    {
        if (!Keyconfig.CheckInput(Keyconfig.KeyAction.shot, true)) { return true; }
        if (!DE_Availability.Availability) { return true; }

        DeShot();
        return true;
    }

    static void DeShot()
    {
        var ray = GetRay();

        Shot?.Invoke(null, ray.direction);

        if (Physics.Raycast(ray, hitInfo: out RaycastHit hit))
        {
            ShootingHit?.Invoke(null, hit);
        }

        // - inner function
        static Ray GetRay()
        {
            return new Ray()
            {
                origin = PM_Camera.CameraTr.position,
                direction = SpreadSolver.CalcSpread(),
            };
        }
    }
}

public class SpreadSolver
{
    static readonly float lifting = 0.8f;
    static readonly float h_random = 1.8f;
    static readonly float v_random = 1.0f;
    static readonly float h_jumping = 1.0f;
    static readonly float v_jumping = 1.6f;

    static readonly float liftingExpo = 1.5f;
    static readonly float randomExpo = 1.5f;
    static readonly float runningExpo = 0.5f;

    static public Vector3 CalcSpread()
    {
        var potentialRate = DE_Potensial.Potential / DE_Potensial.maxPotential;
        if (potentialRate > 1.0f) { potentialRate = 1.0f; }

        var rotX = -PM_Camera.EulerAngles().x * Mathf.Deg2Rad;
        var rotY = PM_Camera.EulerAngles().y * Mathf.Deg2Rad;

        var pqr_vector = new float[3] { 10.0f, 0.0f, 0.0f };

        pqr_vector = CalcLifting(pqr_vector, potentialRate);
        pqr_vector = CalcRandomSpread(pqr_vector, potentialRate);
        pqr_vector = CalcJumpingRunningSpread(pqr_vector);

        var zxy_vector = PQR2ZXY(pqr_vector, rotX, rotY);

        var z = zxy_vector[0];
        var x = zxy_vector[1];
        var y = zxy_vector[2];

        return new Vector3(x, y, z);

        // - inner functions
        static float[] CalcLifting(float[] pqr_vector, float potentialRatio)
        {
            var p = pqr_vector[0];
            var q = pqr_vector[1];
            var r = pqr_vector[2];

            r += lifting * Mathf.Pow(potentialRatio, liftingExpo);

            return new float[3] { p, q, r };
        }

        // - inner function
        static float[] CalcRandomSpread(float[] pqr_vector, float potentialRatio)
        {
            var p = pqr_vector[0];
            var q = pqr_vector[1];
            var r = pqr_vector[2];

            var rate = Mathf.Pow(potentialRatio, randomExpo);
            var qr_spread = GetEllipseRandomSpread(h_random, v_random, 0.0f, rate);

            q += Mathf.Abs(qr_spread[0]) * RandomDirection();
            r += qr_spread[1];

            return new float[3] { p, q, r };

            
        }

        static float[] CalcJumpingRunningSpread(float[] pqr_vector)
        {
            var p = pqr_vector[0];
            var q = pqr_vector[1];
            var r = pqr_vector[2];

            // running
            var maxSpeed = Floats.Get(Floats.Item.pm_max_speed_on_ground);
            var velocity = new Vector2(PM_Main.Rb.velocity.x, PM_Main.Rb.velocity.z).magnitude;

            var v_rate = (velocity / maxSpeed);
            if (v_rate > 1.0f) { v_rate = 1.0f; }

            v_rate = Mathf.Pow(v_rate, runningExpo);

            // jumping
            var j_rate = 0.0f;
            if (PM_Landing.LandingIndicator < 0) { j_rate = 1.0f; }

            var rate = v_rate + j_rate;

            var qr_spread = GetEllipseRandomSpread(h_jumping, v_jumping, 0.0f, rate);

            q += qr_spread[0] * RandomDirection();
            r += qr_spread[1];

            return new float[3] { p, q, r };
        }

        // function
        static float[] PQR2ZXY(float[] pqr_vector, float radrotX, float radrotY)
        {
            var p = pqr_vector[0];
            var q = pqr_vector[1];
            var r = pqr_vector[2];

            var sX = Mathf.Sin(radrotX);
            var cX = Mathf.Cos(radrotX);
            var sY = Mathf.Sin(radrotY);
            var cY = Mathf.Cos(radrotY);

            var z = p * cX * cY - q * sY - r * sX * cY;
            var x = p * cX * sY + q * cY - r * sX * sY;
            var y = p * sX + r * cX;

            return new float[3] { z, x, y };
        }

        // - inner function
        static float[] GetEllipseRandomSpread(float h_max, float v_max, float v_min, float rate)
        {
            h_max *= rate;
            v_max *= rate;
            if (h_max == 0.0f) { return new float[2] { 0.0f, 0.0f }; }

            var qq = UnityEngine.Random.Range(-h_max, h_max);
            var rr_max = v_max * Mathf.Sqrt(1.0f - Mathf.Pow(qq / h_max, 2.0f));
            var rr = UnityEngine.Random.Range(-rr_max, rr_max);

            return new float[2] { qq, rr };
        }

        // - inner function
        static float RandomDirection()
        {
            var value = UnityEngine.Random.Range(0.0f, 1.0f);

            if (value > 0.5f) { return 1.0f; }
            return -1.0f;
        }
    }
}
