using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reelctrl3_3 : MonoBehaviour
{
    public int spinF,PofS,r;
    public int[] mark=new int[3];
    int[] inmk={0,3,0,2,0,88,0,1,0,0,2,0,50,0,1,0,3,0,0,10,0,999,0,3};
    public float ys,Pos,tm,Rnt;
    int j;
    public Reelctrl2_3 RC2;
    public spinsound ssd;
    public GMS3 gamemaster;
    public AudioClip SFX;
    AudioSource snd;

    void Start()
    {
        snd=GetComponent<AudioSource>();
        tm=0.0f;
        Pos=PosStop();
        transform.position=new Vector3(4.5f,Pos,0);
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.S)) spinF=2;
        if(spinF==1){
            if(ys>=-71.2f){
                ys-=365.0f*Time.deltaTime;
            }
            if(ys<-71.2f){
                ys=-71.2f;
            }
            transform.Translate(0,ys*Time.deltaTime,0);
            if(transform.position.y<-53.5f){
                transform.position=new Vector3(transform.position.x,transform.position.y+55.0f,0);
            }
        }else if(spinF==2){
            r=Random.Range(0,66000);
            PofS=setting(r);
            Pos=PosStop();
            spinF=3;
        }else if(spinF==3){
            tm+=Time.deltaTime;
            transform.Translate(0,ys*Time.deltaTime,0);
            if(transform.position.y<-53.5f){
                transform.position=new Vector3(transform.position.x,transform.position.y+55.0f,0);
            }
            if(transform.position.y<=Pos+0.3f && transform.position.y>=Pos-4.9f && tm>=0.25f){
                spinF=4;
            }
        }else if(spinF==4){
            ys=0.0f;
            tm=0.0f;
            transform.position=new Vector3(transform.position.x,Pos,0);
            spinF=0;
            snd.PlayOneShot(SFX);
            gamemaster.getstop();
        }else if(spinF==5){
            tm+=Time.deltaTime;
            ys=-7.0f;
            transform.Translate(0,ys*Time.deltaTime,0);
            if(transform.position.y<-53.5f){
                transform.position=new Vector3(transform.position.x,transform.position.y+55.0f,0);
            }
            if(transform.position.y<=Pos && transform.position.y>=Pos-1.0f && tm>=9.6f){
                spinF=4;
            }
        }else if(spinF==6){
            r=Random.Range(0,66000);
            PofS=setting(r);
            Pos=PosStop();
            ys=-24.0f;
            Rnt=Random.Range(0.6f,6.3f);
            spinF=7;
            snd.Play();
        }else if(spinF==7){
            tm+=Time.deltaTime;
            transform.Translate(0,ys*Time.deltaTime,0);
            if(transform.position.y<-53.5f){
                transform.position=new Vector3(transform.position.x,transform.position.y+55.0f,0);
            }
            if(transform.position.y<=Pos && transform.position.y>=Pos-1.0f && tm>=Rnt){
                snd.Stop();
                spinF=4;
            }
        }else if(spinF==10){
            ys=0.0f;
            transform.position=new Vector3(transform.position.x,Pos,0);
            spinF=0;
        }else if(spinF==101){
            ys=11.1f;
            transform.Translate(0,ys*Time.deltaTime,0);
            if(transform.position.y>=Pos && transform.position.y<=Pos+4.9f){
                spinF=10;
            }
        }else if(spinF==102){
            ys=-11.1f;
            transform.Translate(0,ys*Time.deltaTime,0);
            if(transform.position.y<=Pos && transform.position.y>=Pos-4.9f){
                spinF=10;
            }
        }else{
            transform.position=new Vector3(transform.position.x,Pos,0);
        }
    }

    float PosStop(){ //最終停止位置
        float n;
        switch(PofS){
            case 1:n=0.0f; break;
            case 2:n=-2.5f; break;
            case 3:n=-5.0f; break;
            case 4:n=-7.5f; break;
            case 5:n=-10.0f; break;
            case 6:n=-12.5f; break;
            case 7:n=-15.0f; break;
            case 8:n=-17.5f; break;
            case 9:n=-20.0f; break;
            case 10:n=-22.5f; break;
            case 11:n=-25.0f; break;
            case 12:n=-27.5f; break;
            case 13:n=-30.0f; break;
            case 14:n=-32.5f; break;
            case 15:n=-35.0f; break;
            case 16:n=-37.5f; break;
            case 17:n=-40.0f; break;
            case 18:n=-42.5f; break;
            case 19:n=-45.0f; break;
            case 20:n=-47.5f; break;
            case 21:n=-50.0f; break;
            case 22:n=-52.5f; break;
            default:n=0.0f; break;
        }
        return n;
    }
    int setting(int c){
        c%=22;
        j=c+1;
        for(int i=0;i<3;i++){
            mark[i]=inmk[c+i];
        }
        return j;
    }/*
    public void UP(){
        PofS--;
        Pos=PosStop();
        mark=999;
        spinF=101;
    }
    public void DOWN(){
        PofS++;
        Pos=PosStop();
        mark=999;
        spinF=102;
    }*/
}
