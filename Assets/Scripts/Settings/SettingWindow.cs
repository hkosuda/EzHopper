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

    void Start()
    {
        Timer.Pause();
        DemoTimer.Pause();
    }

    private void Update()
    {
        Timer.Pause();
        DemoTimer.Pause();
    }

    private void OnDestroy()
    {
        Timer.Resume();
    }

    void Close()
    {
        Destroy(gameObject);
    }
}
