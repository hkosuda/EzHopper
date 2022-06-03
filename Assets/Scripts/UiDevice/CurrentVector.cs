using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentVector : MonoBehaviour
{
    static RectTransform isJumping;
    static RectTransform inputVector;
    static RectTransform mainVector;
    static RectTransform addVector;

    private void Awake()
    {
        isJumping = GetRect(0);
        inputVector = GetRect(1);
        mainVector = GetRect(2);
        addVector = GetRect(3);

        // - inner function
        RectTransform GetRect(int n)
        {
            return gameObject.transform.GetChild(0).GetChild(n).gameObject.GetComponent<RectTransform>();
        }
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
        }

        else
        {
            Timer.Updated -= UpdateMethod;
        }
    }

    static void UpdateMethod(object obj, float dt)
    {
        ActivateIfJumping(isJumping);
        UpdateVector(inputVector, PM_InputVector.InputVector, 150.0f, 2.0f);
        UpdateVector(mainVector, PmUtil.NextVector, 10.0f, 4.0f);
        UpdateVector(addVector, PmUtil.AddVector, 100.0f, 4.0f);

        // - inner function
        static void ActivateIfJumping(RectTransform rect)
        {
            var landingIndicator = PM_Landing.LandingIndicator;

            if (landingIndicator < 0) { isJumping.gameObject.SetActive(true); }
            else { isJumping.gameObject.SetActive(false); }
        }

        // - inner function
        static void UpdateVector(RectTransform rect, Vector2 vec, float magnification, float width)
        {
            var rotY = PM_Camera.EulerAngles().y * Mathf.Deg2Rad;

            var vm = vec.y * Mathf.Cos(rotY) + vec.x * Mathf.Sin(rotY);
            var vl = -vec.y * Mathf.Sin(rotY) + vec.x * Mathf.Cos(rotY);

            var angle = -Mathf.Atan2(vl, vm) * Mathf.Rad2Deg;
            rect.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, angle));
            rect.sizeDelta = new Vector2(width, vec.magnitude * magnification);
        }
    }
}