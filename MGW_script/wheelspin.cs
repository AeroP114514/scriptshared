using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wheelspin : MonoBehaviour
{
    float sps=0.0f,posofst,timer=0.0f,wt;
    public int flag=0;
    int smt,r;
    public AudioClip[] SFX;
    AudioSource snd;
    float[] cub={17.0f,49.63f,82.74f,114.56f,147.05f,180.79f,213.18f,246.37f,279.24f,312.06f,344.28f};
    int[] meter={2500,10,200,70,300,40,150,100,50,150,70};

    void Start(){
        snd=GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        if(flag==1){
            timer+=Time.deltaTime;
            if(sps<=525.0f){
                sps+=197.4f*Time.deltaTime;
            }
            if(sps>525.0f){
                sps=525.0f;
            }
            transform.Rotate(0,0,sps*Time.deltaTime);
            if(timer>5.2f){
                flag=2;
            }
        }else if(flag==2){
            timer=0.0f;
            r=Random.Range(0,37000);
            r%=370;
            chk();
            wt=Random.Range(5.11f,14.66f);
            flag=3;
        }else if(flag==3){
            timer+=Time.deltaTime;
            if(sps>=30.0f){
                sps-=41.2f*Time.deltaTime;
            }
            if(sps<30.0f){
                sps=30.0f;
            }
            transform.Rotate(0,0,sps*Time.deltaTime);
            if(timer>wt){
                posofst=Pos();
                flag=4;
            }
        }else if(flag==4){
            timer=0.0f;
            if(sps>=30.0f){
                sps-=41.2f*Time.deltaTime;
            }
            if(sps<30.0f){
                sps=30.0f;
            }
            transform.Rotate(0,0,sps*Time.deltaTime);
            if(transform.eulerAngles.z>=posofst && transform.eulerAngles.z<=posofst+8.0f){
                flag=5;
            }
        }else if(flag==5){
            wt=0.0f;
            sps=0.0f;
            StartCoroutine("decide");
            flag=0;
        }
    }

    IEnumerator decide(){
        yield return new WaitForSeconds(0.75f);
        GameObject fv=GameObject.Find("BGM");
        bgmMngr bm=fv.GetComponent<bgmMngr>();
        bm.StopBGM();
        snd.PlayOneShot(SFX[0]);
        yield return new WaitForSeconds(3.9f);
        GameObject go=GameObject.Find("GM");
        GMS3 gm=go.GetComponent<GMS3>();
        gm.WhDec(meter[smt]);
    }

    void chk(){
        if(r==0){
            smt=0;
        }else if(r>0 && r<=15){
            smt=1;
        }else if(r>15 && r<=40){
            smt=2;
        }else if(r>40 && r<=96){
            smt=3;
        }else if(r>96 && r<=105){
            smt=4;
        }else if(r>105 && r<=141){
            smt=5;
        }else if(r>141 && r<=185){
            smt=6;
        }else if(r>185 && r<=221){
            smt=7;
        }else if(r>221 && r<=269){
            smt=8;
        }else if(r>269 && r<=313){
            smt=9;
        }else if(r>313 && r<=369){
            smt=10;
        }
    }
    float Pos(){
        float p;
        p=cub[smt];
        return p;
    }
}
