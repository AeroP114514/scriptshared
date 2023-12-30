using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GMS1 : MonoBehaviour
{   //Slot
    public int bet,win,Mbet,GameMode,FGrem,FGwin;
    bool inFG=false,skip2;
    int[] symbol=new int[3];
    int[] payt=new int[11];
    public GameObject[] unitUI;
    public TextMeshProUGUI cmtr,bmtr,wmtr,pmtr,state,guidance;
    public GameObject rtnbutton;
    public bgmMngr BGMct;
    public AudioClip[] SFX;
    AudioSource snd;
    public spinsound ssd;
    public scenechanger SC;
    public Reelctrl1 RC1;
    public Reelctrl2 RC2;
    public Reelctrl3 RC3;

    void Start()
    {
        snd=GetComponent<AudioSource>();
        Mbet=80;
        win=0;
        GameMode=0;
        bet=0;
        FGrem=0;
        FGwin=0;
        wmtr.text="";
        pmtr.text="";
    }

    // Update is called once per frame
    void Update()
    {
        cmtr.text="CREDIT\n"+stn.credit.ToString();
        bmtr.text="WAGER\n"+bet.ToString();
        if(GameMode==0){
            rtnbutton.SetActive(true);
            if(stn.credit==0){
                state.text="Game Over. Insert Coins!";
                guidance.text="[I/O] Insert 1 Coin/100 Coins";
            }else{
                state.text="Game Over. Insert Coin or Bet!";
                if(bet!=0){
                    guidance.text="[I/O] Insert 1 Coin/100 Coins\n[B] Bet 1 Credit\n[N] Repeat Bet and Spin (Bet Same Amount of Credit for Previous Game.)";
                }else{
                    guidance.text="[I/O] Insert 1 Coin/100 Coins\n[B] Bet 1 Credit";
                }
                
            }
        }else{
            rtnbutton.SetActive(false);
        }
        payt[10]=bet*1000;
        payt[9]=bet*500;
        payt[8]=bet*300;
        payt[7]=bet*200;
        payt[6]=bet*30;
        payt[5]=bet*50;
        payt[4]=bet*20;
        payt[3]=bet*15;
        payt[2]=bet*10;
        payt[1]=bet*2;
        payt[0]=0;

        if(Input.GetKeyDown(KeyCode.I) && GameMode<=1 && stn.credit<10000){
            stn.credit++;
            snd.PlayOneShot(SFX[0]);
        }
        if(Input.GetKeyDown(KeyCode.O) && GameMode<=1 && stn.credit<10000){
            stn.credit+=100;
            snd.PlayOneShot(SFX[0]);
        }
        if(Input.GetKeyDown(KeyCode.B) && stn.credit>0 && GameMode<=1){
            if(GameMode==0){
                GameMode=1;
                bet=0;
                state.text="Play 1 to "+Mbet.ToString()+" Credits. Bet or Start.";
                guidance.text="[I/O] Insert 1 Coin/100 Coins\n[B] Bet 1 Credit\n[Space] Spin Reel";
            }
            if(bet<Mbet){
                stn.credit--;
                bet++;
                snd.PlayOneShot(SFX[1]);
                if(bet==Mbet){
                    state.text="Good Luck!";
                    guidance.text="[I/O] Insert 1 Coin/100 Coins\n[Space] Spin Reel";
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.Space) && bet>0 && GameMode==1){
            GameMode=2;
            wmtr.text="";
            pmtr.text="";
            StartCoroutine("gamestart");
        }
        if(Input.GetKeyDown(KeyCode.N) && stn.credit>=bet && bet!=0 && GameMode==0){
            REPEATSTART();
        }
        if(Input.GetKeyDown(KeyCode.M) && stn.credit>=Mbet && GameMode==0){
            bet=0;
            GameMode=1;
            stn.credit-=Mbet;
            bet+=Mbet;
            snd.PlayOneShot(SFX[1]);
            state.text="Good Luck!";
            guidance.text="[I/O] Insert 1 Coin/100 Coins\n[Space] Spin Reel";
        }
        if(Input.GetKeyDown(KeyCode.Escape) && GameMode==0){
            SC.SC0();
        }
        if(Input.GetKeyDown(KeyCode.Space) && GameMode==40){
            skip2=true;
        }
    }
    public void getstop(){
        StartCoroutine("judge");
    }
    void REPEATSTART(){
        stn.credit-=bet;
            snd.PlayOneShot(SFX[1]);
            GameMode=2;
            wmtr.text="";
            pmtr.text="";
            StartCoroutine("gamestart");
    }

    IEnumerator gamestart(){
        win=0;
        state.text="Good Luck!";
        guidance.text="";
        for(int i=0;i<3;i++){
            symbol[i]=0;
        }
        yield return new WaitForSeconds(0.05f);
        snd.PlayOneShot(SFX[2]);
        RC1.spinF=1;
        RC2.spinF=1;
        RC3.spinF=1;
        ssd.bgmstart();
        yield return new WaitForSeconds(0.6f);
        RC1.spinF=2;
    }
    IEnumerator judge(){
        state.text="";
        ssd.bgmstop();
        yield return new WaitForSeconds(0.4f);
        symbol[0]=RC1.mark;
        symbol[1]=RC2.mark;
        symbol[2]=RC3.mark;
        int clr1=0,clr2=0,clr3=0;
        for(int i=0;i<3;i++){
            int k=0;
            if(symbol[i]==1 || symbol[i]==10){
                k=1;
            }else if(symbol[i]==2 || symbol[i]==20){
                k=2;
            }else if(symbol[i]==3 || symbol[i]==30){
                k=3;
            }
            switch(i){
                case 0: clr1=k; break;
                case 1: clr2=k; break;
                case 2: clr3=k; break;
            }
        }
        if(symbol[0]==10 && symbol[1]==20 && symbol[2]==30){
            win=payt[10];
        }else if(symbol[0]==10 && symbol[1]==10 && symbol[2]==10){
            win=payt[9];
        }else if(symbol[0]==20 && symbol[1]==20 && symbol[2]==20){
            win=payt[8];
        }else if(symbol[0]==30 && symbol[1]==30 && symbol[2]==30){
            win=payt[7];
        }else if(symbol[0]>=10 && symbol[1]>=10 && symbol[2]>=10){
            win=payt[6];
        }else if(clr1==1 && clr2==2 && clr3==3){
            win=payt[5];
        }else if(clr1==1 && clr2==1 && clr3==1){
            win=payt[4];
        }else if(clr1==2 && clr2==2 && clr3==2){
            win=payt[3];
        }else if(clr1==3 && clr2==3 && clr3==3){
            win=payt[2];
        }else if(symbol[0]!=0 && symbol[1]!=0 && symbol[2]!=0){
            win=payt[1];
        }
        if(win>0){
            state.text="Winner!!";
            skip2=false;
            GameMode=40;
            if(win/bet>=25){
                if(win/bet>=400){
                    snd.clip=SFX[12];
                    SFX[3]=SFX[13];
                }else if(win/bet>=100){
                    snd.clip=SFX[10];
                    SFX[3]=SFX[11];
                }else{
                    snd.clip=SFX[8];
                    SFX[3]=SFX[4];
                }
                snd.PlayOneShot(SFX[7]);
                yield return new WaitForSeconds(3.1f);
            }else{
                snd.clip=SFX[9];
                SFX[3]=SFX[5];
            }
            snd.Play();
            for(int pd=1;pd<=win;pd++){
                if(skip2){
                    wmtr.text="WIN\n"+win.ToString();
                    stn.credit+=(win-pd+1);
                    break;
                }
                if(pd>=1000 && win-pd>=7){
                    yield return new WaitForSeconds(0.01f);
                    stn.credit+=7;
                    pd+=6;
                }else if(pd>=500){
                    yield return new WaitForSeconds(0.015f);
                    stn.credit++;
                }else if(pd>=300){
                    yield return new WaitForSeconds(0.02f);
                    stn.credit++;
                }else if(pd>=200){
                    yield return new WaitForSeconds(0.03f);
                    stn.credit++;
                }else if(pd>=100){
                    yield return new WaitForSeconds(0.04f);
                    stn.credit++;
                }else if(pd>=50){
                    yield return new WaitForSeconds(0.055f);
                    stn.credit++;
                }else{
                    yield return new WaitForSeconds(0.07f);
                    stn.credit++;
                }
                wmtr.text="WIN\n"+pd.ToString();
            }
            pmtr.text="PAID\n"+win.ToString();
            snd.Stop();
            snd.PlayOneShot(SFX[3]);
            GameMode=0;
        }else{
            GameMode=0;
        }
    }
}
