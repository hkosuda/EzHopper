using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingItem : MonoBehaviour
{
    public delegate void OnClickedAction();
    public delegate string ButtonTextMethod();

    Text title;
    OnClickedAction action;
    ButtonTextMethod buttonTextMethod;

    Text buttonText;

    private void Awake()
    {
        title = gameObject.transform.GetChild(0).gameObject.GetComponent<Text>();
        var button = gameObject.transform.GetChild(1).gameObject.GetComponent<Button>();
        buttonText = button.transform.GetChild(0).gameObject.GetComponent<Text>();

        button.onClick.AddListener(InvokeAction);
    }

    void Start()
    {
        UpdateContent(null, false);
        SetEvent(1);
    }

    private void OnDestroy()
    {
        SetEvent(-1);
    }

    void SetEvent(int indicator)
    {
        if (indicator > 0)
        {
            ClientParams.ParamsUdpated += UpdateContent;
        }

        else
        {
            ClientParams.ParamsUdpated -= UpdateContent;
        }
    }

    void InvokeAction()
    {
        action?.Invoke();
    }

    void UpdateContent(object obj, bool mute)
    {
        if (buttonTextMethod == null) { return; }

        buttonText.text = buttonTextMethod();
    }

    public void Initialize(string _title, OnClickedAction action, ButtonTextMethod buttonTextMethod)
    {
        title.text = _title;
        this.action = action;
        this.buttonTextMethod = buttonTextMethod;
    }
}
