using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeSound : MonoBehaviour
{
    static AudioSource audioSource;

    static AudioClip shootingSound;
    static AudioClip sliderSound;

    private void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();

        shootingSound = Resources.Load<AudioClip>("DeSound/de_shooting_03");
        sliderSound = Resources.Load<AudioClip>("DeSound/de_slider");
    }

    public void PlayShootingSound()
    {
        audioSource.volume = Floats.Get(Floats.Item.volume_shooting);
        audioSource.PlayOneShot(shootingSound);
    }

    public void PlaySliderSound()
    {
        audioSource.volume = Floats.Get(Floats.Item.volume_shooting);
        audioSource.PlayOneShot(sliderSound);
    }
}
