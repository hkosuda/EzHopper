using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    static readonly MapName initialMap = MapName.ez_horizon;
    static public GameObject Root { get; private set; }

    private void Awake()
    {
        Root = new GameObject("Root");
        Kernel.Initialize();
    }

    void Start()
    {
        Kernel.Reset();
        MapsManager.Begin(initialMap);

        SetEvent(1);

        CommandReceiver.RequestCommand("anchor set", true);
    }

    private void OnDestroy()
    {
        SetEvent(-1);
    }

    static void SetEvent(int indicator)
    {
        if (indicator > 0)
        {
            MapsManager.Initialized += GenerateRoot;
            MapsManager.Initialized += ResetKernel;
        }

        else
        {
            MapsManager.Initialized -= GenerateRoot;
            MapsManager.Initialized -= ResetKernel;
        }
    }

    static void GenerateRoot(object obj, bool mute)
    {
        if (Root != null) { Destroy(Root); }
        Root = new GameObject("Root");
    }

    static public void SetChildOfRoot(GameObject gameObject)
    {
        if (Root == null) { Root = new GameObject("Root"); }
        gameObject.transform.SetParent(Root.transform);
    }

    static void ResetKernel(object obj, bool mute)
    {
        Kernel.Reset();
    }
}
