using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PmUtil
{
    // constants
    static public readonly float _maxSpeedOnTheGround = 7.7f;
    static public readonly float _maxSpeedInTheAir = 0.7f;
    static public readonly float _accelOnTheGround = 50.0f;
    static public readonly float _accelInTheAir = 100.0f;
    static public readonly float _draggingAccel = 30.0f;

    static public Vector2 AddVector { get; private set; }
    static public Vector2 NextVector { get; private set; }

    static public Vector2 CalcVector(Vector2 inputVector, Vector2 currentVector, float dt, bool onground)
    {
        // load settings 
        var draggingAccel = _draggingAccel;
        var maxSpeed = _maxSpeedOnTheGround;
        var accel = _accelOnTheGround;

        if (!onground)
        {
            draggingAccel = 0.0f;
            maxSpeed = _maxSpeedInTheAir;
            accel = _accelInTheAir;
        }

        var normalizedInputVector = inputVector.normalized;

        var magnitudeOfFriction = Clip(currentVector.magnitude, 0.0f, draggingAccel * dt);

        var frictionVector = currentVector.normalized * (-magnitudeOfFriction);

        var playerVector_fric = currentVector + frictionVector;

        var magnitudeOfProjection = Vector2.Dot(playerVector_fric, normalizedInputVector);

        var magnitudeOfAddVector = Clip(maxSpeed - magnitudeOfProjection, 0.0f, accel * dt);

        var addVector = normalizedInputVector * magnitudeOfAddVector;

        var nextPlayerVector = playerVector_fric + addVector;

        AddVector = addVector;
        NextVector = nextPlayerVector;

        return nextPlayerVector;

        // - inner function
        static float Clip(float val, float minVal, float maxVal)
        {
            if (val < minVal) { return minVal; }
            if (val > maxVal) { return maxVal; }
            return val;
        }
    }
}
