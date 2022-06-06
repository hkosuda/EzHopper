using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    static readonly Floats.Item length = Floats.Item.crosshair_length;
    static readonly Floats.Item width = Floats.Item.crosshair_width;
    static readonly Floats.Item gap = Floats.Item.crosshair_gap;
    static readonly Bools.Item show = Bools.Item.show_crosshair;

    static RectTransform ru;
    static RectTransform rb;
    static RectTransform rr;
    static RectTransform rl;

    static Image u;
    static Image b;
    static Image r;
    static Image l;

    private void Awake()
    {
        ru = GetRect(0);
        rb = GetRect(1);
        rr = GetRect(2);
        rl = GetRect(3);

        u = GetImage(0);
        b = GetImage(1);
        r = GetImage(2);
        l = GetImage(3);


        // - inner function
        RectTransform GetRect(int n)
        {
            return gameObject.transform.GetChild(n).gameObject.GetComponent<RectTransform>();
        }

        Image GetImage(int n)
        {
            return gameObject.transform.GetChild(n).gameObject.GetComponent<Image>();
        }
    }

    void Start()
    {
        UpdateCrosshair();
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
            Floats.Settings[length].ValueUpdated += OnLengthUpdated;
            Floats.Settings[gap].ValueUpdated += OnGapUpdated;
            Bools.Settings[show].ValueUpdated += OnVisibilityUpdated;
        }

        else
        {
            Floats.Settings[length].ValueUpdated -= OnLengthUpdated;
            Floats.Settings[gap].ValueUpdated -= OnGapUpdated;
            Bools.Settings[show].ValueUpdated -= OnVisibilityUpdated;
        }
    }

    static void OnLengthUpdated(object obj, float prev)
    {
        UpdateCrosshair();
    }

    static void OnGapUpdated(object obj, float prev)
    {
        UpdateCrosshair();
    }

    static void OnVisibilityUpdated(object obj, bool prev)
    {
        UpdateCrosshair();
    }

    static public void UpdateCrosshair()
    {
        var _length = Floats.Get(length);
        var _width = Floats.Get(width);
        var _gap = Floats.Get(gap);
        var _show = Bools.Get(show);

        var color = CrosshairCommand.CurrentColor();

        ru.sizeDelta = new Vector2(_width, _length);
        rb.sizeDelta = new Vector2(_width, _length);
        rr.sizeDelta = new Vector2(_length, _width);
        rl.sizeDelta = new Vector2(_length, _width);

        ru.anchoredPosition = new Vector2(0.0f, _gap);
        rb.anchoredPosition = new Vector2(0.0f, -_gap);
        rr.anchoredPosition = new Vector2(_gap, 0.0f);
        rl.anchoredPosition = new Vector2(-_gap, 0.0f);

        u.color = color;
        b.color = color;
        r.color = color;
        l.color = color;

        ru.gameObject.SetActive(_show);
        rb.gameObject.SetActive(_show);
        rr.gameObject.SetActive(_show);
        rl.gameObject.SetActive(_show);
    }
}
