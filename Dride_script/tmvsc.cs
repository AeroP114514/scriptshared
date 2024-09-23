using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class tmvsc : MonoBehaviour
{
    VideoPlayer vp;
    public VideoClip[] cl;
    int r;

    void Start()
    {
        vp=GetComponent<VideoPlayer>();
        r=Random.Range(0,3);
        vp.clip=cl[r];
    }
}
