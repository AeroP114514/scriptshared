using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fbgm : MonoBehaviour
{
    public AudioClip sn0,sn1,sn2;
    AudioSource bgm;
    void Start()
    {
        bgm=GetComponent<AudioSource>();
        st1();
    }
    void Update(){
        if(!bgm.isPlaying && bgm.volume==0.7f){
            bgm.clip=sn2;
            bgm.loop=true;
            bgm.Play();
        }
    }

    public void st1(){
        if(bgm.volume!=0.1f){
            bgm.clip=sn0;
            bgm.volume=0.1f;
            bgm.loop=true;
            bgm.Play();
        }
    }
    public void st2(){
        if(bgm.volume!=0.7f){
            bgm.clip=sn1;
            bgm.volume=0.7f;
            bgm.loop=false;
            bgm.Play();
        }
    }
    public void sp(){
        bgm.Stop();
    }
    public void volup(){
        bgm.volume=0.1f;
    }
    public void voldn(){
        bgm.volume=0;
    }
}
