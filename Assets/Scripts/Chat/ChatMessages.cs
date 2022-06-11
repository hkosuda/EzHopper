using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatMessages : MonoBehaviour
{
    static readonly int maxChats = 25;
    static readonly float chatExistsTime = 7.0f;

    public enum Sender
    {
        none,
        player,
        unknown,
        system,
    }

    public enum TeammateColor
    {
        green,
        cyan,
        purple,
        orange,
    }

    static public EventHandler<MessageSender> ChatReceived { get; set; }

    static VerticalLayoutGroup vlGroup;
    static GameObject _chatMessage;

    static GameObject myself;
    static int frameBuffer;

    static List<GameObject> messageList;
    static List<float> timeList;

    static List<TeammateColor> teammateColorList;

    static bool dontHideMessages;

    private void Awake()
    {
        myself = gameObject;
        _chatMessage = Resources.Load<GameObject>("UiComponent/ChatMessage");

        vlGroup = gameObject.GetComponent<VerticalLayoutGroup>();

        messageList = new List<GameObject>();
        timeList = new List<float>();
    }

    private void Start()
    {
        SetEvent(1);
    }

    private void OnDestroy()
    {
        SetEvent(-1);
    }

    static void SetEvent(int indicator)
    {
        if (indicator > 0)
        {
            CommandReceiver.UnknownCommandRequest += WriteUnkownCommandMessages;
            CommandReceiver.CommandRequestEnd += WriteTracerMessages;
        }

        else
        {
            CommandReceiver.UnknownCommandRequest -= WriteUnkownCommandMessages;
            CommandReceiver.CommandRequestEnd -= WriteTracerMessages;
        }
    }

    static void WriteUnkownCommandMessages(object obj, string sentence)
    {
        var values = CommandReceiver.GetValues(sentence);
        if (values == null || values.Count == 0) { return; }

        var message = values[0] + "というコマンドは存在しません．";
        ProcessMessages(message, sentence);
    }

    static void WriteTracerMessages(object obj, Tracer tracer)
    {
        var tracerMessage = tracer.ChatMessage();
        if (tracerMessage.Trim() == "") { return; }

        SendChat(tracerMessage, Sender.system);
    }

    static void ProcessMessages(string message, string sentence)
    {
        var options = CommandReceiver.GetOptions(sentence);

        if (Tracer.CheckOption(Tracer.Option.echo, options) || Tracer.CheckOption(Tracer.Option.flash, options))
        {
            SendChat(message, Sender.system);
        }
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

            if (timeList[n] > chatExistsTime) 
            {
                timeList[n] = -1.0f;
                if (!dontHideMessages) { messageList[n].SetActive(false); }
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

        if (messageList.Count > maxChats)
        {
            for(var n = messageList.Count - maxChats; n > -1; n--)
            {
                Destroy(messageList[n]);

                timeList.RemoveAt(n);
                messageList.RemoveAt(n);
            }
        }

        ChatReceived?.Invoke(null, new MessageSender(message, sender));

        // - inner function
        static string SenderText(Sender sender)
        {
            if (sender == Sender.player)
            {
                return "<color=yellow>● </color><color=orange>[Player] : </color>";
            }

            if (sender == Sender.unknown)
            {
                return RandomTeammateColor() + "● " + "</color><color=orange>[? ? ?] : </color>";
            }

            if (sender == Sender.system)
            {
                return "<color=lime>[System] : </color>";
            }

            return "";
        }

        // - inner function
        static string RandomTeammateColor()
        {
            if (teammateColorList == null || teammateColorList.Count == 0)
            {
                teammateColorList = new List<TeammateColor>() 
                { 
                    TeammateColor.cyan, TeammateColor.green, TeammateColor.orange, TeammateColor.purple 
                };
            }

            var idx = UnityEngine.Random.Range(0, teammateColorList.Count);
            var color = "<color=" + teammateColorList[idx].ToString() + ">";

            teammateColorList.RemoveAt(idx);
            return color;
        }
    }

    static public void ShowMessages()
    {
        foreach(var message in messageList)
        {
            message.SetActive(true);
        }

        frameBuffer = 2;
        dontHideMessages = true;

        InputSystem.Inactivate();
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
        dontHideMessages = false;

        InputSystem.Activate();
    }

    public class MessageSender
    {
        public string message;
        public Sender sender;

        public MessageSender(string message, Sender sender)
        {
            this.message = message;
            this.sender = sender;
        }
    }
}
