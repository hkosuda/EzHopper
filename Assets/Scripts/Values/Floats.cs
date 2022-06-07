using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class Floats
{
    static public float Get(Item item)
    {
        return Settings[item].CurrentValue;
    }

    static public void AddCommands()
    {
#if UNITY_EDITOR
        foreach (Item item in Enum.GetValues(typeof(Item)))
        {
            if (item == Item.none) { continue; }
            if (Settings.ContainsKey(item)) { continue; }

            Debug.LogWarning(item.ToString() + " have not been set yet.");
        }
#endif

        foreach(var setting in Settings.Keys)
        {
            CommandReceiver.AddCommand(new FlCommand(setting));
        }
    }

    public enum Item
    {
        none,

        mouse_sens,
        
        pm_max_speed_on_ground,
        pm_max_speed_in_air,
        pm_accel_on_ground,
        pm_accel_in_air,
        pm_friction_accel,
        pm_jumping_velocity,
        pm_gravity,

        god_moving_speed,
        god_moving_accel,

        crosshair_length,
        crosshair_width,
        crosshair_gap,
    }

    static public Dictionary<Item, FlSetting> Settings = new Dictionary<Item, FlSetting>()
    {
        {
            Item.mouse_sens, new FlSetting(
                1.0f,
                "マウス感度．",
                new List<FlValidation>(){new Positive() })
        },

        

        {
            Item.pm_friction_accel, new FlSetting(
                35.0f, "地面からの抵抗による加速度の大きさ",
                new List<FlValidation>() { new NotNegative() })
        },

        // player movement
        {
            Item.pm_max_speed_on_ground, new FlSetting(
                7.7f, 
                "地上でのプレイヤーの移動速度の大きさの最大値．", 
                new List<FlValidation>(){ new Positive() })
        },

        { 
            Item.pm_max_speed_in_air, new FlSetting(
                0.7f, 
                "空中でのプレイヤーの移動速度の大きさの最大値．",
                new List<FlValidation>() { new Positive() }) 
        },

        { 
            Item.pm_accel_on_ground, new FlSetting(
                55.0f, 
                "地上でのプレイヤーの加速度の大きさ．",
                new List<FlValidation>() { new NotNegative() })
        },

        {
            Item.pm_accel_in_air, new FlSetting(
                100.0f, "空中でのプレイヤーの加速度の大きさ．",
                new List<FlValidation>() { new NotNegative() })
        },

        { 
            Item.pm_jumping_velocity, new FlSetting(
                5.85f, "ジャンプの初速度の大きさ", 
                new List<FlValidation>() { new NotNegative() }) 
        },

        {
            Item.pm_gravity, new FlSetting(
                -16.0f,
                "重力加速度の値．",
                new List<FlValidation>(){ new Negative() })
        },

        // god
        {
            Item.god_moving_speed, new FlSetting(
                25.0f, "神視点モードでのカメラの移動速度の大きさ．",
                new List<FlValidation>() { new Positive() })
        },

        {
            Item.god_moving_accel, new FlSetting(
                50.0f, "神視点モードでのカメラの加速度の大きさ．",
                new List<FlValidation>() { new NotNegative() })
        },

        // crosshair 
        {
            Item.crosshair_length, new FlSetting(
                6.0f, "クロスヘアの長さ．",
                new List<FlValidation>() { new OnlyInteger(), new NotNegative(), new NSmallerThan(10.0f) })
        },

        {
            Item.crosshair_width, new FlSetting(
                2.0f, "クロスヘアの幅．",
                new List<FlValidation>() { new OnlyInteger(), new NotNegative(), new NSmallerThan(10.0f) })
        },

        {
            Item.crosshair_gap, new FlSetting(
                2.0f, "クロスヘアのギャップの大きさ．",
                new List<FlValidation>() { new OnlyInteger(), new NotNegative(), new NSmallerThan(10.0f) })
        }
    };

    class Positive : FlValidation
    {
        public override bool Check(float value, Tracer tracer = null)
        {
            if (value > 0.0f)
            {
                return true;
            }

            else
            {
                if (tracer != null) { tracer.AddMessage(GetDiscription(), Tracer.MessageLevel.error); }
                return false;
            }
        }

        public override string GetDiscription()
        {
            return "0よりも大きな値でなければなりません．";
        }
    }

    class Negative : FlValidation
    {
        public override bool Check(float value, Tracer tracer = null)
        {
            if (value < 0.0f)
            {
                return true;
            }

            else
            {
                if (tracer != null) { tracer.AddMessage("負の値でなければなりません．", Tracer.MessageLevel.error); }
                return false;
            }
        }

        public override string GetDiscription()
        {
            return "負の値でなければなりません．";
        }
    }

    class NotNegative : FlValidation
    {
        public override bool Check(float value, Tracer tracer = null)
        {
            if (value >= 0.0f)
            {
                return true;
            }

            else
            {
                if (tracer != null) { tracer.AddMessage(GetDiscription(), Tracer.MessageLevel.error); }
                return false;
            }
        }

        public override string GetDiscription()
        {
            return "0以上の値でなければなりません．";
        }
    }

    class OnlyInteger : FlValidation
    {
        public override bool Check(float value, Tracer tracer = null)
        {
            if ((int)value == value)
            {
                return true;
            }

            else
            {
                if (tracer != null) { tracer.AddMessage(GetDiscription(), Tracer.MessageLevel.error); }
                return false;
            }
        }

        public override string GetDiscription()
        {
            return "整数値で指定してください";
        }
    }

    class NSmallerThan : FlValidation
    {
        float value;

        public NSmallerThan(float value)
        {
            this.value = value;
        }

        public override bool Check(float value, Tracer tracer = null)
        {
            if (value < this.value)
            {
                return true;
            }

            else
            {
                if (tracer != null) { tracer.AddMessage(GetDiscription(), Tracer.MessageLevel.error); }
                return false;
            }
        }

        public override string GetDiscription()
        {
            return value.ToString() + "未満の値でなければなりません．";
        }
    }
}
