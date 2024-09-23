using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class gm : MonoBehaviour
{
    public bool ingame=false,introchk;
    public Animator fan,pa,fk1,fk2,chu;
    public fdCtrl FC;
    public int gamemode=0;
    public TextMeshProUGUI cd,tmeter;
    public GameObject[] sens;
    public GameObject Nvg,Ptk;
    int r,prevtg=-1,ip;
    float tm;
    Rigidbody ri;
    public cmq CQ;
    public moving MV;
    public bgmgen1 BGM;
    public nvgt navi;
    public AudioClip[] SE; 
    AudioSource snd;

    void Start()
    {
        tm=3.0f;
        ip=3;
        pub.Timer=180.0f;
        pub.Score=0;
        snd=GetComponent<AudioSource>();
        if(!introchk){
            Nvg.SetActive(false);
            introchk=true;
            gamemode=1;
            for(int i=0;i<sens.Length;i++){
                sens[i].SetActive(false);
            }
            pa.SetTrigger("st");
            chu.SetTrigger("on");
            FC.setsumei();
        }else{
            gamemode=3;
            pa.SetTrigger("in");
            nxtg();
        }
    }

    void Update()
    {
        tmeter.text=Mathf.Ceil(pub.Timer).ToString("F0")+"sec.";
        if(gamemode==2){
            tm-=Time.deltaTime;
            cd.text=Mathf.Ceil(tm).ToString("F0");
            if(tm<ip && ip>=1){
                ip--;
                snd.PlayOneShot(SE[5]);
            }
            if(tm<=0.0f){
                gamemode=3;
                cd.text="GO!!";
                snd.PlayOneShot(SE[1]);
                nxtg();
            }
        }
        if(gamemode==3){
            BGM.BGMstart();
            Nvg.SetActive(true);
            Ptk.SetActive(true);
            gamemode=4;
            ingame=true;
        }
        if(gamemode==4){
            tm-=Time.deltaTime;
            if(tm<=-1.0f){
                cd.text="";
            }
        }
        if(ingame){
            pub.Timer-=Time.deltaTime;
        }
        if(pub.Timer<=0.0f){
            ingame=false;
        }
    }

    void nxtg(){
        r=Random.Range(0,sens.Length*256);
        for(int i=0;i<sens.Length;i++){
            sens[i].SetActive(false);
        }
        if(prevtg==r%sens.Length){
            sens[(r+1)%sens.Length].SetActive(true);
            prevtg=(r+1)%sens.Length;
        }else{
            sens[r%sens.Length].SetActive(true);
            prevtg=r%sens.Length;
        }
        navi.nvst(prevtg);
    }

    void OnCollisionEnter(Collision cl){
        if(cl.gameObject.tag=="en" && ingame){
            gamemode=-1;
            ingame=false;
            navi.nvoff();
            BGM.BGMmasStop();
            snd.PlayOneShot(SE[0]);
            ri=GetComponent<Rigidbody>();
            ri.constraints=RigidbodyConstraints.None;
            ri.drag=0.9f;
            ri.angularDrag=0.5f;
            ri.useGravity=true;
            ri.centerOfMass=new Vector3(Random.Range(-0.06f,0.06f),0.07f,Random.Range(-0.08f,0.08f));
            ri.velocity=new Vector3(0.0f,0.15f*MV.spd,0.35f*MV.spd);
            fan.SetTrigger("dd");
            //StartCoroutine(CQ.Shake(2.9f,MV.spd/66.6f));
        }
    }
    void OnTriggerEnter(Collider ps){
        if(ps.gameObject.tag=="jsens"){
            pub.Score++;
            snd.PlayOneShot(SE[2]);
            int ran=Random.Range(0,2);
            switch(ran){
                case 0: fk1.SetTrigger("get1"); break;
                case 1: fk2.SetTrigger("get2"); break;
            }
            nxtg();
        }
    }
}
