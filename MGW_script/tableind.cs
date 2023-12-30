using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tableind : MonoBehaviour
{
    public GameObject cn;
    public AudioClip SFX1,SFX2;
    AudioSource snd;
    bool tgl=false;

    void Start(){
        snd=GetComponent<AudioSource>();
    }

    public void tON(){
        if(tgl){
            snd.PlayOneShot(SFX2);
            tgl=false;
            cn.SetActive(false);
        }else{
            snd.PlayOneShot(SFX1);
            tgl=true;
            cn.SetActive(true);
        }
    }
    public void tOFF(){
        if(tgl){
            tgl=false;
            cn.SetActive(false);
        }
    }
}
