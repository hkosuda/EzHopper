using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultCommand : Command
{
    public DefaultCommand()
    {
        commandName = "default";
        description = "'pm_' �Ŏn�܂�ݒ���f�t�H���g�l�ɖ߂��܂��D";
        detail = "'pm_' �Ŏn�܂�ݒ�́C�v���C���[�̓����Ɋւ���ݒ�������܂��D";
    }

    public override void CommandMethod(Tracer tracer, List<string> values, List<string> options)
    {
        if (values == null || values.Count == 0) { return; }

        else if (values.Count == 1)
        {
            foreach(var setting in Floats.Settings)
            {
                if (!setting.Key.ToString().StartsWith("pm_")) { continue; }
                setting.Value.SetDefault();
            }

            AddMessage("�S�Ă̒l���f�t�H���g�l�ɂ��ǂ��܂����D", Tracer.MessageLevel.normal, tracer, options);
        }

        else
        {
            AddMessage(ERROR_OverValues(0), Tracer.MessageLevel.error, tracer, options);
        }
    }
}
