using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinerDampingSolver
{
    static public bool Active { get; set; } = false;

    static readonly float t2_rate = 0.5f;

    static readonly float t1 = 0.02f;
    static readonly float t2 = 0.05f;
    static readonly float t3 = 0.1f;

    static readonly float v_amp = 1.4f;
    static readonly float h_amp = 0.0f;

    static float h_a1;
    static float h_b1;
    static float h_a2;
    static float h_b2;
    static float h_a3;
    static float h_b3;

    static float v_a1;
    static float v_b1;
    static float v_a2;
    static float v_b2;
    static float v_a3;
    static float v_b3;

    static float pastTime;

    static public void Initialize()
    {
        h_a1 = h_amp / t1;
        v_a1 = v_amp / t1;

        h_b1 = 0.0f;
        v_b1 = 0.0f;

        h_a2 = (h_amp * t2_rate - h_amp) / (t2 - t1);
        v_a2 = (v_amp * t2_rate - v_amp) / (t2 - t1);

        h_b2 = h_amp - h_a2 * t1;
        v_b2 = v_amp - v_a2 * t1;

        h_a3 = (0 - t2_rate * h_amp) / (t3 - t2);
        v_a3 = (0 - t2_rate * v_amp) / (t3 - t2);

        h_b3 = -h_a3 * t3;
        v_b3 = -v_a3 * t3;

        pastTime = 0.0f;
        Active = true;
    }

    static public void UpdateTime(float dt)
    {
        pastTime += dt;
    }

    static public float[] GetPosition()
    {
        if (pastTime < t1)
        {
            var v = v_a1 * pastTime + v_b1;
            var h = h_a1 * pastTime + h_b1;

            return new float[2] { v, h };
        }

        else if (pastTime < t2)
        {
            var v = v_a2 * pastTime + v_b2;
            var h = h_a2 * pastTime + h_b2;

            return new float[2] { v, h };
        }

        else if (pastTime < t3)
        {
            var v = v_a3 * pastTime + v_b3;
            var h = h_a3 * pastTime + h_b3;

            return new float[2] { v, h };
        }

        else
        {
            Active = false;
            return new float[2] { 0.0f, 0.0f };
        }
    }
}
