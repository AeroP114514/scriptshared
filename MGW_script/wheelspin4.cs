using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wheelspin4 : MonoBehaviour
{
    float sps=0.0f,posofst,timer=0.0f,wt;
    public int flag=0;
    int smt,r;
    public AudioClip[] SFX;
    AudioSource snd;
    float[] cub={17.0f,49.63f,82.74f,114.56f,147.05f,180.79f,213.18f,246.37f,279.24f,312.06f,344.28f};
    int[] meter1={4,1,3,2,4,1,3,2,1,3,2};
    int[] meter2={77,1,8,4,6,8,6,8,6,4,6};
    int[] rfk={1,202,213,597,603,675,729,756,1152,1233,1321};

    void Start(){
        snd=GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        if(flag==1){
            timer+=Time.deltaTime;
            wt=Random.Range(4.37f,7.82f);
            flag=6;
        }else if(flag==2){
            timer=0.0f;
            r=Random.Range(0,132200);
            r%=1322;
            chk();
            wt=Random.Range(6.11f,19.66f);
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
        }else if(flag==6){
            timer+=Time.deltaTime;
            if(sps<=525.0f){
                sps+=197.4f*Time.deltaTime;
            }
            if(sps>525.0f){
                sps=525.0f;
            }
            transform.Rotate(0,0,sps*Time.deltaTime);
            if(timer>wt){
                flag=2;
            }
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
        GMS4 gm=go.GetComponent<GMS4>();
        gm.WhDec(meter1[smt],meter2[smt]);
    }

    void chk(){
        if(r==0){ //1
            smt=0;
        }else if(r>0 && r<=rfk[1]){ //202
            smt=1;
        }else if(r>rfk[1] && r<=rfk[2]){ //11
            smt=2;
        }else if(r>rfk[2] && r<=rfk[3]){ //384
            smt=3;
        }else if(r>rfk[3] && r<=rfk[4]){ //6
            smt=4;
        }else if(r>rfk[4] && r<=rfk[5]){ //72
            smt=5;
        }else if(r>rfk[5] && r<rfk[6]){ //54
            smt=6;
        }else if(r>rfk[6] && r<=rfk[7]){ //27
            smt=7;
        }else if(r>rfk[7] && r<=rfk[8]){ //396
            smt=8;
        }else if(r>rfk[8] && r<=rfk[9]){ //81
            smt=9;
        }else if(r>rfk[9] && r<=rfk[10]){ //88
            smt=10;
        }
    }
    float Pos(){
        float p;
        p=cub[smt];
        return p;
    }
}
