using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MySetting
{
    public string Description { get; }

    public MySetting(string description)
    {
        Description = description;
    }
}
