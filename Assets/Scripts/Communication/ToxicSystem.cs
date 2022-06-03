using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicSystem : IKernelManager
{
    static AudioClip sampleClip;

    static GameObject _toxicVC;

    public void Initialize()
    {
        sampleClip = Resources.Load<AudioClip>("DeSound/de_slider");

        

        SetEvent(1);
    }

    public void Shutdown()
    {
        _toxicVC = null;

        SetEvent(-1);
    }

    static void SetEvent(int indicator)
    {
        if (indicator > 0)
        {
            ChatMessages.ChatReceived += ReactingToChat;
            InvalidArea.CourseOut += ReactToCourseOut;
        }

        else
        {
            ChatMessages.ChatReceived -= ReactingToChat;
        }
    }

    public void Reset()
    {
        ClientParams.SetNoobOrNot(false);
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

        PlayToxicChat(sampleClip);
    }

    

    static void ReactToCourseOut(object obj, Vector3 position)
    {
        if (!ClientParams.ImNoob) { return; }

        var chatMessages = new List<string>();

        if (Rnd(0.5f)) { chatMessages.Add("noob"); }
        if (Rnd(0.5f)) { chatMessages.Add("..."); }
        if (Rnd(0.5f)) { chatMessages.Add("idiot"); }
        if (Rnd(0.5f)) { chatMessages.Add("gg"); }
        if (Rnd(0.5f)) { chatMessages.Add("???"); }
        if (Rnd(0.5f)) { chatMessages.Add("????????????????????????????"); }

        foreach (var message in chatMessages)
        {
            SendToxicChat(message, 1.0f, 2.5f);
        }

        if (chatMessages.Count == 0) { SendToxicChat("nt", 1.0f, 2.5f); }

        
    }

    // utility
    static void SendToxicChat(string message, float minDelay = 2.0f, float maxDelay = 4.0f)
    {
        var chat = new GameObject("ToxicChat");
        GameSystem.SetChildOfRoot(chat);

        chat.AddComponent<ToxicChat>();
        chat.GetComponent<ToxicChat>().Initialize(message, minDelay, maxDelay);
    }

    static void PlayToxicChat(AudioClip audioClip, float volume = 0.5f, float minDelay = 0.5f, float maxDelay = 1.5f)
    {
        ToxicVoices.AddToxcVoice(audioClip);
    }

    static bool Rnd(float probability)
    {
        return UnityEngine.Random.Range(0.0f, 1.0f) <= probability;
    }
}
