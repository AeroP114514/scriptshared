using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reelctrl2_4 : MonoBehaviour
{
    public int spinF,PofS,r;
    public int[] mark=new int[3];
    int[] mdef={999,2,4,5,3,1,1,6,2,7,2,1,2,1,4,88,3,2,5,7,4,1,8,8,8,2,3,6,1,5,3,7,4,1,5,3,6,6,2,4,999,2}; //初期値
    int[] inmk={999,2,4,5,3,1,1,6,2,7,2,1,2,1,4,88,3,2,5,7,4,1,8,8,8,2,3,6,1,5,3,7,4,1,5,3,6,6,2,4,999,2};
    float[] PosStop={0.0f,-2.5f,-5.0f,-7.5f,-10.0f,-12.5f,-15.0f,-17.5f,
                    -20.0f,-22.5f,-25.0f,-27.5f,-30.0f,-32.5f,-35.0f,-37.5f,
                    -40.0f,-42.5f,-45.0f,-47.5f,-50.0f,-52.5f,-55.0f,-57.5f,
                    -60.0f,-62.5f,-65.0f,-67.5f,-70.0f,-72.5f,-75.0f,-77.5f,
                    -80.0f,-82.5f,-85.0f,-87.5f,-90.0f,-92.5f,-95.0f,-97.5f,0.0f};
    public float ys,Pos,tm;
    int j;
    bool sens;
    public Reelctrl1_4 RC1;
    public Reelctrl3_4 RC3;
    public GMS4 gamemaster;
    public AudioClip SFX,SFX1,SFX2;
    AudioSource snd;

    void Start()
    {
        snd=GetComponent<AudioSource>();
        tm=0.0f;
        Pos=PosStop[PofS];
        transform.position=new Vector3(-4.0f,Pos,0);
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.S)) spinF=2;
        if(spinF==1){
            if(ys>=-76.5f){
                ys-=315.0f*Time.deltaTime;
            }
            if(ys<-76.5f){
                ys=-76.5f;
            }
            transform.Translate(0,ys*Time.deltaTime,0);
            if(transform.position.y<-98.5f){
                transform.position=new Vector3(transform.position.x,transform.position.y+100.0f,0);
            }
        }else if(spinF==2){
            r=Random.Range(0,80000);
            PofS=setting(r);
            Pos=PosStop[PofS];
            transform.position=new Vector3(transform.position.x,Pos+gamemaster.gamespd,0);
            if(transform.position.y>0.0f){
                transform.position=new Vector3(transform.position.x,transform.position.y-100.0f,0);
            }
            spinF=3;
        }else if(spinF==3){
            tm+=Time.deltaTime;
            transform.Translate(0,ys*Time.deltaTime,0);
            if(transform.position.y<-98.5f){
                transform.position=new Vector3(transform.position.x,transform.position.y+100.0f,0);
            }
            if(transform.position.y<=Pos+0.3f && transform.position.y>=Pos-5.2f && tm>=0.05f){
                spinF=4;
            }
        }else if(spinF==4){
            ys=0.0f;
            tm=0.0f;
            transform.position=new Vector3(transform.position.x,Pos,0);
            spinF=0;
            snd.PlayOneShot(SFX);
            RCC();
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

    void RCC(){
        if((RC1.mark[0]==88 || RC1.mark[1]==88 || RC1.mark[2]==88)&&(mark[0]==88 || mark[1]==88 || mark[2]==88)&& !gamemaster.nowFG){
            snd.PlayOneShot(SFX2);
            RC3.spinF=6;
        }else{
            if(sens){
                snd.PlayOneShot(SFX1);
            }
            RC3.spinF=2;
        }
    }

    int setting(int c){
        sens=false;
        c%=40;
        j=c+1;
        for(int i=0;i<3;i++){
            mark[i]=inmk[c+i];
            if(mark[i]==88){
                sens=true;
            }
        }
        return j;
    }

    public void changesymbol(int tier){
        switch(tier){
            case 5: a5(); a4(); a3(); a2(); a1(); break;
            case 4: a4(); a3(); a2(); a1(); break;
            case 3: a3(); a2(); a1(); break;
            case 2: a2(); a1(); break;
            case 1: a1(); break;
            case 0: a0(); break;
            default: break;
        }
    }
    void a5(){
        for(int i=0;i<inmk.Length;i++){
            if(inmk[i]==4){
                inmk[i]=999;
            }
        }
    }
    void a4(){
        for(int i=0;i<inmk.Length;i++){
            if(inmk[i]==5){
                inmk[i]=999;
            }
        }
    }
    void a3(){
        for(int i=0;i<inmk.Length;i++){
            if(inmk[i]==6){
                inmk[i]=999;
            }
        }
    }
    void a2(){
        for(int i=0;i<inmk.Length;i++){
            if(inmk[i]==7){
                inmk[i]=999;
            }
        }
    }
    void a1(){
        for(int i=0;i<inmk.Length;i++){
            if(inmk[i]==8){
                inmk[i]=999;
            }
        }
    }
    void a0(){
        for(int i=0;i<inmk.Length;i++){
            inmk[i]=mdef[i];
        }
    }
}
