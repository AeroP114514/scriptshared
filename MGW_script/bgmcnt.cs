using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgmcnt : MonoBehaviour
{
    public AudioClip inp;
    AudioSource snd;
    bool np=false;
    void Start()
    {
        snd=GetComponent<AudioSource>();
        snd.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if(!snd.isPlaying && np==false){
            snd.clip=inp;
            snd.loop=true;
            np=true;
            snd.Play();
        }
    }

}
