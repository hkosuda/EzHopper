using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicSystem : IKernelManager
{
    public void Initialize()
    {
        SetEvent(1);
    }

    public void Shutdown()
    {
        SetEvent(-1);
    }

    static void SetEvent(int indicator)
    {
        if (indicator > 0)
        {
            ChatMessages.ChatReceived += ReactingToChat;
        }

        else
        {
            ChatMessages.ChatReceived -= ReactingToChat;
        }
    }

    public void Reset()
    {
        
    }

    static void ReactingToChat(object obj, ChatMessages.MessageSender messageSender)
    {
        if (messageSender.sender != ChatMessages.Sender.player) { return; }
        var message = messageSender.message.Trim().ToLower();

        // processing
        if (message == "i love you" || message == "i love u")
        {
            SendToxicChat("i love you");
        }
    }

    // utility
    static void SendToxicChat(string message, float minDelay = 2.0f, float maxDelay = 4.0f)
    {
        var chat = new GameObject("ToxicChat");
        GameSystem.SetChildOfRoot(chat);

        chat.AddComponent<ToxicChat>();
        chat.GetComponent<ToxicChat>().Initialize(message, minDelay, maxDelay);
    }

    static bool Rnd(float probability)
    {
        return UnityEngine.Random.Range(0.0f, 1.0f) <= probability;
    }
}
