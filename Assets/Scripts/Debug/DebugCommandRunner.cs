using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCommandRunner : MonoBehaviour
{
#if UNITY_EDITOR

    static bool initialized = false;

    private void Update()
    {
        if (initialized) { return; }

        initialized = true;
        //ErrorTest();
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

        CommandReceiver.RequestCommand("invoke add on_enter_next_checkpoint \"chat クリアに要した回数：%counter%\"");
        CommandReceiver.RequestCommand("invoke add on_enter_next_checkpoint \"chat 経過時間：%time%\"");

        CommandReceiver.RequestCommand("bind p \"anchor set -echo\"");
        CommandReceiver.RequestCommand("bind v \"anchor back -echo\"");
        CommandReceiver.RequestCommand("bind r \"recorder start -echo\"");
        CommandReceiver.RequestCommand("bind f \"recorder end -echo\"");
        CommandReceiver.RequestCommand("bind z \"ghost end -echo\"");

        CommandReceiver.RequestCommand("write_events 1");
        CommandReceiver.RequestCommand("override echo");
    }

    static void ErrorTest()
    {
        // < invoke test >

        // cant add 'begin' to on_map_changed (add)
        CommandReceiver.RequestCommand("invoke add on_map_changed \"begin ez_nostalgia\"");

        // cant add 'begin' to on_map_changed (insert)
        CommandReceiver.RequestCommand("invoke add on_map_changed \"counter set 1\" -m");
        CommandReceiver.RequestCommand("invoke insert on_map_changed 0 \"begin ez_athletic\"");

        // cant add 'begin' to on_map_changed (replace)
        CommandReceiver.RequestCommand("invoke replace on_map_changed 0 \"begin ez_athletic\"");

        // insert test (duplication)
        CommandReceiver.RequestCommand("invoke remove_all all -m");
        CommandReceiver.RequestCommand("invoke add on_enter_checkpoint \"timer stop\" -m");
        CommandReceiver.RequestCommand("invoke add on_enter_checkpoint \"recorder end\" -m");
        CommandReceiver.RequestCommand("invoke insert on_enter_checkpoint 0 \"recorder end -f\"");

        // insert text (invalid index)
        CommandReceiver.RequestCommand("invoke remove_all all -m");
        CommandReceiver.RequestCommand("invoke add on_enter_checkpoint \"timer stop\" -m");
        CommandReceiver.RequestCommand("invoke add on_enter_checkpoint \"recorder end\" -m");
        CommandReceiver.RequestCommand("invoke insert on_enter_checkpoint 2 \"recorder end -f\"");

        // replace test (invalid index)
        CommandReceiver.RequestCommand("invoke remove_all all -m");
        CommandReceiver.RequestCommand("invoke add on_course_out \"back\" -m");
        CommandReceiver.RequestCommand("invoke replace on_course_out -1 \"back 0\"");

        // <bind test>

    }
#endif
}
