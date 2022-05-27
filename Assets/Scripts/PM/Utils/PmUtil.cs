using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PmUtil
{
    // constants
    static readonly float _maxSpeedOnTheGround = 9.5f;
    static readonly float _maxSpeedInTheAir = 0.6f;
    static readonly float _accelOnTheGround = 80.0f;
    static readonly float _accelInTheAir = 100.0f;
    static readonly float _draggingAccel = 50.0f;

    // daccel = 30.0 - 35.0 ?

    static public Vector2 AddVector { get; private set; }

    

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

        AddVector = inputVector * magnitudeOfAddVector;

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
