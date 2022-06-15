using System;   
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// profile ... 0 : length, 1 : width, 2 : gap, 3 : color

public class CrosshairCommand : Command
{
    static readonly List<string> profiles = new List<string>()
    {
        "length", "width", "gap", "color"
    };

    public CrosshairCommand()
    {
        commandName = "crosshair";
        description = "�N���X�w�A�̐ݒ���s���܂��D";
        detail = "'crosshair <profile>' �����s����ƁC�v���t�@�C���Ɋ�Â��N���X�w�A�̐ݒ���ꊇ�ŕύX�ł��܂��i<profile>�͒���10�̐����̕�����ł��j�D" +
            "�v���t�@�C���́C'crosshair' �����s���邱�ƂŎ擾�ł��܂��D\n" +
            "'crosshair length <value>' �����s����ƁC�N���X�w�A�̒������w��ł��܂��i<value>�ɂ͔C�ӂ̐������w�肵�Ă��������j�D" +
            "'crosshair width <value>' �̓N���X�w�A�̕��C'crosshair gap <value>' �̓N���X�w�A�̊Ԋu�C'crosshair color <value>' �̓N���X�w�A�̐F���w��ł��܂��D" +
            "�Ȃ��C�N���X�w�A�̐F��1����6�܂ł̐��������p�\�ŁC���̒l�͐F�ɑΉ����Ă��܂��D�l�ƐF�̑Ή��͉��L�̒ʂ�ł��D\n" +
            "1: ���C2: �ԁC3: �}�[���^�C4: ���F�C5: �΁C6: �V�A��";
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
            return profiles;
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

    public override void CommandMethod(Tracer tracer, List<string> values, List<string> options)
    {
        if (values == null || values.Count == 0) { return; }

        if (values.Count == 1)
        {
            var length = Floats.Get(Floats.Item.crosshair_length);
            var width = Floats.Get(Floats.Item.crosshair_width);
            var gap = Floats.Get(Floats.Item.crosshair_gap);
            var colorIndex = (int)currentColor;

            var lengthStr = PaddingZeros(Mathf.RoundToInt(length));
            var widthStr = PaddingZeros(Mathf.RoundToInt(width));
            var gapStr = PaddingZeros(Mathf.RoundToInt(gap));
            var colorStr = Mathf.RoundToInt(colorIndex).ToString();

            var profile = lengthStr + widthStr + gapStr + colorStr;

            AddMessage("���݂̃N���X�w�A�̃v���t�@�C���F" + profile, Tracer.MessageLevel.normal, tracer, options);
        }

        else if (values.Count == 2)
        {
            var profile = values[1];
            ApplyCrosshairSettings(profile, tracer, options);
        }

        else if (values.Count == 3)
        {
            var option = values[1];
            var value = values[2];

            if (option == "length")
            {
                SetValue(Floats.Item.crosshair_length, value, tracer, options);
            }

            else if (option == "width")
            {
                SetValue(Floats.Item.crosshair_width, value, tracer, options);
            }

            else if (option == "gap")
            {
                SetValue(Floats.Item.crosshair_gap, value, tracer, options);
            }

            else if (option == "color")
            {
                var color = GetColor(value);

                if (color == CrosshairColor.none)
                {
                    AddMessage(color + "�͗L���ȐF�ł͂���܂���D", Tracer.MessageLevel.error, tracer, options);
                }

                else
                {
                    currentColor = color;
                    Crosshair.UpdateCrosshair();
                    AddMessage("�N���X�w�A�̐F��ύX���܂����D", Tracer.MessageLevel.normal, tracer, options);
                }
            }

            else
            {
                AddMessage(ERROR_AvailableOnly(1, profiles) + "�������́C�N���X�w�A�̃v���t�@�C�����w�肵�Ă��������D", Tracer.MessageLevel.error, tracer, options);
            }
        }

        else
        {
            AddMessage(ERROR_OverValues(3), Tracer.MessageLevel.error, tracer, options);
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
        static void SetValue(Floats.Item item, string value, Tracer tracer, List<string> options)
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
                AddMessage(value + "��L���Ȑ��l�ɕϊ��ł��܂���D", Tracer.MessageLevel.error, tracer, options);
            }
        }
    }

    static string PaddingZeros(int num)
    {
        if (num < 10)
        {
            return "00" + num.ToString();
        }

        else if (num < 100)
        {
            return "0" + num.ToString();
        }

        else
        {
            return num.ToString();
        }
    }

    static void ApplyCrosshairSettings(string profile, Tracer tracer, List<string> options)
    {
        if (profile.Length == 10)
        {
            var length = profile[0].ToString() + profile[1].ToString() + profile[2].ToString();
            var width = profile[3].ToString() + profile[4].ToString() + profile[5].ToString();
            var gap = profile[6].ToString() + profile[7].ToString() + profile[8].ToString();
            var color = profile[9].ToString();

            TryUpdate(Floats.Item.crosshair_length, length, tracer, options);
            TryUpdate(Floats.Item.crosshair_width, width, tracer, options);
            TryUpdate(Floats.Item.crosshair_gap, gap, tracer, options);
            
            if (int.TryParse(color, out var index))
            {
                if (0 < index && index < 7)
                {
                    currentColor = (CrosshairColor)index;
                    Crosshair.UpdateCrosshair();

                    AddMessage("�v���t�@�C���̓ǂݍ��݂ɐ������܂����D", Tracer.MessageLevel.normal, tracer, options);
                }

                else
                {
                    AddMessage("�F�̎w�肪�L���ł͂���܂���D1����6�܂ł̐����Ŏw�肵�Ă��������D", Tracer.MessageLevel.error, tracer, options);
                }
            }

            else
            {
                AddMessage(color + "�𐮐��ɕϊ��ł��܂���D", Tracer.MessageLevel.error, tracer, options);
            }
        }

        else
        {
            AddMessage("�v���t�@�C���͒���10�̕�����łȂ���΂Ȃ�܂���D", Tracer.MessageLevel.error, tracer, options);
        }

        // - inner function
        static void TryUpdate(Floats.Item item, string value, Tracer tracer, List<string> options)
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
                AddMessage(value + "�𐮐��ɕϊ��ł��܂���D", Tracer.MessageLevel.error, tracer, options);
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
