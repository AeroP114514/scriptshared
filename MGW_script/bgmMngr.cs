using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgmMngr : MonoBehaviour
{
    AudioSource bgm;
    public AudioClip sounds;
    public AudioClip[] clp=new AudioClip[10];
    bool nowpl=false;
    void Start()
    {
        bgm=GetComponent<AudioSource>();
    }

    public void StartBGM(){
        nowpl=true;
        bgm.clip=sounds;
        bgm.Play();
    }
    public void StopBGM(){
        if(nowpl){
            nowpl=false;
            bgm.Stop();
        }
    }
    public void Plms(int n){
        if(!nowpl){
            nowpl=true;
            bgm.Stop();
            bgm.clip=clp[n];
            bgm.Play();
        }
    }
    public void ret(){
        nowpl=false;
    }
}
