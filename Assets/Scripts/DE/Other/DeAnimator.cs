using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeAnimator : MonoBehaviour
{
    static public EventHandler<bool> PreparingEnd { get; set; }

    static Animator animator;

    void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
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
            DE_Availability.PreparingBegin += BeginPreparingAnimation;
            DE_Shooter.Shot += BeginShootingAnimation;
        }

        else
        {
            DE_Availability.PreparingBegin -= BeginPreparingAnimation;
            DE_Shooter.Shot -= BeginShootingAnimation;
        }
    }

    static void BeginPreparingAnimation(object obj, bool mute)
    {
        animator.SetTrigger("Preparing");
    }

    static void BeginShootingAnimation(object obj, Vector3 direction)
    {
        animator.SetTrigger("Shooting");
    }

    // - used in animation
    public void InvokePreparingEnd()
    {
        PreparingEnd?.Invoke(null, false);
    }
}
