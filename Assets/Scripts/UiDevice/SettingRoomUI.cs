using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingRoomUI : MonoBehaviour
{
    static readonly float messageExistTime = 3.0f;
    static readonly int frameBuffer = 10;

    static float pastTime;
    static int frameBufferRemain;

    static Text triggerMessage;
    static AudioSource audioSource;

    static AudioClip sound;

    static List<IgniteCommand.Igniter> invokedIgniters;

    private void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        triggerMessage = gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>();

        triggerMessage.text = "";

        sound = Resources.Load<AudioClip>("Sound/trigger_sound");
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
            Timer.Updated += UpdateMethod;
            Timer.LateUpdated += LateUpdateMethod;

            InvalidArea.CourseOut += NotifyCourseOut;
            CheckPoint.EnterCheckpoint += NotifyEnterCheckpoint;
            CheckPoint.ExitCheckpoint += NotifyExitCheckpoint;
            CheckPoint.EnterStart += NotifyEnterStart;
            CheckPoint.ExitStart += NotifyExitStart;
            CheckPoint.EnterGoal += NotifyEnterGoal;
        }

        else
        {
            Timer.Updated -= UpdateMethod;
            Timer.LateUpdated -= LateUpdateMethod;

            InvalidArea.CourseOut -= NotifyCourseOut;
            CheckPoint.EnterCheckpoint -= NotifyEnterCheckpoint;
            CheckPoint.ExitCheckpoint -= NotifyExitCheckpoint;
            CheckPoint.EnterStart -= NotifyEnterStart;
            CheckPoint.ExitStart -= NotifyExitStart;
            CheckPoint.EnterGoal -= NotifyEnterGoal;
        }
    }

    static void UpdateMethod(object obj, float dt)
    {
        pastTime += dt;

        if (pastTime > messageExistTime)
        {
            triggerMessage.text = "";
        }
    }

    static void LateUpdateMethod(object obj, bool mute)
    {
        frameBufferRemain--;
        if (frameBufferRemain < 0) { frameBufferRemain = -1; return; }

        if (frameBufferRemain == 0)
        {
            if (invokedIgniters != null && invokedIgniters.Count > 0)
            {
                UpdateText();
                invokedIgniters = new List<IgniteCommand.Igniter>();
            }
        }
    }

    static void NotifyCourseOut(object obj, Vector3 pos)
    {
        Notify(IgniteCommand.Igniter.on_course_out);
    }

    static void NotifyEnterCheckpoint(object obj, Vector3 pos)
    {
        Notify(IgniteCommand.Igniter.on_enter_checkpoint);
    }

    static void NotifyExitCheckpoint(object obj, Vector3 pos)
    {
        Notify(IgniteCommand.Igniter.on_exit_checkpoint);
    }

    static void NotifyEnterStart(object obj, Vector3 pos)
    {
        Notify(IgniteCommand.Igniter.on_enter_start);
    }

    static void NotifyExitStart(object obj, Vector3 pos)
    {
        Notify(IgniteCommand.Igniter.on_exit_start);
    }

    static void NotifyEnterGoal(object obj, Vector3 pos)
    {
        Notify(IgniteCommand.Igniter.on_enter_goal);
    }

    static void Notify(IgniteCommand.Igniter igniter)
    {
        if (invokedIgniters == null) { invokedIgniters = new List<IgniteCommand.Igniter>(); }

        frameBufferRemain = frameBuffer;
        invokedIgniters.Add(igniter);
    }

    static void UpdateText()
    {
        var message = "";

        foreach(var igniter in invokedIgniters)
        {
            message += igniter.ToString() + "\n";
        }

        pastTime = 0.0f;
        triggerMessage.text = message;

        audioSource.volume = 0.5f;
        audioSource.PlayOneShot(sound);
    }
}
