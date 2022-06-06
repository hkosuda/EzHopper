using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowScrBarMain : MonoBehaviour
{
    private void Start()
    {
        var image = gameObject.GetComponent<Image>();
        image.color = new Color(0.6f, 0.6f, 0.6f, 1.0f);
    }
}
