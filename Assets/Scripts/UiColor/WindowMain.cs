using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowMain : MonoBehaviour
{
    private void Start()
    {
        var image = gameObject.GetComponent<Image>();
        image.color = new Color(0.3f, 0.3f, 0.3f, 1.0f);
    }
}
