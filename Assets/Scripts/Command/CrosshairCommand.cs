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

    static readonly Dictionary<CrosshairColor, Color> colorList = new Dictionary<CrosshairColor, Color>()
    {
        { CrosshairColor.white, new Color(1.0f, 1.0f, 1.0f, 1.0f) },
        { CrosshairColor.red, new Color(1.0f, 0.0f, 0.0f, 1.0f) },
        { CrosshairColor.magenta, new Color(1.0f, 0.0f, 1.0f, 1.0f) },
        { CrosshairColor.yellow, new Color(1.0f, 1.0f, 0.0f, 1.0f) },
        { CrosshairColor.green, new Color(0.0f, 1.0f, 0.0f, 1.0f) },
        { CrosshairColor.cyan, new Color(0.0f, 1.0f, 1.0f, 1.0f) },
    };

    static CrosshairColor currentColor = CrosshairColor.white;

    public override List<string> AvailableValues(List<string> values)
    {
        if (values == null || values.Count == 0) { return new List<string>(); }

        if (values.Count < 3)
        {
            return new List<string>()
            {
                "length", "width", "gap", "color"
            };
        }

        if (values.Count < 4)
        {
            if (values[1] == "color")
            {
                var colors = new List<string>();

                foreach(var key in colorList.Keys)
                {
                    colors.Add(key.ToString());
                }

                return colors;
            }
        }

        return new List<string>();
    }

    public override void CommandMethod(Tracer tracer, List<string> values)
    {
        if (values == null || values.Count == 0) { return; }

        if (values.Count == 1)
        {
            var length = Floats.Get(Floats.Item.crosshair_length);
            var width = Floats.Get(Floats.Item.crosshair_width);
            var gap = Floats.Get(Floats.Item.crosshair_gap);
            var colorIndex = (int)currentColor;

            var profile = length.ToString() + width.ToString() + gap.ToString() + colorIndex.ToString();

            tracer.AddMessage("現在のクロスヘアのプロファイル：" + profile, Tracer.MessageLevel.normal);
            return;
        }

        if (values.Count == 2)
        {

            return;
        }

        if (values.Count == 3)
        {
            var option = values[1];
            var value = values[2];

            if (option == "length")
            {
                SetValue(Floats.Item.crosshair_length, value, tracer);
                return;
            }

            if (option == "width")
            {
                SetValue(Floats.Item.crosshair_width, value, tracer);
                return;
            }

            if (option == "gap")
            {
                SetValue(Floats.Item.crosshair_gap, value, tracer);
                return;
            }

            if (option == "color")
            {
                var color = GetColor(value);

                if (color == CrosshairColor.none)
                {
                    tracer.AddMessage(color + "は有効な色ではありません．", Tracer.MessageLevel.error);
                    return;
                }

                else
                {
                    currentColor = color;
                    Crosshair.UpdateCrosshair();

                    tracer.AddMessage("クロスヘアの色を変更しました．", Tracer.MessageLevel.normal);
                    return;
                }
            }

            else
            {
                tracer.AddMessage(option + "は有効なオプションではありません．", Tracer.MessageLevel.error);
            }
        }

        else
        {
            tracer.AddMessage("値を3つ以上指定することはできません．", Tracer.MessageLevel.error);
        }

        // - inner function
        static CrosshairColor GetColor(string value)
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

        // - inner function
        static void SetValue(Floats.Item item, string value, Tracer tracer)
        {
            var setting = Floats.Settings[item];

            if (float.TryParse(value, out var num))
            {
                if (setting.ValidationCheck(num, tracer))
                {
                    setting.SetValue(num);
                }
            }

            else
            {
                tracer.AddMessage(value + "を有効な数値に変換できません．", Tracer.MessageLevel.error);
            }
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
