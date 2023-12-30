using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spinsound : MonoBehaviour
{
    public AudioClip[] Sbgm;
    AudioSource snd;
    int t;
    void Start()
    {
        snd=GetComponent<AudioSource>();
        t=Random.Range(0,4);
    }

    public void bgmstart(){
        t++;
        if(t>Sbgm.Length-2){t=0;}
        snd.clip=Sbgm[t];
        snd.Play();
    }
    public void bgmstop(){
        snd.Stop();
    }
    public void chance(){
        snd.Stop();
        snd.clip=Sbgm[4];
        snd.Play();
    }
}
