using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgmgen1 : MonoBehaviour
{
    public AudioClip[] bgm;
    AudioSource sn;
    bool ispl=false;

    void Start()
    {
        sn=GetComponent<AudioSource>();
    }

    public void BGMstart(){
        sn.clip=bgm[1];
        sn.Play();
    }
    public void sndPlay(int ind){
        if(!ispl){
            ispl=true;
            sn.clip=bgm[ind];
            sn.Play();
        }
    }
    public void BGMmasStop(){
        ispl=false;
        sn.Stop();
    }
}
