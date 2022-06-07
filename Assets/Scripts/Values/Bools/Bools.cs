using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static  public class Bools
{
    static public bool Get(Item item)
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
            CommandReceiver.AddCommand(new BooleanCommand(setting));
        }
    }

    public enum Item
    {
        none,

        show_help,
        show_button,
        show_crosshair,
        show_velocity,
        show_vector,
        show_weapon,

        left_hand,
    }

    static public Dictionary<Item, BlSetting> Settings = new Dictionary<Item, BlSetting>()
    {
        { 
            Item.show_crosshair, new BlSetting(true, "�N���X�w�A��\������D") 
        },

        { 
            Item.show_help, new BlSetting(true, "��ʂ̒[�Ɏg�p����{�^���̃w���v��\������D")
        },

        {
            Item.show_button, new BlSetting(true, "�t�B�[���h��̃{�^����\������D")
        },

        { 
            Item.show_velocity, new BlSetting(true, "�v���C���[�̐��������̑��x�̑傫����\������D")
        },

        { 
            Item.show_vector, new BlSetting(false, "�v���C���[�̐��������̑��x��\������D") 
        },

        {
            Item.show_weapon, new BlSetting(true, "�������Ă��镐���\������D")
        },

        {
            Item.left_hand, new BlSetting(false, "�e������Ŏ��D") 
        },
    };
}
