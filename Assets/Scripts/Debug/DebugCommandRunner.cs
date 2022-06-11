using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCommandRunner : MonoBehaviour
{
    static bool initialized = false;

    void Update()
    {
        if (!initialized)
        {
            initialized = true;
            RunCommand();
        }
    }

    static void RunCommand()
    {
        CommandReceiver.RequestCommand("ignite add on_course_out \"back -m\"");
        CommandReceiver.RequestCommand("ignite add on_exit_checkpoint \"timer start -f\"");
        CommandReceiver.RequestCommand("ignite add on_enter_checkpoint \"timer stop -f\"");

#if UNITY_EDITOR
        CommandReceiver.RequestCommand("bind p anchor set -echo");
        CommandReceiver.RequestCommand("bind v anchor back -echo");
        CommandReceiver.RequestCommand("bind 1 recorder start -echo");
        CommandReceiver.RequestCommand("bind -1 recorder end -echo");
        CommandReceiver.RequestCommand("bind r drecorder save -echo");
        CommandReceiver.RequestCommand("bind z ghost end -echo");
#endif
    }
}
