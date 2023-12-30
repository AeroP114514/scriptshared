using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgmMngr : MonoBehaviour
{
    public AudioClip[] bgms;
    AudioSource M;
    bool np=false,rc=false;
    int scnt;
    void Start()
    {
        M=GetComponent<AudioSource>();
        scnt=Random.Range(0,304);
    }

    public void PS(){
        if(np==false){
            np=true;
            M.clip=bgms[scnt%19];
            M.Play();
            scnt+=Random.Range(1,15);
        }
    }
    public void GS(){
        if(rc==false){
            M.Stop();
            rc=true;
            M.clip=bgms[scnt%16+19];
            M.Play();
        }
    }
    public void SS(){
        if(np){
            M.Stop();
            rc=false;
            np=false;
        }
    }
}
