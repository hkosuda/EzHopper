using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKernelManager
{
    public void Initialize();
    public void Shutdown();
    public void Reset();
}
