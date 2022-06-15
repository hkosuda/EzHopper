using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandRunner : MonoBehaviour
{
    static bool initialized = false;

    void Update()
    {
        if (!initialized)
        {
            initialized = true;
            Default();
        }
    }

    static void Default()
    {
        foreach (var command in SAU_Default.commandList)
        {
            CommandReceiver.RequestCommand(command);
        }

        foreach (var command in SAU_Keybind.commandList)
        {
            CommandReceiver.RequestCommand(command);
        }
    }
}
