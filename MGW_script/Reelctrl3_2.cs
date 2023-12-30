using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reelctrl3_2 : MonoBehaviour
{
    public int spinF,PofS,r,mark;
    public float ys,Pos,tm;
    int j;
    public Reelctrl1_2 RC1;
    public Reelctrl2_2 RC2;
    public spinsound ssd;
    public GMS2 gamemaster;
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
            r=Random.Range(0,61700);
            PofS=setting(r);
            Pos=PosStop();
            if((RC1.mark==10 || RC1.mark==20 || RC1.mark==999)&&(RC2.mark==10 || RC2.mark==20 || RC2.mark==999)){
                spinF=5;
                ssd.chance();
                tm+=Random.Range(0.02f,7.45f);
            }else{
                spinF=3;
            }
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
        c%=617;
        if(c<30){
            j=1;
        }else if(c>=30 && c<73){
            j=2;
        }else if(c>=73 && c<107){
            j=3;
        }else if(c>=107 && c<150){
            j=4;
        }else if(c>=150 && c<172){
            j=5;
        }else if(c>=172 && c<215){
            j=6;
        }else if(c>=215 && c<247){
            j=7;
        }else if(c>=247 && c<255){
            j=8;
        }else if(c>=255 && c<260){
            j=9;
        }else if(c>=260 && c<268){
            j=10;
        }else if(c>=268 && c<302){
            j=11;
        }else if(c>=302 && c<345){
            j=12;
        }else if(c>=345 && c<377){
            j=13;
        }else if(c>=377 && c<420){
            j=14;
        }else if(c>=420 && c<450){
            j=15;
        }else if(c>=450 && c<458){
            j=16;
        }else if(c>=458 && c<463){
            j=17;
        }else if(c>=463 && c<471){
            j=18;
        }else if(c>=471 && c<505){
            j=19;
        }else if(c>=505 && c<548){
            j=20;
        }else if(c>=548 && c<574){
            j=21;
        }else if(c>=574 && c<617){
            j=22;
        }
        switch(j){
            case 1: mark=3; break;
            case 3: mark=1; break;
            case 5: mark=20; break;
            case 7: mark=2; break;
            case 9: mark=999; break;
            case 11: mark=1; break;
            case 13: mark=2; break;
            case 15: mark=3; break;
            case 17: mark=999; break;
            case 19: mark=1; break;
            case 21: mark=10; break;
            default: mark=0; break;
        }
        return j;
    }
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
    }
}
