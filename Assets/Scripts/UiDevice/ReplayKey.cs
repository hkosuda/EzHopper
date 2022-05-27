using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReplayKey : MonoBehaviour
{
    static readonly Color activeColor = new Color(0.0f, 1.0f, 0.0f, 1.0f);
    static readonly Color inactiveColor = new Color(0.0f, 0.8f, 0.0f, 0.2f);

    public enum Key
    {
        f, b, l, r
    }

    TextMeshProUGUI text;
    List<Image> lines;

    bool keydown;

    private void Awake()
    {
        text = gameObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();

        lines = new List<Image>()
        {
            GetImage(0),
            GetImage(1),
            GetImage(2),
            GetImage(3),
        };

        // - innerfunction
        Image GetImage(int n)
        {
            return gameObject.transform.GetChild(0).GetChild(n).gameObject.GetComponent<Image>();
        }
    }

    void Start()
    {
        UpdateColor();
    }

    public void SetStatus(bool keydown)
    {
        this.keydown = keydown;
        UpdateColor();
    }

    void UpdateColor()
    {
        Color color;

        if (keydown)
        {
            color = activeColor;
        }

        else
        {
            color = inactiveColor;
        }

        text.color = color;

        foreach (var line in lines)
        {
            line.color = color;
        }
    }
}
