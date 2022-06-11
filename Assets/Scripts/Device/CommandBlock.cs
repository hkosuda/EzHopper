using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandBlock : MonoBehaviour
{
    static readonly Bools.Item item = Bools.Item.show_button;

    [SerializeField] string command;

    bool initialized = false;

    GameObject canvas;

    private void Awake()
    {
        canvas = gameObject.transform.parent.GetChild(1).gameObject;

        initialized = false;
    }

    void Start()
    {
        SetEvent(1);
        UpdateVisibility(null, false);
    }

    private void OnDestroy()
    {
        SetEvent(-1);
    }

    private void Update()
    {
#if UNITY_EDITOR
        //if (!initialized)
        //{
        //    initialized = true;

        //    if (!CommandReceiver.RequestCommand(command, false, false))
        //    {
        //        Debug.LogError("Command Failed : " + command);
        //    }
        //}
#endif
    }

    void SetEvent(int indicator)
    {
        if (indicator > 0)
        {
            DE_Shooter.ShootingHit += React;
            Bools.Settings[item].ValueUpdated += UpdateVisibility;
        }

        else
        {
            DE_Shooter.ShootingHit -= React;
            Bools.Settings[item].ValueUpdated -= UpdateVisibility;
        }
    }

    void React(object obj, RaycastHit hit)
    {
        if (hit.collider.gameObject != gameObject) { return; }

        CommandReceiver.RequestCommand(command + " -mute");
    }

    void UpdateVisibility(object obj, bool prev)
    {
        if (canvas == null) { canvas = gameObject.transform.parent.GetChild(1).gameObject; }

        gameObject.SetActive(Bools.Get(item));
        canvas.SetActive(Bools.Get(item));
    }
}
