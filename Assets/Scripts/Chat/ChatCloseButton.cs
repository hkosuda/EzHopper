using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatCloseButton : MonoBehaviour
{
    void Start()
    {
        var button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(ChatArea.DeactivateFields);
    }
}
