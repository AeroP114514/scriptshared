using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgmMngr : MonoBehaviour
{
    AudioSource bgm;
    public AudioClip sounds;
    public AudioClip[] clp=new AudioClip[10],clp2,featureAudio;
    bool nowpl,freeSoundFlag;
    int fl=0;
    public GameMain gameMain;
    void Start()
    {
        bgm=GetComponent<AudioSource>();
        fl=Random.Range(0,10);
    }

    public void StartBGM(){
        nowpl=true;
        bgm.volume=0.4f;
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

    public void ReelSpinBGM(){
        if(gameMain.chanceState==GameMain.ChanceState.NORMAL){
            nowpl=true;
            bgm.loop=true;
            bgm.volume=0.4f;
            bgm.Stop();
            bgm.clip=clp[fl];
            fl++;
            if(fl>13){
                fl=0;
            }
            bgm.Play();
        }
    }
    public void fanf(int f){
        nowpl=true;
        bgm.volume=1.0f;
        bgm.Stop();
        bgm.loop=false;
        bgm.clip=clp2[f];
        bgm.Play();
    }
}
