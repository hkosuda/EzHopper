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

        //shootingSound = Resources.Load<AudioClip>("DeSound/de_shot");
        shootingSound = Resources.Load<AudioClip>("DeSound/de_shooting_huge");
        sliderSound = Resources.Load<AudioClip>("DeSound/de_slider");
    }

    public void PlayShootingSound()
    {
        //audioSource.clip = shootingSound;
        //audioSource.Play();
        audioSource.PlayOneShot(shootingSound);
    }

    public void PlaySliderSound()
    {
        audioSource.clip = sliderSound;
        audioSource.PlayOneShot(sliderSound);
    }
}
