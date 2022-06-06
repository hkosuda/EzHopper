using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowText : MonoBehaviour
{
    private void Start()
    {
        var text = gameObject.GetComponent<Text>();
        text.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }
}
