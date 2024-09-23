using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reelscr : MonoBehaviour
{
    public int reelmode,rpos,ra,reelid;
    float tgpos,timer;
    public float spd;
    float[] p1={-6.0f,-14.0f,-22.0f,-30.0f},p2={-2.0f,-10.0f,-20.0f,-26.0f},p3={-4.0f,-12.0f,-18.0f,-28.0f};
    public int sym;
    public bool stopchk,fac,sev,rsfx;
    public bgmMngr bgms;
    public AudioClip[] se;
    AudioSource snd;

    void Start()
    {
        reelmode=0;
        timer=0.0f;
        spd=-18.4f;
        stopchk=true;
        snd=GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(reelmode){
            case 1: transform.Translate(0.0f,2.9f*Time.deltaTime,0.0f); timer+=Time.deltaTime; break;
            case 2: timer+=Time.deltaTime; break;
            case 3: transform.Translate(0.0f,spd*Time.deltaTime,0.0f); if(transform.position.y<-31.8f){transform.position=new Vector2(transform.position.x,transform.position.y+32.0f);} timer+=Time.deltaTime; break;
            case 4: transform.Translate(0.0f,spd*Time.deltaTime,0.0f); timer+=Time.deltaTime; break;
            case 5: transform.Translate(0.0f,3.1f*Time.deltaTime,0.0f); break;
            default: break;
        }
        if(reelmode==1 && timer>=0.12f){
            timer=0.0f;
            reelmode=2;
        }
        if(reelmode==2 && timer>=0.12f){
            timer=0.0f;
            reelmode=3;
            if(reelid==1 || reelid==2){
                bgms.reelbgm();
                if(reelid==1){
                    Time.timeScale=pub.gameSpeed;
                }
            }
        }
        if(reelmode==4 && transform.position.y<tgpos-0.48f){
            reelmode=5;
            transform.position=new Vector2(transform.position.x,tgpos-0.48f);
            if(rsfx || reelid==1 || reelid==2){
                snd.PlayOneShot(se[0]);
            }
        }
        if(reelmode==5 && transform.position.y>=tgpos){
            reelmode=0;
            transform.position=new Vector2(transform.position.x,tgpos);
            stopchk=true;
            timer=0.0f;
        }
    }

    public void reelstart(){
        timer=0.0f;
        stopchk=false;
        rsfx=true;
        reelmode=1;
    }
    public void reelstop(int f,bool sc){
        timer=0.0f;
        sym=f;
        switch(sym){
            case 0: ra=Random.Range(0,16); tgpos=(ra*-2.0f)-1.0f; fac=false; sev=false; break;
            case 1: ra=Random.Range(0,4); tgpos=p1[ra]; fac=true; sev=false; break;
            case 2: ra=Random.Range(0,4); tgpos=p2[ra]; fac=true; sev=false; break;
            case 3: ra=Random.Range(0,4); tgpos=p3[ra]; fac=true; sev=false; break;
            case 10:ra=Random.Range(0,2); tgpos=(ra*-16.0f)-8.0f; fac=false; sev=true; break;
            case 20:tgpos=-16.0f; fac=false; sev=true; break;
            case 999:tgpos=0.0f; fac=true; sev=true; break;
        }
        transform.position=new Vector2(transform.position.x,tgpos+4.4f);
        rsfx=sc;
        reelmode=4;
    }
}
