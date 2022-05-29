using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatMessages : MonoBehaviour
{
    public enum Sender
    {
        none,
        player,
        unknown,
        system,
    }

    static EventHandler<string> ChatReceived { get; set; }

    static VerticalLayoutGroup vlGroup;
    static GameObject _chatMessage;

    static GameObject myself;
    static int frameBuffer;

    static List<GameObject> messageList;
    static List<float> timeList;

    private void Awake()
    {
        myself = gameObject;
        _chatMessage = Resources.Load<GameObject>("UiComponent/ChatMessage");

        vlGroup = gameObject.GetComponent<VerticalLayoutGroup>();

        messageList = new List<GameObject>();
        timeList = new List<float>();
    }

    void Update()
    {
        frameBuffer--;

        if (frameBuffer == 0)
        {
            vlGroup.CalculateLayoutInputHorizontal();
            vlGroup.CalculateLayoutInputVertical();

            vlGroup.SetLayoutHorizontal();
            vlGroup.SetLayoutVertical();
        }

        if (frameBuffer < 0) { frameBuffer = 0; }

        for(var n = messageList.Count - 1; n > -1; n--)
        {
            if (timeList[n] < 0.0f) { continue; }

            timeList[n] += Time.deltaTime;

            if (timeList[n] > 7.0f) 
            {
                timeList[n] = -1.0f;
                messageList[n].SetActive(false);
            }
        }
    }

    static public void SendChat(string message, Sender sender)
    {
        if (messageList == null) { messageList = new List<GameObject>(); timeList = new List<float>(); }
        if (timeList == null) { messageList = new List<GameObject>(); timeList = new List<float>(); }

        var chatMessage = Instantiate(_chatMessage);
        chatMessage.transform.SetParent(myself.transform);

        chatMessage.GetComponent<Text>().text = SenderText(sender) + message;
        frameBuffer = 2;

        timeList.Add(0.0f);
        messageList.Add(chatMessage);

        if (messageList.Count > 10)
        {
            for(var n = messageList.Count - 10; n > -1; n--)
            {
                Destroy(messageList[n]);

                timeList.RemoveAt(n);
                messageList.RemoveAt(n);
            }
        }

        ChatReceived?.Invoke(null, message);

        // - inner function
        static string SenderText(Sender sender)
        {
            if (sender == Sender.player)
            {
                return "<color=orange>[Player] : </color>";
            }

            if (sender == Sender.unknown)
            {
                return "<color=blue>[? ? ?] : </color>";
            }

            if (sender == Sender.system)
            {
                return "<color=lime>[System] : </color>";
            }

            return "";
        }
    }

    static public void ShowMessages()
    {
        foreach(var message in messageList)
        {
            message.SetActive(true);
        }

        frameBuffer = 2;
    }

    static public void HideMessages()
    {
        for(var n = 0; n < messageList.Count; n++)
        {
            if (timeList[n] < 0.0f)
            {
                messageList[n].SetActive(false);
            }
        }

        frameBuffer = 2;
    }
}
