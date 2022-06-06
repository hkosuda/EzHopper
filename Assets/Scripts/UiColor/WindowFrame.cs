using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowFrame : MonoBehaviour
{
    void Start()
    {
        var image = gameObject.GetComponent<Image>();
        image.color = new Color(0.8f, 0.8f, 0.8f, 1.0f);
    }
}
