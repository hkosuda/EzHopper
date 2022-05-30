using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuggestButton : MonoBehaviour
{
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(OnClickMethod);
    }

    void OnClickMethod()
    {
        var text = gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text;
        ConsoleInputField.ChangeValue(text + " ");
    }
}
