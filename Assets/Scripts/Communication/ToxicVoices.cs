using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicVoices : MonoBehaviour
{
    static GameObject myself;
    static GameObject _toxicVC;

    static List<AudioClip> toxicVoiceList;
    static List<float> toxicDelayList;

    private void Awake()
    {
        _toxicVC = Resources.Load<GameObject>("UiComponent/ToxicVC");
        myself = gameObject;
    }

    private void Start()
    {
        SetEvent(1);
    }

    private void OnDestroy()
    {
        SetEvent(-1);
    }

    private void SetEvent(int indicator)
    {
        if (indicator > 0)
        {
            Timer.Updated += UpdateMethod;
            MapsManager.Initialized += RemoveAll;
        }

        else
        {
            Timer.Updated -= UpdateMethod;
            MapsManager.Initialized -= RemoveAll;
        }
    }

    static void RemoveAll(object obj, bool mute)
    {
        toxicVoiceList = new List<AudioClip>();
        toxicDelayList = new List<float>();
    }

    static public void AddToxcVoice(AudioClip audioClip, float volume = 0.5f, float minDelay = 0.5f, float maxDelay = 1.5f)
    {
        if (toxicVoiceList == null || toxicDelayList == null)
        {
            toxicVoiceList = new List<AudioClip>();
            toxicDelayList = new List<float>();
        }

        toxicVoiceList.Add(audioClip);
        toxicDelayList.Add(UnityEngine.Random.Range(minDelay, maxDelay));
    }

    static void UpdateMethod(object obj, float dt)
    {
        if (toxicVoiceList == null || toxicVoiceList.Count == 0) { return; }

        for(var n = toxicVoiceList.Count - 1; n > -1; n--)
        {
            toxicDelayList[n] -= dt;

            if (toxicDelayList[n] < 0.0f)
            {
                var toxicVC = Instantiate(_toxicVC);
                toxicVC.transform.SetParent(myself.transform);

                toxicVC.GetComponent<ToxicVoice>().Initialize(toxicVoiceList[n]);

                toxicVoiceList.RemoveAt(n);
                toxicDelayList.RemoveAt(n);
            }
        }
    }
}
