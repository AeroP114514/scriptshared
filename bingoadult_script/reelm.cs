using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reelm : MonoBehaviour
{
    public GameObject agm;
    gm d;
    public GameObject[] ss;
    sens ik;
    public int sm=0;
    int rn=0,P=1;
    float pos,timer;
    public AudioClip SFX1,SFX2;
    AudioSource snd;
    void Start()
    {
        pos=decpos(P);
        transform.position=new Vector3(0,pos,1);
        d=agm.GetComponent<gm>();
        snd=GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(sm==1){
            timer+=Time.deltaTime;
            transform.Translate(0,-30.4f*Time.deltaTime,0);
            if(transform.position.y<-20.0f){
                transform.position=new Vector3(0,1.59f,1);
            }
            if(timer>=1.1f){
                pos=decpos(P);
                sm=2;
            }
        }else if(sm==2){
            timer+=Time.deltaTime;
            transform.Translate(0,-30.4f*Time.deltaTime,0);
            if(transform.position.y<-20.0f){
                transform.position=new Vector3(0,1.59f,1);
            }
            if((transform.position.y<pos+0.08f && transform.position.y>pos-0.4f)||(timer>=2.5f)){
                sm=3;
            }
        }else if(sm==3){
            switch(P){
                case 1: d.chance(); break;
                case 2: ik=ss[2].GetComponent<sens>(); ik.slotc(); d.inchk(3); break;
                case 3: ik=ss[7].GetComponent<sens>(); ik.slotc(); d.inchk(8); break;
                case 4: ik=ss[3].GetComponent<sens>(); ik.slotc(); d.inchk(4); break;
                case 5: for(int i=1;i<8;i+=2){
                            ik=ss[i].GetComponent<sens>(); ik.slotc(); d.inchk(i+1); 
                        } break;
                case 6: ik=ss[1].GetComponent<sens>(); ik.slotc(); d.inchk(2); break;
                case 7: ik=ss[6].GetComponent<sens>(); ik.slotc(); d.inchk(7); break;
                case 8: ik=ss[4].GetComponent<sens>(); ik.slotc(); d.inchk(5); break;
                case 9: for(int h=0;h<9;h+=2){
                            ik=ss[h].GetComponent<sens>(); ik.slotc(); d.inchk(h+1); 
                        } break;
                case 10:ik=ss[5].GetComponent<sens>(); ik.slotc(); d.inchk(6); break;
                case 11:ik=ss[0].GetComponent<sens>(); ik.slotc(); d.inchk(1); break;
                case 12:ik=ss[8].GetComponent<sens>(); ik.slotc(); d.inchk(9); break;
            }
            snd.PlayOneShot(SFX2);
            sm=0;
        }else{
            transform.position=new Vector3(0,pos,1);
        }
    }
    public void A(){
        if(sm==0){
        rn=Random.Range(0,200);
        //rn=Random.Range(125,127);
        if(rn<20){
            P=11;
        }else if(rn>=20 && rn<40){
            P=6;
        }else if(rn>=40 && rn<58){
            P=8;
        }else if(rn>=58 && rn<78){
            P=2;
        }else if(rn>=78 && rn<98){
            P=4;
        }else if(rn>=98 && rn<103){
            P=5;
        }else if(rn>=103 && rn<123){
            P=10;
        }else if(rn>=123 && rn<138){
            P=1;
        }else if(rn>=138 && rn<158){
            P=7;
        }else if(rn>=158 && rn<160){
            P=9;
        }else if(rn>=160 && rn<180){
            P=3;
        }else{
            P=12;
        }
        snd.PlayOneShot(SFX1);
        timer=0;
        sm=1;
        }
    }

    float decpos(int n){
        float s;
        switch(n){
            case 1:  s=1.16f;  break;
            case 2:  s=-0.63f; break;
            case 3:  s=-2.42f; break;
            case 4:  s=-4.21f; break;
            case 5:  s=-6.03f; break;
            case 6:  s=-7.81f; break;
            case 7:  s=-9.6f; break;
            case 8:  s=-11.41f; break;
            case 9:  s=-13.2f; break;
            case 10:  s=-15.01f; break;
            case 11:  s=-16.81f; break;
            case 12:  s=-18.6f; break;
            default: s=0.0f;  break;
        }
        return s;
    }
}
