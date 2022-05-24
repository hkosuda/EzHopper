using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class Calcf
{
    static public float SafetyDiv(float a, float b, float alternative)
    {
        if (b == 0.0f) { return alternative; }

        return a / b;
    }
}
