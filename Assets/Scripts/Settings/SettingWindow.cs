using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingWindow : MonoBehaviour
{
    private void Awake()
    {
        var closeButton = gameObject.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<Button>();
        closeButton.onClick.AddListener(Close);
    }

    void Close()
    {
        Destroy(gameObject);
    }
}
