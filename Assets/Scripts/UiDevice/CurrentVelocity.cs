using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentVelocity : MonoBehaviour
{
    static readonly Bools.Item _show = Bools.Item.show_velocity;

    static Text velocityText;

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
            Timer.Updated += UpdateMethod;
            Bools.Settings[_show].ValueUpdated += UpdateVisibility;
        }

        else
        {
            Timer.Updated -= UpdateMethod;
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
        var pv = new Vector2(v.x, v.z);

        velocityText.text = pv.magnitude.ToString("f2") + " [m/s]";
    }
}
