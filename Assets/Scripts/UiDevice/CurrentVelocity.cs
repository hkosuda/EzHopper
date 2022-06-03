using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentVelocity : MonoBehaviour
{
    static Text velocityText;

    private void Awake()
    {
        velocityText = gameObject.GetComponent<Text>();
    }

    private void Start()
    {
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
        }

        else
        {
            Timer.Updated -= UpdateMethod;
        }
    } 

    static void UpdateMethod(object obj, float dt)
    {
        var v = PM_Main.Rb.velocity;
        var pv = new Vector2(v.x, v.z);

        velocityText.text = pv.magnitude.ToString("f2") + ", " + v.y.ToString("f2");
        //velocityText.text = PM_Camera.degRotX.ToString("f2");
        //velocityText.text = PM_Landing.DeltaY.ToString("f5") + ", " + v.y.ToString("f2");
    }
}
