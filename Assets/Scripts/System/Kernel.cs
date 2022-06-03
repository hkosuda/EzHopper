using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class Kernel
{
    static List<IKernelManager> managerList;
    static List<IKernelController> controllerList;

    static public void Initialize()
    {
        managerList = new List<IKernelManager>()
        {
            new CommandInitializer(),
            new PlayerRecorder(),
            new DemoManager(),

            new DebugPlayerRecorder(),
            new ToxicSystem(),
        };

        controllerList = new List<IKernelController>()
        {
        };

        foreach(var manager in managerList)
        {
            manager.Initialize();
        }

        foreach(var controller in controllerList)
        {
            controller.Begin();
        }
    }

    static public void Shutdown()
    {
        foreach(var manager in managerList)
        {
            manager.Shutdown();
        }

        foreach(var controller in controllerList)
        {
            controller.Finish();
        }
    }

    static public void Reset()
    {
        foreach(var manager in managerList)
        {
            manager.Reset();
        }
    }
}
