using System;   
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// profile ... 0 : length, 1 : width, 2 : gap, 3 : color

public class CrosshairCommand : Command
{
    public CrosshairCommand()
    {
        commandName = "crosshair";
    }

    enum CrosshairColor
    {
        none,

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

            tracer.AddMessage("���݂̃N���X�w�A�̃v���t�@�C���F" + profile, Tracer.MessageLevel.normal);
            return;
        }

        if (values.Count == 2)
        {
            var profile = values[1];

            ApplyCrosshairSettings(profile, tracer);
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
                    tracer.AddMessage(color + "�͗L���ȐF�ł͂���܂���D", Tracer.MessageLevel.error);
                    return;
                }

                else
                {
                    currentColor = color;
                    Crosshair.UpdateCrosshair();

                    tracer.AddMessage("�N���X�w�A�̐F��ύX���܂����D", Tracer.MessageLevel.normal);
                    return;
                }
            }

            else
            {
                tracer.AddMessage(option + "�͗L���ȃI�v�V�����ł͂���܂���D", Tracer.MessageLevel.error);
            }
        }

        else
        {
            tracer.AddMessage("�l��3�ȏ�w�肷�邱�Ƃ͂ł��܂���D", Tracer.MessageLevel.error);
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
                tracer.AddMessage(value + "��L���Ȑ��l�ɕϊ��ł��܂���D", Tracer.MessageLevel.error);
            }
        }
    }

    static void ApplyCrosshairSettings(string profile, Tracer tracer)
    {
        if (profile.Length == 4)
        {
            var length = profile[0].ToString();
            var width = profile[1].ToString();
            var gap = profile[2].ToString();
            var color = profile[3].ToString();

            TryUpdate(Floats.Item.crosshair_length, length, tracer);
            TryUpdate(Floats.Item.crosshair_width, width, tracer);
            TryUpdate(Floats.Item.crosshair_gap, gap, tracer);
            
            if (int.TryParse(color, out var index))
            {
                if (0 < index && index < 7)
                {
                    currentColor = (CrosshairColor)index;
                    Crosshair.UpdateCrosshair();

                    tracer.AddMessage("�v���t�@�C���̓ǂݍ��݂ɐ������܂����D", Tracer.MessageLevel.normal);
                }

                else
                {
                    tracer.AddMessage("�F�̎w�肪�L���ł͂���܂���D1����6�܂ł̐����Ŏw�肵�Ă��������D", Tracer.MessageLevel.error);
                }
            }

            else
            {
                tracer.AddMessage(color + "�𐮐��ɕϊ��ł��܂���D", Tracer.MessageLevel.error);
            }
        }

        else
        {
            tracer.AddMessage("�v���t�@�C���͒���4�̕�����łȂ���΂Ȃ�܂���D", Tracer.MessageLevel.error);
        }

        // - inner function
        static void TryUpdate(Floats.Item item, string value, Tracer tracer)
        {
            var setting = Floats.Settings[item];

            if (int.TryParse(value, out var num))
            {
                if (setting.ValidationCheck(num, tracer))
                {
                    setting.SetValue(num);
                }
            }

            else
            {
                tracer.AddMessage(value + "�𐮐��ɕϊ��ł��܂���D", Tracer.MessageLevel.error);
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
