using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoTransition : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    private bool transtioned = false;

    // Update is called once per frame
    void Update()
    {
        if(videoPlayer == null || videoPlayer.clip == null)
        {
            TranstionOnInvoke();
        }
        else
        {
            if (videoPlayer.isPlaying)
            {
                if (!transtioned)
                {
                    Invoke("TranstionOnInvoke", 6);
                }
                
            }
        }
    }

    void TranstionOnInvoke()
    {
        Scenemanager.Instance.NextLevel();
        transtioned = true;

    }
}
