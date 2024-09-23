using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class mnmeter : MonoBehaviour
{
    public Texture[] mn;
    public RawImage r;
    int cntr=0;
    float tm;
    public AudioClip[] SE;
    AudioSource snd;

    void Start(){
        snd=GetComponent<AudioSource>();
        cntr=0;
        tm=0.0f;
        r.texture=mn[0];
    }

    void Update(){
        if(pub.Timer<=120.0f && cntr==0){
            last2m();
        }
        if(pub.Timer<=60.0f && cntr==2){
            last1m();
        }
        if(pub.Timer<=0.0f && cntr==4){
            snd.PlayOneShot(SE[0]);
            timeup();
        }
        if(cntr==1 || cntr==3 || cntr==5){
            tm-=Time.deltaTime;
            if(tm<=0.0f){
                cntr++;
                r.texture=mn[0];
                if(cntr>=5){
                    SceneManager.LoadScene("result");
                }
            }
        }
    }

    void last2m(){
        cntr=1;
        tm=3.0f;
        r.texture=mn[1];
    }
    void last1m(){
        cntr=3;
        tm=3.0f;
        r.texture=mn[2];
    }
    void timeup(){
        cntr=5;
        tm=3.0f;
        r.texture=mn[3];
    }
}
