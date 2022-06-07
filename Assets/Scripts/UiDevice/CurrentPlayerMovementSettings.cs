using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentPlayerMovementSettings : MonoBehaviour
{
    static readonly Floats.Item item01 = Floats.Item.pm_max_speed_on_ground;
    static readonly Floats.Item item02 = Floats.Item.pm_max_speed_in_air;
    static readonly Floats.Item item03 = Floats.Item.pm_accel_on_ground;
    static readonly Floats.Item item04 = Floats.Item.pm_accel_in_air;
    static readonly Floats.Item item05 = Floats.Item.pm_jumping_velocity;
    static readonly Floats.Item item06 = Floats.Item.pm_gravity;
    static readonly Floats.Item item07 = Floats.Item.pm_friction_accel;

    static Text text;

    private void Awake()
    {
        text = gameObject.GetComponent<Text>();
    }

    private void Start()
    {
        UpdateContent();

        SetEvent(1);
    }

    private void OnDestroy()
    {
        SetEvent(-1);
    }

    static void SetEvent(int indicator)
    {
        if (indicator > 0)
        {
            Floats.Settings[item01].ValueUpdated += Update01;
            Floats.Settings[item02].ValueUpdated += Update02;
            Floats.Settings[item03].ValueUpdated += Update03;
            Floats.Settings[item04].ValueUpdated += Update04;
            Floats.Settings[item05].ValueUpdated += Update05;
            Floats.Settings[item06].ValueUpdated += Update06;
            Floats.Settings[item06].ValueUpdated += Update07;
        }

        else
        {
            Floats.Settings[item01].ValueUpdated -= Update01;
            Floats.Settings[item02].ValueUpdated -= Update02;
            Floats.Settings[item03].ValueUpdated -= Update03;
            Floats.Settings[item04].ValueUpdated -= Update04;
            Floats.Settings[item05].ValueUpdated -= Update05;
            Floats.Settings[item06].ValueUpdated -= Update06;
            Floats.Settings[item06].ValueUpdated -= Update07;
        }
    }

    static void Update01(object obj, float prev)
    {
        UpdateContent();
    }

    static void Update02(object obj, float prev)
    {
        UpdateContent();
    }

    static void Update03(object obj, float prev)
    {
        UpdateContent();
    }

    static void Update04(object obj, float prev)
    {
        UpdateContent();
    }

    static void Update05(object obj, float prev)
    {
        UpdateContent();
    }

    static void Update06(object obj, float prev)
    {
        UpdateContent();
    }

    static void Update07(object obj, float prev)
    {
        UpdateContent();
    }

    static void UpdateContent()
    {
        var changedPmSettings = new List<Floats.Item>();

        CheckDefault(changedPmSettings, Floats.Item.pm_max_speed_on_ground);
        CheckDefault(changedPmSettings, Floats.Item.pm_max_speed_in_air);
        CheckDefault(changedPmSettings, Floats.Item.pm_accel_on_ground);
        CheckDefault(changedPmSettings, Floats.Item.pm_accel_in_air);
        CheckDefault(changedPmSettings, Floats.Item.pm_jumping_velocity);
        CheckDefault(changedPmSettings, Floats.Item.pm_gravity);

        if (changedPmSettings.Count > 0)
        {
            var t = "Current Player Movement Settings\n";

            foreach(var item in changedPmSettings)
            {
                t += item.ToString() + " : " + Floats.Get(item).ToString() + "\n";
            }

            text.text = t;
        }

        else
        {
            text.text = "";
        }

        // - inner function
        static void CheckDefault(List<Floats.Item> list, Floats.Item item)
        {
            if (Floats.Get(item) != Floats.Settings[item].DefaultValue)
            {
                list.Add(item);
            }
        }
    }
}
