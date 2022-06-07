using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Confirmation : MonoBehaviour
{
    public delegate void Action();

    static GameObject _confirmation;

    static Text messageText;
    static GameObject myself;

    static Action action;
    static string message;

    private void Awake()
    {
        myself = gameObject;
        var frame = gameObject.transform.GetChild(0).GetChild(0).GetChild(0);

        messageText = GetText(frame, 0);

        var yesButton = GetButton(frame, 1);
        var noButton = GetButton(frame, 2);

        yesButton.onClick.AddListener(InvokeAction);
        noButton.onClick.AddListener(DestroyMyself);

        // - inner function
        Text GetText(Transform frame, int n)
        {
            return frame.GetChild(n).GetChild(0).gameObject.GetComponent<Text>();
        }

        Button GetButton(Transform frame, int n)
        {
            return frame.GetChild(n).gameObject.GetComponent<Button>();
        }
    }

    private void Start()
    {
        messageText.text = message;
    }

    static void InvokeAction()
    {
        action?.Invoke();
        DestroyMyself();
    }

    static void DestroyMyself()
    {
        Destroy(myself);
    }

    static public void BeginConfirmation(string _message, Action _action)
    {
        if (_confirmation == null) { _confirmation = Resources.Load<GameObject>("UI/Confirmation"); }
        Instantiate(_confirmation);

        action = null;

        message = _message;
        action = _action;
    }
}
