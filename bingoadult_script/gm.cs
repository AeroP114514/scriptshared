using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class gm : MonoBehaviour
{
    public GameObject ball,sk,bbk,ds1,ds2,ds3,ds4,coins,bgmcnt,wmobj;
    public GameObject[] ss;
    public int bingom=0,winc=0,winm=0,odds=0,credit=0,wager=1,coinsplay=0,maxbet,exmulti=1,spline;
    int[] w1={3,4,6,8},w2={5,6,10,15},w3={7,10,14,25},w4={10,15,20,40},
            w5={15,20,32,70},w6={20,30,50,100},w8={40,60,90,250};
    bool[] cntr=new bool[9];
    bool[] bmtr=new bool[8];
    public bool sht=false,del=false,isplay=false,mstp=false,firstbingo=false;
    public TextMeshProUGUI crd,tx,wpb,played;
    public TextMeshProUGUI[] pot;
    tmplr libero;
    public AudioClip SFX0,SFX1,SFX2,SFX3,SFX10,SFX11,SFX12,SFX13,SFX14,SFX15,SFX16;
    AudioSource snd;
    public linegen SpLineInd;
    public reelm SlotCont;
    bgmMngr chbgm;
    void Start()
    {
        snd=GetComponent<AudioSource>();
        chbgm=bgmcnt.GetComponent<bgmMngr>();
        libero=wmobj.GetComponent<tmplr>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(bingom){
            case 0:winm=0; break;
            case 1:winm=w1[odds]*wager*exmulti; break;
            case 2:winm=w2[odds]*wager*exmulti; break;
            case 3:winm=w3[odds]*wager*exmulti; break;
            case 4:winm=w4[odds]*wager*exmulti; break;
            case 5:winm=w5[odds]*wager*exmulti; break;
            case 6:winm=w6[odds]*wager*exmulti; break;
            case 8:winm=w8[odds]*wager*exmulti; break;
            default:winm=w6[odds]*wager*exmulti; break;
        }
        if((Input.GetKeyDown(KeyCode.C) && !sht && !del && isplay && !mstp && SlotCont.sm==0)||(bingom==8 && odds==3 && isplay && !sht)){
            isplay=false;
            del=true;
            chbgm.SS();
            StartCoroutine("cash");
        }

        if(Input.GetKeyDown(KeyCode.I)){
            coinin();
        }
        if(bingom>=1 && isplay){
            chbgm.GS();
            libero.lampon();
        }
        crd.text=credit.ToString();
        tx.text=winm.ToString();
        wpb.text=wager.ToString();
        played.text=coinsplay.ToString();
        pot[0].text=(w1[odds]*wager*exmulti).ToString();
        pot[1].text=(w2[odds]*wager*exmulti).ToString();
        pot[2].text=(w3[odds]*wager*exmulti).ToString();
        pot[3].text=(w4[odds]*wager*exmulti).ToString();
        pot[4].text=(w5[odds]*wager*exmulti).ToString();
        pot[5].text=(w6[odds]*wager*exmulti).ToString();
        pot[6].text=(w8[odds]*wager*exmulti).ToString();

        if(Input.GetKeyDown(KeyCode.Space) && !sht && !del && credit>=wager && !mstp && SlotCont.sm==0){
            if(isplay==false){
                isplay=true;
                spline=Random.Range(0,8);
                chbgm.PS();
                SpLineInd.gs(spline);
            }
            ds1.SetActive(true);
            ds2.SetActive(true);
            ds3.SetActive(true);
            ds4.SetActive(true);
            credit-=wager;
            coinsplay+=wager;
            sht=true;
            snd.PlayOneShot(SFX1);
            Instantiate(ball,new Vector3(Random.Range(-0.070f,0.070f),8.3f,0),Quaternion.identity);
        }
    }

    public void lose(){
        isplay=false;
        chbgm.SS();
        snd.PlayOneShot(SFX2);
        del=true;
        StartCoroutine("sweep");
    }

    public void inchk(int n){
        cntr[n-1]=true;
        if(cntr[0] && cntr[1] && cntr[2]){
            bmtr[0]=true;
            for(int j=0;j<3;j++){
                sens aa=ss[j].GetComponent<sens>();
                aa.bng=true;
            }
        }
        if(cntr[3] && cntr[4] && cntr[5]){
            bmtr[1]=true;
            for(int j=3;j<6;j++){
                sens aa=ss[j].GetComponent<sens>();
                aa.bng=true;
            }
        }
        if(cntr[6] && cntr[7] && cntr[8]){
            bmtr[2]=true;
            for(int j=6;j<9;j++){
                sens aa=ss[j].GetComponent<sens>();
                aa.bng=true;
            }
        }
        if(cntr[0] && cntr[3] && cntr[6]){
            bmtr[3]=true;
            for(int j=0;j<7;j+=3){
                sens aa=ss[j].GetComponent<sens>();
                aa.bng=true;
            }
        }
        if(cntr[1] && cntr[4] && cntr[7]){
            bmtr[4]=true;
            for(int j=1;j<8;j+=3){
                sens aa=ss[j].GetComponent<sens>();
                aa.bng=true;
            }
        }
        if(cntr[2] && cntr[5] && cntr[8]){
            bmtr[5]=true;
            for(int j=2;j<9;j+=3){
                sens aa=ss[j].GetComponent<sens>();
                aa.bng=true;
            }
        }
        if(cntr[0] && cntr[4] && cntr[8]){
            bmtr[6]=true;
            for(int j=0;j<9;j+=4){
                sens aa=ss[j].GetComponent<sens>();
                aa.bng=true;
            }
        }
        if(cntr[2] && cntr[4] && cntr[6]){
            bmtr[7]=true;
            for(int j=2;j<7;j+=2){
                sens aa=ss[j].GetComponent<sens>();
                aa.bng=true;
            }
        }
        bingom=0;
        for(int i=0;i<8;i++){
            if(bmtr[i]){
                bingom++;
            }
        }
        if(bingom>0 && !firstbingo){
            firstbingo=true;
            FBjudge();
        }
    }
    void FBjudge(){
        if(bmtr[spline]){
            exmulti=2;
            snd.PlayOneShot(SFX16);
        }else{
            SpLineInd.res();
        }
    }

    IEnumerator sweep(){
        yield return new WaitForSeconds(0.5f);
        bingom=0;
        exmulti=1;
        firstbingo=false;
        libero.lampoff();
        SpLineInd.res();
        sk.SetActive(false);
        for(int i=0;i<ss.Length;i++){
            sens zz=ss[i].GetComponent<sens>();
            zz.res();
            cntr[i]=false;
        }
        for(int f=0;f<bmtr.Length;f++){
            bmtr[f]=false;
        }
        odds=0;
        yield return new WaitForSeconds(1.5f);
        coinsplay=0;
        del=false;
        sk.SetActive(true);
    }
    IEnumerator cash(){
        libero.lampoff();
        if(winm>0){
            if(winm>=80*wager){
                snd.PlayOneShot(SFX12);
                yield return new WaitForSeconds(12.4f);
            }else if(winm>=20*wager){
                snd.PlayOneShot(SFX11);
                yield return new WaitForSeconds(3.3f);
            }else{
                snd.PlayOneShot(SFX10);
                yield return new WaitForSeconds(1.5f);
            }
            /*if(winm>=1000){
                for(int l=0;l<winm;l++){
                    Instantiate(coins,new Vector3(Random.Range(8.8f,12.6f),8.5f,0),Quaternion.identity);
                    snd.PlayOneShot(SFX13);
                    yield return new WaitForSeconds(0.03f);
                }
            }else if(winm>=200){
                for(int l=0;l<winm;l++){
                    Instantiate(coins,new Vector3(Random.Range(8.8f,12.6f),8.5f,0),Quaternion.identity);
                    snd.PlayOneShot(SFX13);
                    yield return new WaitForSeconds(0.07f);
                }
            }else if(winm>=50){
                for(int l=0;l<winm;l++){
                    Instantiate(coins,new Vector3(Random.Range(8.8f,12.6f),8.5f,0),Quaternion.identity);
                    snd.PlayOneShot(SFX13);
                    yield return new WaitForSeconds(0.1f);
                }
            }else{
                for(int l=0;l<winm;l++){
                    Instantiate(coins,new Vector3(Random.Range(8.8f,12.6f),8.5f,0),Quaternion.identity);
                    snd.PlayOneShot(SFX13);
                    yield return new WaitForSeconds(0.16f);
                }
            }*/
            for(int l=0;l<winm;l++){
                if(l>1000 && winm-l>=57){
                    credit+=57;
                    l+=56;
                }else if(l>500 && winm-l>=31){
                    credit+=31;
                    l+=30;
                }else if(l>300 && winm-l>=19){
                    credit+=19;
                    l+=18;
                }else if(l>=150 && winm-l>=12){
                    credit+=12;
                    l+=11;
                }else if(l>=50 && winm-l>=7){
                    credit+=7;
                    l+=6;
                }else if(l>=30 && winm-l>=3){
                    credit+=3;
                    l+=2;
                }else{
                    credit++;
                }
                snd.PlayOneShot(SFX13);
                yield return new WaitForSeconds(0.09f);
            }
        }else{
            snd.PlayOneShot(SFX3);
        }
        yield return new WaitForSeconds(0.6f);
        yield return StartCoroutine("sweep");
    }

    public void chance(){
        StartCoroutine("lc");
    }
    IEnumerator lc(){
        sk.SetActive(false);
        bbk.SetActive(true);
        mstp=true;
        for(int i=0;i<ss.Length;i++){
            sens zz=ss[i].GetComponent<sens>();
            zz.lucky();
        }
        yield return new WaitForSeconds(0.4f);
        sk.SetActive(true);
        bbk.SetActive(false);
        yield return new WaitForSeconds(0.3f);
        mstp=false;
    }

    public void BUP(){
        if(wager<maxbet){
            wager++;
            snd.PlayOneShot(SFX14);
        }
    }
    public void BDN(){
        if(wager>1){
            wager--;
            snd.PlayOneShot(SFX15);
        }
    }
    public void coinin(){
        credit++;
        snd.PlayOneShot(SFX0);
    }
}
