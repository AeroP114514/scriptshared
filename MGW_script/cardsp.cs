using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cardsp : MonoBehaviour
{
    public Sprite[] cop;
    public Sprite bk;
    public int cid,cflg;
    public bool anmflg=false;
    SpriteRenderer spr;
    public float timer,ro;
    Animator an;

    void Start()
    {
        cflg=0;
        spr=GetComponent<SpriteRenderer>();
        an=GetComponent<Animator>();
        FCh(3);
    }

    // Update is called once per frame
    void Update()
    {
        if(anmflg){
            an.SetBool("hitting", true);
        }else{
            an.SetBool("hitting", false);
        }
        if(cflg==1){
            spr.sprite=bk;
            timer+=Time.deltaTime;
            ro=Mathf.Sin(timer*15);
            transform.rotation=Quaternion.Euler(0,ro*90,0);
            if(timer*30>=Mathf.PI){
                cflg=2;
            }
        }else if(cflg==2){
            transform.rotation=Quaternion.Euler(0,90,0);
            spr.sprite=cop[cid];
            cflg=3;
        }else if(cflg==3){
            timer+=Time.deltaTime;
            ro=Mathf.Sin(timer*15);
            transform.rotation=Quaternion.Euler(0,ro*90,0);
            if(timer*30>=Mathf.PI*2){
                cflg=4;
            }
        }else if(cflg==4){
            transform.rotation=Quaternion.Euler(0,0,0);
        }else if(cflg==5){
            timer+=Time.deltaTime;
            ro=Mathf.Sin(timer*15);
            transform.rotation=Quaternion.Euler(0,ro*90,0);
            if(timer*30>=Mathf.PI){
                cflg=6;
            }
        }else if(cflg==6){
            transform.rotation=Quaternion.Euler(0,90,0);
            spr.sprite=bk;
            cflg=7;
        }else if(cflg==7){
            timer+=Time.deltaTime;
            ro=Mathf.Sin(timer*15);
            transform.rotation=Quaternion.Euler(0,ro*90,0);
            if(timer*30>=Mathf.PI*2){
                cflg=4;
            }
        }else if(cflg==101){
            timer+=Time.deltaTime;
            ro=Mathf.Sin(timer*30);
            transform.rotation=Quaternion.Euler(0,ro*90,0);
            if(timer>=0.5f){
                spr.sprite=cop[cid];
                cflg=4;
            }
        }
    }

    public void FCh(int n){ //1で裏→表、2で表→裏、0or3は即時
        if(n==1){
            timer=0;
            cflg=1;
        }else if(n==2){
            timer=0;
            cflg=5;
        }else if(n==3){
            spr.sprite=cop[cid];
        }else if(n==9){
            timer=0;
            cflg=101;
        }else{
            spr.sprite=bk;
        }
    }
}
