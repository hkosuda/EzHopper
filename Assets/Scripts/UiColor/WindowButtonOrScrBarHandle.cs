using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowButtonOrScrBarHandle : MonoBehaviour
{
    private void Start()
    {
        var image = gameObject.GetComponent<Image>();
        image.color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
    }
}