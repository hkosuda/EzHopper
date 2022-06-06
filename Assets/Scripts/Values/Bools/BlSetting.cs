using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlSetting : MySetting
{
    public EventHandler<bool> ValueUpdated { get; set; }

    public bool CurrentValue { get; private set; }
    public bool DefaultValue { get; }

    public BlSetting(bool defaultValue, string description) : base(description)
    {
        DefaultValue = defaultValue;
        CurrentValue = defaultValue;
    }

    public void SetDefault()
    {
        CurrentValue = DefaultValue;
    }

    public void SetValue(bool value)
    {
        var previousValue = CurrentValue;

        CurrentValue = value;
        ValueUpdated?.Invoke(this, previousValue);
    }
}
