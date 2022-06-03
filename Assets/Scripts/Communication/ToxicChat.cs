using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicChat : MonoBehaviour
{
    float delay = 3.0f;
    string message = "";

    void Start()
    {
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
            Timer.Updated += UpdateMethod;
        }

        else
        {
            Timer.Updated -= UpdateMethod;
        }
    }

    void UpdateMethod(object obj, float dt)
    {
        delay -= dt;

        if (delay < 0.0f)
        {
            ChatMessages.SendChat(message, ChatMessages.Sender.unknown);
            Destroy(gameObject);
        }
    }

    public void Initialize(string message, float minDelay = 2.0f, float maxDelay = 4.0f)
    {
        this.message = message;

        UnityEngine.Random.InitState(Mathf.Abs(gameObject.GetInstanceID()));
        delay = UnityEngine.Random.Range(minDelay, maxDelay);
    }
}
