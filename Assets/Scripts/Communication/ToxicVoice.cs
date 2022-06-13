using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToxicVoice : MonoBehaviour
{
    static List<ChatMessages.TeammateColor> teammateColorList;

    float playingTime;

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
            InGameTimer.Updated += UpdateMethod;
            MapsManager.Initialized += DestroyMyself;
        }

        else
        {
            InGameTimer.Updated -= UpdateMethod;
            MapsManager.Initialized -= DestroyMyself;
        }
    }

    void UpdateMethod(object obj, float dt)
    {
        playingTime -= dt;

        if (playingTime < 0.0f)
        {
            Destroy(gameObject);
        }
    }

    void DestroyMyself(object obj, bool mute)
    {
        Destroy(gameObject);
    }

    public void Initialize(AudioClip audioClip, float volume = 0.5f)
    {
        playingTime = audioClip.length + 0.5f;
        var audioSource = gameObject.GetComponent<AudioSource>();

        var text = gameObject.transform.GetChild(0).gameObject.GetComponent<Text>();
        text.text = "<color=" + GetTeammateColor() + ">Åú </color><color=orange>[? ? ?]</color>";

        audioSource.PlayOneShot(audioClip, volume);
    }

    static string GetTeammateColor()
    {
        if (teammateColorList == null || teammateColorList.Count == 0)
        {
            teammateColorList = new List<ChatMessages.TeammateColor>()
            {
                ChatMessages.TeammateColor.cyan, ChatMessages.TeammateColor.green, ChatMessages.TeammateColor.orange, ChatMessages.TeammateColor.purple
            };
        }

        var idx = UnityEngine.Random.Range(0, teammateColorList.Count);
        return teammateColorList[idx].ToString();
    }
}
