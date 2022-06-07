using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlayWindow : MonoBehaviour
{
    void Start()
    {
        var button = gameObject.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<Button>();
        button.onClick.AddListener(DestroyMyself);
    }

    void DestroyMyself()
    {
        Object.Destroy(gameObject);
    }
}
