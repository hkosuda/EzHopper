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
#if UNITY_EDITOR
        Standard();
#endif
    }

    static void Standard()
    {
        // 異なるチェックポイントに到達したらレコーダーを停止．
        // 

        CommandReceiver.RequestCommand("invoke add on_course_out \"back -m\"");

        CommandReceiver.RequestCommand("invoke add on_exit_checkpoint \"recorder start -f\"");
        CommandReceiver.RequestCommand("invoke add on_enter_checkpoint \"recorder end -f\"");
        CommandReceiver.RequestCommand("invoke add on_enter_another_checkpoint \"recorder save %map%_%now% -f\"");
        CommandReceiver.RequestCommand("invoke add on_course_out \"recorder stop -f\"");

        CommandReceiver.RequestCommand("invoke add on_exit_checkpoint \"timer start -f\"");
        CommandReceiver.RequestCommand("invoke add on_enter_checkpoint \"timer stop -m\"");
        CommandReceiver.RequestCommand("invoke add on_enter_checkpoint \"timer now -f\"");

        CommandReceiver.RequestCommand("invoke add on_enter_checkpoint \"ghost\" -m");
        CommandReceiver.RequestCommand("invoke add on_exit_checkpoint \"ghost end -m\"");

        CommandReceiver.RequestCommand("toggle t \"observer start -f\" \"observer end -f\"");

        CommandReceiver.RequestCommand("bind p \"anchor set -echo\"");
        CommandReceiver.RequestCommand("bind v \"anchor back -echo\"");
        CommandReceiver.RequestCommand("bind r \"recorder start -echo\"");
        CommandReceiver.RequestCommand("bind f \"recorder end -echo\"");
        CommandReceiver.RequestCommand("bind z \"ghost end -echo\"");

        CommandReceiver.RequestCommand("write_events 1");
        CommandReceiver.RequestCommand("override echo");
    }
}
