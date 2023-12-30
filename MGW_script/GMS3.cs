using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GMS3 : MonoBehaviour
{   //FrenzyWheelBonusBox
    public int Lines,Coins,bet,win,Mbet,GameMode,FGrem,FGwin,BNSPay,TPL;
    int lnc=-1;
    float timer=0.0f;
    bool inFG=false,skip2;
    int[] symbol=new int[3];
    int[] payt=new int[8];
    int[] setLine={1,3,5,9,15};
    int[,] refpos=new int[15,3]{
        {1,1,1},{2,2,2},{0,0,0},{2,1,0},{0,1,2},{1,2,1},{1,0,1},{2,1,2},{0,1,0},{1,2,2},{1,0,0},{2,2,1},{0,0,1},{2,0,2},{0,2,0}
    };
    public TextMeshProUGUI cmtr,bmtr,wmtr,pmtr,state,guidance,pt1,pt2,LI,CI,BNSP;
    public GameObject rtnbutton,tblbutton;
    public bgmMngr BGMct;
    public AudioClip[] SFX;
    AudioSource snd;
    public spinsound ssd;
    public scenechanger SC;
    public tableind tblind;
    public Lind atom;
    public Reelctrl1_3 RC1;
    public Reelctrl2_3 RC2;
    public Reelctrl3_3 RC3;
    public movecam Cam;
    public wheelspin wheelobj;

    void Start()
    {
        snd=GetComponent<AudioSource>();
        Mbet=10;
        win=0;
        GameMode=0;
        bet=0;
        Lines=0;
        Coins=0;
        FGrem=0;
        FGwin=0;
        BNSPay=0;
        wmtr.text="";
        pmtr.text="";
        BNSP.text="";
    }

    // Update is called once per frame
    void Update()
    {
        cmtr.text="CREDIT\n"+stn.credit.ToString();
        bmtr.text="WAGER\n"+bet.ToString();
        LI.text=Lines.ToString();
        CI.text=Coins.ToString();
        if(GameMode==0){
            rtnbutton.SetActive(true);
            tblbutton.SetActive(true);
            if(stn.credit==0){
                state.text="Game Over. Insert Coins!";
                guidance.text="[I/O] Insert 1 Coin/100 Coins";
            }else{
                if(bet!=0){
                    state.text="Game Over. Insert Coin or Bet!";
                    guidance.text="[I/O] Insert 1 Coin/100 Coins\n[B] Bet 1 Credit Per Line\n[N] Repeat Bet and Spin (Bet Same Amount of Credit for Previous Game.)";
                }else if(Lines==0){
                    state.text="Game Over. Insert Coin or Select Play Lines!";
                    guidance.text="[I/O] Insert 1 Coin/100 Coins\n[V] Select Play Lines";
                }else{
                    state.text="Selected PayLine = "+Lines.ToString();
                    guidance.text="[I/O] Insert 1 Coin/100 Coins\n[V] Select Play Lines\n[B] Bet 1 Credit Per Line";
                }
            }
        }else{
            rtnbutton.SetActive(false);
            tblbutton.SetActive(false);
        }
        TPL=Coins*800;
        payt[7]=Coins*100;
        payt[6]=Coins*75;
        payt[5]=Coins*50;
        payt[4]=Coins*30;
        payt[3]=Coins*20;
        payt[2]=Coins*10;
        payt[1]=Coins*5;
        payt[0]=0;
        if(bet!=0){
            pt1.text=payt[6].ToString()+"\n"+payt[5].ToString()+"\n"+payt[4].ToString()+"\n"+
            payt[3].ToString()+"\n"+payt[2].ToString()+"\n"+payt[1].ToString();
            pt2.text=TPL.ToString();
        }else{
            pt1.text="x80\nx50\nx40\nx25\nx10\nx5";
            pt2.text="x1000";
        }
        
        if(Input.GetKeyDown(KeyCode.I) && GameMode<=1 && stn.credit<10000){
            stn.credit++;
            snd.PlayOneShot(SFX[0]);
        }
        if(Input.GetKeyDown(KeyCode.O) && GameMode<=1 && stn.credit<10000){
            stn.credit+=100;
            snd.PlayOneShot(SFX[0]);
        }
        if(Input.anyKey && !Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !Input.GetMouseButton(2)){
            tblind.tOFF();
        }
        if(Input.GetKeyDown(KeyCode.V) && GameMode==0){
            Coins=0;
            if(lnc>=4){
                lnc=0;
            }else{
                lnc++;
            }
            Lines=setLine[lnc];
            bet=Coins*Lines;
            atom.L1(Lines);
            snd.PlayOneShot(SFX[16+lnc]);
        }
        if(Input.GetKeyDown(KeyCode.B) && stn.credit>=Lines && GameMode<=1 && Lines!=0){
            if(GameMode==0){
                GameMode=1;
                Coins=0;
                state.text="Play Up To 10 Credits Per Line. Bet or Start.";
                guidance.text="[I/O] Insert 1 Coin/100 Coins\n[B] Bet 1 Credit Per Line\n[Space] Spin Reels";
            }
            if(Coins<Mbet){
                stn.credit-=Lines;
                Coins++;
                bet=Coins*Lines;
                snd.PlayOneShot(SFX[1]);
                if(Coins==Mbet){
                    state.text="Good Luck!";
                    guidance.text="[I/O] Insert 1 Coin/100 Coins\n[Space] Spin Reels";
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
        if(Input.GetKeyDown(KeyCode.Escape) && GameMode==0){
            SC.SC0();
        }
        if(Input.GetKeyDown(KeyCode.Space) && GameMode==40){
            skip2=true;
        }
        if(GameMode==254){
            state.text="Press Start Button To Spin The Wheel!";
            timer+=Time.deltaTime;
            if(Input.GetKeyDown(KeyCode.Space) || timer>=30.0f){
                timer=0.0f;
                BGMct.StopBGM();
                state.text="Good Luck!";
                GameMode=255;
                wheelobj.flag=1;
                BGMct.Plms(2);
            }
        }
        if(GameMode==257){
            timer+=Time.deltaTime;
            if(Input.GetKeyDown(KeyCode.Space) || timer>=30.0f){
                GameMode=256;
                StartCoroutine("freegame");
                state.text="Free Spin Remaining = "+FGrem.ToString()+"...Good Luck!";
            }
        }
    }

    public void getstop(){
        StartCoroutine("judge");
    }
    public void WhDec(int n){
        BNSPay+=n*Coins;
        Cam.f=2;
        BNSP.text="Present Bonus Value = "+BNSPay.ToString();
        StartCoroutine("Fstart");
    }
    void REPEATSTART(){
        stn.credit-=bet;
            snd.PlayOneShot(SFX[1]);
            GameMode=2;
            wmtr.text="";
            pmtr.text="";
            StartCoroutine("gamestart");
    }
    void Ftest(){
        if(FGrem>0){
            StartCoroutine("freegame");
        }else{
            StartCoroutine("fin");
        }
    }

    IEnumerator gamestart(){
        win=0;
        atom.anypay=0;
        lnc=-1;
        atom.Loff();
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
        yield return new WaitForSeconds(0.85f);
        RC1.spinF=2;
    }
    IEnumerator freegame(){
        win=0;
        atom.anypay=0;
        lnc=-1;
        atom.Loff();
        wmtr.text="F.WIN\n"+FGwin.ToString();
        state.text="Free Spin Remaining = "+FGrem.ToString()+"...Good Luck!";
        guidance.text="";
        for(int i=0;i<3;i++){
            symbol[i]=0;
        }
        yield return new WaitForSeconds(0.05f);
        snd.PlayOneShot(SFX[2]);
        RC1.spinF=1;
        RC2.spinF=1;
        RC3.spinF=1;
        yield return new WaitForSeconds(0.85f);
        RC1.spinF=2;
    }

    IEnumerator Fstart(){
        FGrem+=10;
        inFG=false;
        yield return new WaitForSeconds(1.0f);
        BGMct.Plms(3);
        state.text="Press Start Button To Start!";
        GameMode=257;
    }

    IEnumerator judge(){
        state.text="";
        ssd.bgmstop();
        yield return new WaitForSeconds(0.4f);
        int[] LPay=new int[Lines];
        for(int i=0;i<Lines;i++){
            atom.won[i]=0;
            int clc=0;
            symbol[0]=RC1.mark[refpos[i,0]];
            symbol[1]=RC2.mark[refpos[i,1]];
            symbol[2]=RC3.mark[refpos[i,2]];
            int[] fl={0,0,0};
            for(int k=0;k<3;k++){
                if(symbol[k]==1 || symbol[k]==2 || symbol[k]==3){
                    fl[k]=1;
                }
            }
            if(symbol[0]==999 && symbol[1]==999 && symbol[2]==999){
                clc=payt[7];
            }else if((symbol[0]==10 || symbol[0]==999)&&(symbol[1]==10 || symbol[1]==999)&&(symbol[2]==10 || symbol[2]==999)){
                clc=payt[6];
            }else if(symbol[0]==50 && symbol[1]==50 && symbol[2]==50){
                clc=payt[5];
            }else if((symbol[0]==3 || symbol[0]==999)&&(symbol[1]==3 || symbol[1]==999)&&(symbol[2]==3 || symbol[2]==999)){
                clc=payt[4];
            }else if((symbol[0]==2 || symbol[0]==999)&&(symbol[1]==2 || symbol[1]==999)&&(symbol[2]==2 || symbol[2]==999)){
                clc=payt[3];
            }else if((symbol[0]==1 || symbol[0]==999)&&(symbol[1]==1 || symbol[1]==999)&&(symbol[2]==1 || symbol[2]==999)){
                clc=payt[2];
            }else if((fl[0]==1 || symbol[0]==999)&&(fl[1]==1 || symbol[1]==999)&&(fl[2]==1 || symbol[2]==999)){
                clc=payt[1];
            }else if(symbol[0]==88 && symbol[1]==88 && symbol[2]==88){
                inFG=true;
                atom.anypay=bet*1;
                win+=bet*1;
            }
            for(int n=0;n<3;n++){
                if(symbol[n]==999){
                    clc*=2;
                }
            }
            LPay[i]=clc;
            atom.won[i]=clc;
            win+=clc;
        }
        for(int i=0;i<3;i++){
            if(RC1.mark[i]==50){
                win+=BNSPay;
                atom.anypay+=BNSPay;
            }
            if(RC2.mark[i]==50){
                win+=BNSPay;
                atom.anypay+=BNSPay;
            }
            if(RC3.mark[i]==50){
                win+=BNSPay;
                atom.anypay+=BNSPay;
            }
        }
        if(inFG){
            state.text="Free Spin Bonus!!";
            wmtr.text="WIN\n"+win.ToString();
            FGwin+=win;
            win=0;
            snd.PlayOneShot(SFX[14]);
            yield return new WaitForSeconds(3);
            Cam.f=1;
            yield return new WaitForSeconds(1);
            BGMct.Plms(1);
            timer=0.0f;
            GameMode=254;
        }
        if(win>0){
            if(FGrem>0){
                StartCoroutine("FCnt");
            }else{
                StartCoroutine("Counting");
            }
        }else if((FGrem>0 || inFG)&& GameMode!=254){
            FGrem--;
            inFG=false;
            yield return new WaitForSeconds(1);
            GameMode=256;
            Ftest();
        }else if(GameMode==254){

        }else{
            GameMode=0;
        }
    }

    IEnumerator fin(){
        BNSP.text="";
        win=FGwin;
        FGwin=0;
        BNSPay=0;
        snd.PlayOneShot(SFX[15]);
        state.text="Bonus Game is Over.";
        BGMct.StopBGM();
        yield return new WaitForSeconds(4.5f);
        StartCoroutine("Counting");
    }
    IEnumerator Counting(){
        state.text="Winner!!";
        skip2=false;
        atom.L5();
        GameMode=40;
        if(win/bet>=25){
            if(win/bet>=300){
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
            yield return new WaitForSeconds(6.0f);
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
    }
    IEnumerator FCnt(){
        state.text="Winner!!";
        skip2=false;
        atom.L5();
        GameMode=40;
        snd.clip=SFX[21];
        SFX[3]=SFX[22];
        snd.Play();
        for(int pd=1;pd<=win;pd++){
            if(skip2){
                wmtr.text="F.WIN\n"+FGwin.ToString();
                FGwin+=(win-pd+1);
                break;
            }
            if(pd>=300 && win-pd>=7){
                yield return new WaitForSeconds(0.01f);
                FGwin+=7;
                pd+=6;
            }else if(pd>=200){
                yield return new WaitForSeconds(0.015f);
                FGwin++;
            }else if(pd>=150){
                yield return new WaitForSeconds(0.02f);
                FGwin++;
            }else if(pd>=100){
                yield return new WaitForSeconds(0.03f);
                FGwin++;
            }else if(pd>=50){
                yield return new WaitForSeconds(0.04f);
                FGwin++;
            }else if(pd>=20){
                yield return new WaitForSeconds(0.055f);
                FGwin++;
            }else{
                yield return new WaitForSeconds(0.07f);
                FGwin++;
            }
            wmtr.text="F.WIN\n"+FGwin.ToString();
        }
        wmtr.text="F.WIN\n"+FGwin.ToString();
        snd.Stop();
        snd.PlayOneShot(SFX[3]);
        FGrem--;
        inFG=false;
        yield return new WaitForSeconds(1);
        GameMode=256;
        Ftest();
    }
}
