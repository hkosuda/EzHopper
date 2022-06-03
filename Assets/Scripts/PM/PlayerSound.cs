using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    static readonly int landingSoundFrameBuffer = 8;
    static readonly float footstepInterval = 0.32f;

    static float landingSoundFrameBufferRemain;
    static float footstepIntervalRemain;

    static AudioSource audioSource;
    static AudioClip landingSound;
    static AudioClip footstepSound;

    static float prevVy;

    private void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        landingSound = Resources.Load<AudioClip>("PmSound/landing");
        footstepSound = Resources.Load<AudioClip>("PmSound/footstep");
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
            PM_Landing.Landed += PlayLandingSound;
            PM_Demo.Landed += PlayLandingSoundDemo;

            Timer.Updated += UpdateMethod;
            Timer.LateUpdated += LateUpdateMethod;
        }

        else
        {
            PM_Landing.Landed -= PlayLandingSound;
            PM_Demo.Landed -= PlayLandingSoundDemo;

            Timer.Updated -= UpdateMethod;
            Timer.LateUpdated -= LateUpdateMethod;
        }
    }

    static void PlayLandingSound(object obj, RaycastHit hit)
    {
        if (landingSoundFrameBufferRemain > 0) { return; }

        if (prevVy < -2.0f)
        {
            audioSource.volume = 0.5f;
        }

        else if (prevVy < -0.1f)
        {
            audioSource.volume = 0.3f;
        }

        else
        {
            audioSource.volume = 0.1f;
        }

        audioSource.PlayOneShot(landingSound);
        landingSoundFrameBufferRemain = landingSoundFrameBuffer;
    }

    static void UpdateMethod(object obj, float dt)
    {
        landingSoundFrameBufferRemain--;
        if (landingSoundFrameBufferRemain < 0) { landingSoundFrameBufferRemain = 0; }

        if (PM_Landing.LandingIndicator < 0)
        {
            footstepIntervalRemain = footstepInterval;
            return;
        }

        if (PM_Main.Rb.velocity.magnitude < 6.0f) { return; }

        footstepIntervalRemain -= dt;

        if (footstepIntervalRemain < 0.0f)
        {
            footstepIntervalRemain = footstepInterval;
            audioSource.PlayOneShot(footstepSound);
        }
    }

    static void LateUpdateMethod(object obj, bool mute)
    {
        prevVy = PM_Main.Rb.velocity.y;
    }

    static void PlayLandingSoundDemo(object obj, bool mute)
    {
        if (PM_Main.Rb.velocity.y < -2.0f)
        {
            audioSource.volume = 0.5f;
        }

        else
        {
            audioSource.volume = 0.3f;
        }

        audioSource.PlayOneShot(landingSound);
    }
}
