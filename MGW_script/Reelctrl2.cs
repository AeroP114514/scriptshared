using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reelctrl2 : MonoBehaviour
{
    public int spinF,PofS,r,mark;
    public float ys,Pos,tm;
    int j;
    public Reelctrl3 RC3;
    public AudioClip SFX;
    AudioSource snd;

    void Start()
    {
        snd=GetComponent<AudioSource>();
        tm=0.0f;
        Pos=PosStop();
        transform.position=new Vector3(0.0f,Pos,0);
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
            if(transform.position.y<-63.5f){
                transform.position=new Vector3(transform.position.x,2.4f,0);
            }
        }else if(spinF==2){
            r=Random.Range(0,62800);
            PofS=setting(r);
            Pos=PosStop();
            spinF=3;
        }else if(spinF==3){
            tm+=Time.deltaTime;
            transform.Translate(0,ys*Time.deltaTime,0);
            if(transform.position.y<-63.5f){
                transform.position=new Vector3(transform.position.x,transform.position.y+66.0f,0);
            }
            if(transform.position.y<=Pos+0.3f && transform.position.y>=Pos-4.9f && tm>=0.15f){
                spinF=4;
            }
        }else if(spinF==4){
            ys=0.0f;
            tm=0.0f;
            transform.position=new Vector3(transform.position.x,Pos,0);
            spinF=0;
            snd.PlayOneShot(SFX);
            RC3.spinF=2;
        }else{
            transform.position=new Vector3(transform.position.x,Pos,0);
        }
    }

    float PosStop(){ //最終停止位置
        float n;
        switch(PofS){
            case 1:n=0.0f; break;
            case 2:n=-3.0f; break;
            case 3:n=-6.0f; break;
            case 4:n=-9.0f; break;
            case 5:n=-12.0f; break;
            case 6:n=-15.0f; break;
            case 7:n=-18.0f; break;
            case 8:n=-21.0f; break;
            case 9:n=-24.0f; break;
            case 10:n=-27.0f; break;
            case 11:n=-30.0f; break;
            case 12:n=-33.0f; break;
            case 13:n=-36.0f; break;
            case 14:n=-39.0f; break;
            case 15:n=-42.0f; break;
            case 16:n=-45.0f; break;
            case 17:n=-48.0f; break;
            case 18:n=-51.0f; break;
            case 19:n=-54.0f; break;
            case 20:n=-57.0f; break;
            case 21:n=-60.0f; break;
            case 22:n=-63.0f; break;
            default:n=0.0f; break;
        }
        return n;
    }
    int setting(int c){
        c%=628;
        if(c<24){
            j=1;
        }else if(c>=24 && c<54){
            j=2;
        }else if(c>=54 && c<84){
            j=3;
        }else if(c>=84 && c<114){
            j=4;
        }else if(c>=114 && c<140){
            j=5;
        }else if(c>=140 && c<170){
            j=6;
        }else if(c>=170 && c<198){
            j=7;
        }else if(c>=198 && c<228){
            j=8;
        }else if(c>=228 && c<250){
            j=9;
        }else if(c>=250 && c<280){
            j=10;
        }else if(c>=280 && c<310){
            j=11;
        }else if(c>=310 && c<340){
            j=12;
        }else if(c>=340 && c<364){
            j=13;
        }else if(c>=364 && c<394){
            j=14;
        }else if(c>=394 && c<424){
            j=15;
        }else if(c>=424 && c<454){
            j=16;
        }else if(c>=454 && c<484){
            j=17;
        }else if(c>=484 && c<514){
            j=18;
        }else if(c>=514 && c<542){
            j=19;
        }else if(c>=542 && c<572){
            j=20;
        }else if(c>=572 && c<598){
            j=21;
        }else if(c>=598 && c<628){
            j=22;
        }
        switch(j){
            case 1: mark=10; break;
            case 3: mark=2; break;
            case 5: mark=30; break;
            case 7: mark=1; break;
            case 9: mark=20; break;
            case 11: mark=3; break;
            case 13: mark=10; break;
            case 15: mark=3; break;
            case 17: mark=2; break;
            case 19: mark=1; break;
            case 21: mark=30; break;
            default: mark=0; break;
        }
        return j;
    }

}
