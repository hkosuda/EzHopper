using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class StreamingVideoLoader : MonoBehaviour
{
    [SerializeField] string relativePath = "";
    [SerializeField] int frameBuffer = 1;

    int frameBufferRemain = 1;

    private void Awake()
    {
        frameBufferRemain = frameBuffer;
    }

    void Update()
    {
        frameBufferRemain--;
        if (frameBufferRemain < 0) { frameBufferRemain = -1; return; }
        if (frameBufferRemain != 0) { return; }

        frameBufferRemain = -1;

        var player = GetComponent<VideoPlayer>();
        player.source = VideoSource.Url;
        player.url = Application.streamingAssetsPath + "/" + relativePath.Trim() + ".mp4";
        player.prepareCompleted += PrepareCompleted;
        player.Prepare();
    }
    void PrepareCompleted(VideoPlayer vp)
    {
        vp.prepareCompleted -= PrepareCompleted;
        vp.Play();
    }
}
