using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    void Start()
    {
        var closeButton = gameObject.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<Button>();
        closeButton.onClick.AddListener(Close);
    }

    void Close()
    {
        Destroy(gameObject);
    }

    void Update()
    {
        Timer.Pause();
    }

    private void OnDestroy()
    {
        Timer.Resume();
    }
}
