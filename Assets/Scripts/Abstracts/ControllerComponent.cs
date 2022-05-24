using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ControllerComponent
{
    public virtual void Initialize() { }

    public virtual void Shutdown() { }

    public virtual void Update(float dt) { }
    public virtual void LateUpdate() { }
}
