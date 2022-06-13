using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatMessage : MonoBehaviour
{
    static readonly int frameBuffer = 2;

    VerticalLayoutGroup vlGroup;
    int frameBufferRemain;

    private void Awake()
    {
        vlGroup = gameObject.GetComponent<VerticalLayoutGroup>();
    }

    void Update()
    {
        frameBufferRemain--;
        if (frameBufferRemain < 0) { frameBufferRemain = -1; }

        if (frameBufferRemain == 0)
        {
            vlGroup.CalculateLayoutInputHorizontal();
            vlGroup.CalculateLayoutInputVertical();

            vlGroup.SetLayoutHorizontal();
            vlGroup.SetLayoutVertical();
        }
    }

    public void SetMessage(string message)
    {
        var text = gameObject.transform.GetChild(0).gameObject.GetComponent<Text>();
        text.text = message;

        frameBufferRemain = frameBuffer;
    }
}
