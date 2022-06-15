using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentVelocity : MonoBehaviour
{
    static readonly Bools.Item _show = Bools.Item.show_velocity;

    static Text velocityText;
    static string currentVelocity = "";

    private void Awake()
    {
        velocityText = gameObject.GetComponent<Text>();
    }

    private void Start()
    {
        UpdateVisibility(null, false);
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
            InGameTimer.Updated += UpdateMethod;
            Bools.Settings[_show].ValueUpdated += UpdateVisibility;
        }

        else
        {
            InGameTimer.Updated -= UpdateMethod;
            Bools.Settings[_show].ValueUpdated -= UpdateVisibility;
        }
    } 

    static void UpdateVisibility(object obj, bool prev)
    {
        velocityText.gameObject.SetActive(Bools.Get(_show));
    }

    static void UpdateMethod(object obj, float dt)
    {
        var v = PM_Main.Rb.velocity;
        var pv = new Vector2(v.x, v.z).magnitude.ToString("f2");

        if (pv != currentVelocity)
        {
            currentVelocity = pv;
            velocityText.text = pv + " [m/s]";
        }
    }
}
