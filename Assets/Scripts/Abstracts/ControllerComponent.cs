using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ControllerComponent
{
    public virtual void Initialize() { }

    public virtual void Shutdown() { }

    public virtual bool Update(float dt) { return true; }
    public virtual void LateUpdate() { }
}
