using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sfxgen : MonoBehaviour
{
    public AudioClip SFX;
    AudioSource snd;
    bool isp=true;
    float timer;
    void Start()
    {
        snd=GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(timer>=2.0f){
            timer=0;
            isp=true;
        }
        timer+=Time.deltaTime;
    }
    public void pl(){
        if(isp){
            timer=0;
            isp=false;
            snd.PlayOneShot(SFX);
        }
    }
}
