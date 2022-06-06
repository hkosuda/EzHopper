using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeAnimator : MonoBehaviour
{
    static readonly Bools.Item visibility = Bools.Item.show_weapon;

    static public EventHandler<bool> PreparingEnd { get; set; }

    static Animator animator;
    static GameObject meshGroup;

    void Awake()
    {
        meshGroup = gameObject.transform.GetChild(0).gameObject;
        animator = gameObject.GetComponent<Animator>();

        UpdateVisibility(null, false);

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

            Bools.Settings[visibility].ValueUpdated += UpdateVisibility;
        }

        else
        {
            DE_Availability.PreparingBegin -= BeginPreparingAnimation;
            DE_Shooter.Shot -= BeginShootingAnimation;

            Bools.Settings[visibility].ValueUpdated -= UpdateVisibility;
        }
    }

    static void UpdateVisibility(object obj, bool prev)
    {
        meshGroup.SetActive(Bools.Get(visibility));
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
