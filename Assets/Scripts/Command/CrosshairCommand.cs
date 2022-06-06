using System;   
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairCommand : Command
{
    public CrosshairCommand()
    {
        commandName = "crosshair";
    }

    enum CrosshairColor
    {
        none = 0,

        white = 1,
        red = 2,
        magenta = 3,
        yellow = 4,
        green = 5,
        cyan = 6,
    }

    static Dictionary<CrosshairColor, Color> colorList = new Dictionary<CrosshairColor, Color>()
    {
        { CrosshairColor.white, new Color(1.0f, 1.0f, 1.0f, 1.0f) },
        { CrosshairColor.red, new Color(1.0f, 0.0f, 0.0f, 1.0f) },
        { CrosshairColor.magenta, new Color(1.0f, 0.0f, 1.0f, 1.0f) },
        { CrosshairColor.yellow, new Color(1.0f, 1.0f, 0.0f, 1.0f) },
        { CrosshairColor.green, new Color(0.0f, 1.0f, 0.0f, 1.0f) },
        { CrosshairColor.cyan, new Color(0.0f, 1.0f, 1.0f, 1.0f) },
    };

    static CrosshairColor currentColor = CrosshairColor.white;

    public override void CommandMethod(Tracer tracer, List<string> values)
    {
        if (values == null || values.Count == 0) { return; }

        if (values.Count == 1)
        {
            var length = Floats.Get(Floats.Item.crosshair_length);
            var gap = Floats.Get(Floats.Item.crosshair_gap);
            var colorIndex = (int)currentColor;

            var profile = length.ToString() + gap.ToString() + colorIndex.ToString();

            tracer.AddMessage("現在のクロスヘアのプロファイル：" + profile, Tracer.MessageLevel.normal);
            tracer.AddMessage("プロファイルは，このcrosshairコマンドの値に指定することで，クロスヘアを素早く設定できます．", Tracer.MessageLevel.warning);
            return;
        }

        //var color = GetCrosshairColor(value);

        // - inner function
        static CrosshairColor GetCrosshairColor(string value)
        {
            foreach(CrosshairColor color in Enum.GetValues(typeof(CrosshairColor)))
            {
                if (value == color.ToString())
                {
                    return color;
                }
            }

            return CrosshairColor.none;
        }
    }

    static public Color CurrentColor()
    {
        if (!colorList.ContainsKey(currentColor))
        {
            return new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }

        return colorList[currentColor];
    }
}
