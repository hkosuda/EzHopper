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
            Item.show_crosshair, new BlSetting(true, "クロスヘアを表示する．") 
        },

        { 
            Item.show_help, new BlSetting(true, "画面の端に使用するボタンのヘルプを表示する．")
        },

        {
            Item.show_button, new BlSetting(true, "フィールド上のボタンを表示する．")
        },

        { 
            Item.show_velocity, new BlSetting(true, "プレイヤーの水平方向の速度の大きさを表示する．")
        },

        { 
            Item.show_vector, new BlSetting(false, "プレイヤーの水平方向の速度を表示する．") 
        },

        {
            Item.show_weapon, new BlSetting(true, "所持している武器を表示する．")
        },

        {
            Item.left_hand, new BlSetting(false, "銃を左手で持つ．") 
        },
    };
}
