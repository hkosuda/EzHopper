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

        observer_moving_speed,
        observer_moving_accel,

        crosshair_length,
        crosshair_width,
        crosshair_gap,

        recorder_limit_time,

        volume_shooting,
        volume_footstep,
        volume_landing,
        volume_message,
    }

    static public Dictionary<Item, FlSetting> Settings = new Dictionary<Item, FlSetting>()
    {
        {
            Item.mouse_sens, new FlSetting(
                1.6f,
                "�}�E�X���x�D",
                new List<FlValidation>(){new Positive() })
        },

        

        {
            Item.pm_friction_accel, new FlSetting(
                35.0f, "�n�ʂ���̒�R�ɂ������x�̑傫��",
                new List<FlValidation>() { new NotNegative() })
        },

        // player movement
        {
            Item.pm_max_speed_on_ground, new FlSetting(
                7.7f, 
                "�n��ł̃v���C���[�̈ړ����x�̑傫���̍ő�l�D", 
                new List<FlValidation>(){ new Positive(), new NSmallerThan(100.0f) })
        },

        { 
            Item.pm_max_speed_in_air, new FlSetting(
                0.7f, 
                "�󒆂ł̃v���C���[�̈ړ����x�̑傫���̍ő�l�D",
                new List<FlValidation>() { new Positive(), new NSmallerThan(100.0f) }) 
        },

        { 
            Item.pm_accel_on_ground, new FlSetting(
                55.0f, 
                "�n��ł̃v���C���[�̉����x�̑傫���D",
                new List<FlValidation>() { new NotNegative(), new NSmallerThan(1000.0f) })
        },

        {
            Item.pm_accel_in_air, new FlSetting(
                100.0f, "�󒆂ł̃v���C���[�̉����x�̑傫���D",
                new List<FlValidation>() { new NotNegative(), new NSmallerThan(1000.0f) })
        },

        { 
            Item.pm_jumping_velocity, new FlSetting(
                5.85f, "�W�����v�̏����x�̑傫��", 
                new List<FlValidation>() { new NotNegative(), new NSmallerThan(30.0f) }) 
        },

        {
            Item.pm_gravity, new FlSetting(
                16.0f,
                "�d�͉����x�̑傫���D",
                new List<FlValidation>(){ new NotNegative(), new NSmallerThan(100.0f) })
        },

        // observer
        {
            Item.observer_moving_speed, new FlSetting(
                25.0f, "�_���_���[�h�ł̃J�����̈ړ����x�̑傫���D",
                new List<FlValidation>() { new Positive(), new NSmallerThan(1000.0f) })
        },

        {
            Item.observer_moving_accel, new FlSetting(
                50.0f, "�_���_���[�h�ł̃J�����̉����x�̑傫���D",
                new List<FlValidation>() { new NotNegative(), new NSmallerThan(10000.0f) })
        },

        // crosshair 
        {
            Item.crosshair_length, new FlSetting(
                6.0f, "�N���X�w�A�̒����D",
                new List<FlValidation>() { new OnlyInteger(), new NotNegative(), new NSmallerThan(1000.0f) })
        },

        {
            Item.crosshair_width, new FlSetting(
                2.0f, "�N���X�w�A�̕��D",
                new List<FlValidation>() { new OnlyInteger(), new NotNegative(), new NSmallerThan(1000.0f) })
        },

        {
            Item.crosshair_gap, new FlSetting(
                2.0f, "�N���X�w�A�̃M���b�v�̑傫���D",
                new List<FlValidation>() { new OnlyInteger(), new NotNegative(), new NSmallerThan(1000.0f) })
        },

        {
            Item.recorder_limit_time, new FlSetting(
                120.0f, "���R�[�_�[�������Œ�~����܂ł̎��ԁD",
                new List<FlValidation>() { new NLargerThan(10.0f), new NSmallerThan(600.0f) })
        },

        // volume
        {
            Item.volume_shooting, new FlSetting(
                0.15f, "�ˌ����̑傫���D",
                new List<FlValidation>() { new ZeroToOne() })
        },

        {
            Item.volume_footstep, new FlSetting(
                0.4f, "�����̑傫���D",
                new List<FlValidation>() { new ZeroToOne() })
        },

        {
            Item.volume_landing, new FlSetting(
                0.5f, "���n���̑傫���D",
                new List<FlValidation>() { new ZeroToOne() })
        },

        {
            Item.volume_message, new FlSetting(
                0.3f, "���b�Z�[�W�̒ʒm���̑傫���D",
                new List<FlValidation>() { new ZeroToOne() })
        },
    };

    class Positive : FlValidation
    {
        public override bool Check(float value, Tracer tracer = null, List<string> options = null)
        {
            if (value > 0.0f)
            {
                return true;
            }

            else
            {
                if (tracer != null) { Command.AddMessage(GetDiscription(), Tracer.MessageLevel.error, tracer, options); }
                return false;
            }
        }

        public override string GetDiscription()
        {
            return "0�����傫�Ȓl�łȂ���΂Ȃ�܂���D";
        }
    }

    class Negative : FlValidation
    {
        public override bool Check(float value, Tracer tracer = null, List<string> options = null)
        {
            if (value < 0.0f)
            {
                return true;
            }

            else
            {
                if (tracer != null) { Command.AddMessage("���̒l�łȂ���΂Ȃ�܂���D", Tracer.MessageLevel.error, tracer, options); }
                return false;
            }
        }

        public override string GetDiscription()
        {
            return "���̒l�łȂ���΂Ȃ�܂���D";
        }
    }

    class NotNegative : FlValidation
    {
        public override bool Check(float value, Tracer tracer = null, List<string> options = null)
        {
            if (value >= 0.0f)
            {
                return true;
            }

            else
            {
                if (tracer != null) { Command.AddMessage(GetDiscription(), Tracer.MessageLevel.error, tracer, options); }
                return false;
            }
        }

        public override string GetDiscription()
        {
            return "0�ȏ�̒l�łȂ���΂Ȃ�܂���D";
        }
    }

    class OnlyInteger : FlValidation
    {
        public override bool Check(float value, Tracer tracer = null, List<string> options = null)
        {
            if ((int)value == value)
            {
                return true;
            }

            else
            {
                if (tracer != null) { Command.AddMessage(GetDiscription(), Tracer.MessageLevel.error, tracer, options); }
                return false;
            }
        }

        public override string GetDiscription()
        {
            return "�����l�Ŏw�肵�Ă�������";
        }
    }

    class NSmallerThan : FlValidation
    {
        float value;

        public NSmallerThan(float value)
        {
            this.value = value;
        }

        public override bool Check(float value, Tracer tracer = null, List<string> options = null)
        {
            if (value < this.value)
            {
                return true;
            }

            else
            {
                if (tracer != null) { Command.AddMessage(GetDiscription(), Tracer.MessageLevel.error, tracer, options); }
                return false;
            }
        }

        public override string GetDiscription()
        {
            return value.ToString() + "�����̒l�łȂ���΂Ȃ�܂���D";
        }
    }

    class NLargerThan : FlValidation
    {
        float value;

        public NLargerThan(float value)
        {
            this.value = value;
        }

        public override bool Check(float value, Tracer tracer = null, List<string> options = null)
        {
            if (value > this.value)
            {
                return true;
            }

            else
            {
                if (tracer != null) { Command.AddMessage(GetDiscription(), Tracer.MessageLevel.error, tracer, options); }
                return false;
            }
        }

        public override string GetDiscription()
        {
            return value.ToString() + "�����傫�Ȓl�łȂ���΂Ȃ�܂���D";
        }
    }

    class ZeroToOne : FlValidation
    {
        public override bool Check(float value, Tracer tracer = null, List<string> options = null)
        {
            if (0.0f <= value && value <= 1.0f)
            {
                return true;
            }

            else
            {
                if (tracer != null) { Command.AddMessage(GetDiscription(), Tracer.MessageLevel.error, tracer, options); }
                return false;
            }
        }

        public override string GetDiscription()
        {
            return "0�ȏ�1�ȉ��̒l�łȂ���΂Ȃ�܂���D";
        }
    }
}
