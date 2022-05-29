using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatArea : MonoBehaviour
{
    static GameObject inputField;
    static RectTransform messagesRect;

    private void Awake()
    {
        inputField = gameObject.transform.GetChild(0).gameObject;
        messagesRect = gameObject.transform.GetChild(1).gameObject.GetComponent<RectTransform>();
    }

    void Start()
    {
        SetEvent(1);
        DeactivateFields();
    }

    private void OnDestroy()
    {
        SetEvent(-1);

        DE_Main.Suspend = false;
        PM_Main.Suspend = false;
    }

    static void SetEvent(int indicator)
    {
        if (indicator > 0)
        {
            Timer.Updated += UpdateMethod;
        }

        else
        {
            Timer.Updated -= UpdateMethod;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            DeactivateFields();
        }
    }

    static void UpdateMethod(object obj, float dt)
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ActivateFields();
        }
    }

    static void ActivateFields()
    {
        inputField.SetActive(true);
        messagesRect.offsetMin = new Vector2(0.0f, 40.0f);

        ChatInputField.Activate();
        ChatMessages.ShowMessages();

        DE_Main.Suspend = true;
        PM_Main.Suspend = true;
    }

    static void DeactivateFields()
    {
        inputField.SetActive(false);
        messagesRect.offsetMin = new Vector2(0.0f, 0.0f);

        ChatMessages.HideMessages();

        DE_Main.Suspend = false;
        PM_Main.Suspend = false;
    }
}
