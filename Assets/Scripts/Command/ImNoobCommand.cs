using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImNoobCommand :Command
{
    public ImNoobCommand()
    {
        commandName = "i_am_noob";
    }

    public override void CommandMethod(Tracer tracer, List<string> values)
    {
        if (values == null || values.Count < 1) { return; }

        if (values.Count == 1)
        {
            ClientParams.SetNoobOrNot(true);
            return;
        }

        var value = values[1].ToLower();

        if (value == "false")
        {
            ClientParams.SetNoobOrNot(false);
            return;
        }

        if (value == "true")
        {

        }

        else if (value != "true")
        {
            tracer.AddMessage("'true'‚à‚µ‚­‚Í'false'‚Ì‚Ý’l‚Æ‚µ‚ÄÝ’è‰Â”\‚Å‚·D", Tracer.MessageLevel.error);
        }
    }
}
