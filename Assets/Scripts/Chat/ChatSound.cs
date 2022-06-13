using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatSound : MonoBehaviour
{
    static readonly float soundCooldown = 0.5f;

    static AudioClip _sound;
    static AudioSource audioSource;

    static float soundCooldownRemain = 0.0f;

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

    private void Update()
    {
        soundCooldownRemain -= Time.deltaTime;
        
        if (soundCooldownRemain < 0.0f)
        {
            soundCooldownRemain = -1.0f;
        }
    }

    static void PlaySound(object obj, ChatMessages.MessageSender messageSender)
    {
        if (messageSender.sender == ChatMessages.Sender.player) { return; }
        if (soundCooldownRemain > 0.0f) { return; }

        audioSource.volume = Floats.Get(Floats.Item.volume_message);
        audioSource.PlayOneShot(_sound);

        soundCooldownRemain = soundCooldown;
    }
}
