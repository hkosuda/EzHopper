using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlSetting : MySetting
{
    public EventHandler<float> ValueUpdated { get; set; }

    public float CurrentValue { get; private set; }
    public float DefaultValue { get; }
    public List<FlValidation> Validations { get; }

    public FlSetting(float defaultValue, string description,
        List<FlValidation> validations = null) : base(description)
    {
        CurrentValue = defaultValue;
        DefaultValue = defaultValue;
        Validations = validations;
    }

    public bool ValidationCheck(float value, Tracer tracer = null, List<string> options = null)
    {
        if (Validations == null)
        {
            return true;
        }

        var flag = true;

        foreach (var validation in Validations)
        {
            if (!validation.Check(value, tracer, options)) { flag = false; }
        }

        return flag;
    }

    public void SetDefault()
    {
        CurrentValue = DefaultValue;
    }

    public void SetValue(float value)
    {
        var previousValue = CurrentValue;

        CurrentValue = value;
        ValueUpdated?.Invoke(this, previousValue);
    }
}

public abstract class Validation
{
    public abstract string GetDiscription();
}

public abstract class FlValidation : Validation
{
    public abstract bool Check(float value, Tracer tracer = null, List<string> options = null);
}
