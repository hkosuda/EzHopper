using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    [SerializeField] MapName _initialMap = MapName.ez_athletic;

    static MapName initialMap = MapName.ez_horizon;
    static public GameObject Root { get; private set; }

    private void Awake()
    {
        initialMap = _initialMap;

        Root = new GameObject("Root");
        Kernel.Initialize();
    }

    void Start()
    {
        Kernel.Reset();
        MapsManager.Begin(initialMap);

        SetEvent(1);

#if UNITY_EDITOR
        CommandReceiver.RequestCommand("anchor set");
#endif 
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
