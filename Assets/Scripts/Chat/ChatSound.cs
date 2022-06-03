using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatSound : MonoBehaviour
{
    static AudioClip _sound;
    static AudioSource audioSource;

    private void Awake()
    {
        if (_sound == null)
        {
            _sound = Resources.Load<AudioClip>("Sound/chat_sound");
        }

        audioSource = gameObject.GetComponent<AudioSource>();
    }

    void Start()
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
            ChatMessages.ChatReceived += PlaySound;
        }

        else
        {
            ChatMessages.ChatReceived -= PlaySound;
        }
    }

    static void PlaySound(object obj, ChatMessages.MessageSender messageSender)
    {
        if (messageSender.sender == ChatMessages.Sender.player) { return; }

        audioSource.PlayOneShot(_sound);
    }
}
