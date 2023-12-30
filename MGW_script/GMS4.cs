using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GMS4 : MonoBehaviour
{   //FrenzyWheelMiracleMika
    public int Lines,Coins,bet,win,Mbet,GameMode,FGrem,FGwin,FLv,TPL;
    int lnc=-1,MLTP=1,EXP=0,ftotalwon=0,fplayed=0;
    public float GMSPDdef,gamespd;
    float timer=0.0f;
    public bool nowFG=false;
    bool inFG=false,skip2,autoplay=false,BnsCrs=false;
    int[] symbol=new int[5];
    int[,] payt=new int[10,4];
    int[] setLine={1,5,9,15,25};
    int[,] refpos=new int[25,5]{
        {1,1,1,1,1},{2,2,2,2,2},{0,0,0,0,0},{2,1,0,1,2},{0,1,2,1,0},
        {1,2,2,2,1},{1,0,0,0,1},{2,2,1,2,2},{0,0,1,0,0},{1,2,1,0,1},
        {1,0,1,2,1},{2,1,2,1,2},{0,1,0,1,0},{2,0,2,0,2},{0,2,0,2,0},
        {1,1,2,1,1},{1,1,0,1,1},{2,1,1,1,0},{0,1,1,1,2},{2,0,1,2,0},
        {0,2,1,0,2},{2,0,1,0,2},{0,2,1,2,0},{1,2,0,2,1},{1,0,2,0,1}
    };
    int[] tgt={0,35,80,150,300,9999999};
    string[] nxlv={"0","35","80","150","300","-"};
    public TextMeshProUGUI cmtr,bmtr,wmtr,pmtr,state,guidance,pt1,pt2,pt3,pt4,pt6,LI,CI,BNSP;
    public GameObject rtnbutton,tblbutton,indct;
    public bgmMngr BGMct;
    public AudioClip[] SFX;
    AudioSource snd;
    public spinsound ssd;
    public scenechanger SC;
    public tableind tblind;
    public Lind4 atom;
    public Reelctrl1_4 RC1;
    public Reelctrl2_4 RC2;
    public Reelctrl3_4 RC3;
    public Reelctrl4_4 RC4;
    public Reelctrl5_4 RC5;
    public movecam4 Cam;
    public wheelspin4 wheelobj;
    public FreeResult fres;

    void Start()
    {
        snd=GetComponent<AudioSource>();
        Mbet=20;
        win=0;
        GameMode=0;
        bet=0;
        Lines=0;
        Coins=0;
        FGrem=0;
        FGwin=0;
        FLv=0;
        gamespd=GMSPDdef;
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
        TPL=Coins*1000;
        payt[9,0]=Coins*5; payt[9,1]=Coins*25; payt[9,2]=Coins*125; payt[9,3]=Coins*1000;
        payt[8,0]=Coins*3; payt[8,1]=Coins*20; payt[8,2]=Coins*100; payt[8,3]=Coins*500;
        payt[7,0]=Coins*2; payt[7,1]=Coins*15; payt[7,2]=Coins*75; payt[7,3]=Coins*300;
        payt[6,0]=0; payt[6,1]=Coins*10; payt[6,2]=Coins*20; payt[6,3]=Coins*100;
        payt[5,0]=0; payt[5,1]=Coins*7; payt[5,2]=Coins*15; payt[5,3]=Coins*100;
        payt[4,0]=0; payt[4,1]=Coins*7; payt[4,2]=Coins*15; payt[4,3]=Coins*75;
        payt[3,0]=0; payt[3,1]=Coins*5; payt[3,2]=Coins*10; payt[3,3]=Coins*50;
        payt[2,0]=0; payt[2,1]=Coins*5; payt[2,2]=Coins*10; payt[2,3]=Coins*30;
        payt[1,0]=0; payt[1,1]=Coins*4; payt[1,2]=Coins*10; payt[1,3]=Coins*20;
        payt[0,0]=0;
        if(bet!=0){
            pt1.text=payt[9,0].ToString()+"\n"+payt[8,0].ToString()+"\n"+payt[7,0].ToString()+"\n-\n-\n-\n-\n-\n-";
            pt2.text=payt[9,1].ToString()+"\n"+payt[8,1].ToString()+"\n"+payt[7,1].ToString()+"\n"+payt[6,1].ToString()+"\n"+payt[5,1].ToString()+"\n"+
            payt[4,1].ToString()+"\n"+payt[3,1].ToString()+"\n"+payt[2,1].ToString()+"\n"+payt[1,1].ToString();
            pt3.text=payt[9,2].ToString()+"\n"+payt[8,2].ToString()+"\n"+payt[7,2].ToString()+"\n"+payt[6,2].ToString()+"\n"+payt[5,2].ToString()+"\n"+
            payt[4,2].ToString()+"\n"+payt[3,2].ToString()+"\n"+payt[2,2].ToString()+"\n"+payt[1,2].ToString();
            pt4.text="\n"+payt[8,3].ToString()+"\n"+payt[7,3].ToString()+"\n"+payt[6,3].ToString()+"\n"+payt[5,3].ToString()+"\n"+
            payt[4,3].ToString()+"\n"+payt[3,3].ToString()+"\n"+payt[2,3].ToString()+"\n"+payt[1,3].ToString();
            pt6.text=TPL.ToString();
        }else{
            pt1.text="x5\nx3\nx2\n-\n-\n-\n-\n-\n-";
            pt2.text="x25\nx20\nx15\nx10\nx7\nx7\nx5\nx5\nx4";
            pt3.text="x125\nx100\nx75\nx20\nx15\nx15\nx10\nx10\nx10";
            pt4.text="\nx500\nx300\nx100\nx100\nx75\nx50\nx30\nx20";
            pt6.text="x1000";
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
                state.text="Play Up To "+Mbet+" Credits Per Line. Bet or Start.";
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
        if((Input.GetKeyDown(KeyCode.N) || autoplay) && stn.credit>=bet && bet!=0 && GameMode==0){
            REPEATSTART();
        }
        if(Input.GetKeyDown(KeyCode.Escape) && GameMode==0){
            SC.SC0();
        }
        if(Input.GetKeyDown(KeyCode.A)){
            if(autoplay){
                autoplay=false;
            }else{
                autoplay=true;
            }
        }
        if(Input.GetKeyDown(KeyCode.Space)){
            if(GameMode==3){
                gamespd=7.2f;
            }else if(GameMode==40){
                skip2=true;
            }
        }
        if(Input.GetKeyDown(KeyCode.Q) && GameMode==0 && stn.credit>=bet*100 && bet!=0){ //BonusCruster
            stn.credit-=bet*100;
            GameMode=2;
            win=0;
            atom.anypay=0;
            lnc=-1;
            atom.Loff();
            state.text="Good Luck!";
            guidance.text="";
            wmtr.text="";
            pmtr.text="";
            for(int i=0;i<5;i++){
                symbol[i]=0;
            }
            if(nowFG){
                BNSP.text="";
                nowFG=false;
                RC1.changesymbol(0);
                RC2.changesymbol(0);
                RC3.changesymbol(0);
                RC4.changesymbol(0);
                RC5.changesymbol(0);
            }
            BnsCrs=true;
            StartCoroutine("judge");
        }
        if(GameMode==254){
            state.text="Press Start Button To Spin The Wheel!";
            timer+=Time.deltaTime;
            if(Input.GetKeyDown(KeyCode.Space) || timer>=30.0f){
                state.text="Good Luck!";
                BGMct.StopBGM();
                timer=0.0f;
                GameMode=255;
                StartCoroutine("startsound");
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

        if(nowFG && FGrem>0){
            BNSP.text="EXP = "+EXP.ToString()+"/"+nxlv[FLv]+"   Free Lv."+FLv.ToString();
        }
    }

    public void getstop(){
        StartCoroutine("judge");
    }
    public void WhDec(int n,int v){
        FGrem+=v;
        FLv=n;
        Cam.f=2;
        BNSP.text="Free Lv."+FLv.ToString();
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

    IEnumerator startsound(){
        snd.PlayOneShot(SFX[26]);
        yield return new WaitForSeconds(0.47f);
        wheelobj.flag=1;
        BGMct.Plms(2);
    }

    IEnumerator gamestart(){
        win=0;
        atom.anypay=0;
        lnc=-1;
        atom.Loff();
        gamespd=GMSPDdef;
        state.text="Good Luck!";
        guidance.text="";
        for(int i=0;i<5;i++){
            symbol[i]=0;
        }
        if(nowFG){
            BNSP.text="";
            nowFG=false;
            RC1.changesymbol(0);
            RC2.changesymbol(0);
            RC3.changesymbol(0);
            RC4.changesymbol(0);
            RC5.changesymbol(0);
        }
        yield return new WaitForSeconds(0.05f);
        snd.PlayOneShot(SFX[2]);
        RC1.spinF=1;
        RC2.spinF=1;
        RC3.spinF=1;
        RC4.spinF=1;
        RC5.spinF=1;
        ssd.bgmstart();
        yield return new WaitForSeconds(0.7f);
        GameMode=3;
        RC1.spinF=2;
    }
    IEnumerator freegame(){
        win=0;
        atom.anypay=0;
        lnc=-1;
        fplayed++;
        atom.Loff();
        gamespd=GMSPDdef;
        state.text="Free Spin Remaining = "+FGrem.ToString()+"...Good Luck!";
        guidance.text="";
        for(int i=0;i<5;i++){
            symbol[i]=0;
        }
        if(!nowFG){
            nowFG=true;
            BGMct.Plms(FLv+2);
            snd.PlayOneShot(SFX[23]);
            RC1.changesymbol(FLv);
            RC2.changesymbol(FLv);
            RC3.changesymbol(FLv);
            RC4.changesymbol(FLv);
            RC5.changesymbol(FLv);
            yield return new WaitForSeconds(1);
        }
        wmtr.text="F.WIN\n"+FGwin.ToString();
        yield return new WaitForSeconds(0.05f);
        snd.PlayOneShot(SFX[2]);
        RC1.spinF=1;
        RC2.spinF=1;
        RC3.spinF=1;
        RC4.spinF=1;
        RC5.spinF=1;
        yield return new WaitForSeconds(0.7f);
        GameMode=3;
        RC1.spinF=2;
    }

    IEnumerator Fstart(){
        inFG=false;
        yield return new WaitForSeconds(1.0f);
        EXP=0;
        BGMct.Plms(FLv+2);
        state.text="Press Start Button To Start!";
        GameMode=257;
    }

    IEnumerator judge(){
        state.text="";
        ssd.bgmstop();
        yield return new WaitForSeconds(0.2f);
        int[] LPay=new int[Lines];
        for(int i=0;i<Lines;i++){
            atom.won[i]=0;
            int clc=0,Ntm=0,ofakind=0,jochain=0;
            symbol[0]=RC1.mark[refpos[i,0]];
            symbol[1]=RC2.mark[refpos[i,1]];
            symbol[2]=RC3.mark[refpos[i,2]];
            symbol[3]=RC4.mark[refpos[i,3]];
            symbol[4]=RC5.mark[refpos[i,4]];
            for(int k=0;k<5;k++){
                if(symbol[k]==88){
                    break;
                }else if(symbol[k]==999 || symbol[k]==Ntm){
                    ofakind++;
                }else if(Ntm==0){
                    Ntm=symbol[k];
                    ofakind++;
                }else{
                    break;
                }
            }
            for(int k=0;k<5;k++){
                if(symbol[k]==999){
                    jochain++;
                }else{
                    break;
                }
            }
            if(Ntm==0){
                Ntm=9;
            }
            if(ofakind>=2){
                if(jochain>=2){
                    if(payt[9,jochain-2]>payt[Ntm,ofakind-2]){
                        clc=payt[9,jochain-2];
                    }else{
                        clc=payt[Ntm,ofakind-2];
                    }
                }else{
                    clc=payt[Ntm,ofakind-2];
                }
            }
            clc*=MLTP;
            LPay[i]=clc;
            atom.won[i]=clc;
            win+=clc;
        }
        int sctcnt=0;
        for(int i=0;i<3;i++){
            if(RC1.mark[i]==88){
                sctcnt++;
            }
            if(RC2.mark[i]==88){
                sctcnt++;
            }
            if(RC3.mark[i]==88){
                sctcnt++;
            }
            if(RC4.mark[i]==88){
                sctcnt++;
            }
            if(RC5.mark[i]==88){
                sctcnt++;
            }
            if(RC1.mark[i]==4){
                EXP++;
            }
            if(RC2.mark[i]==4){
                EXP++;
            }
            if(RC3.mark[i]==4){
                EXP++;
            }
            if(RC4.mark[i]==4){
                EXP++;
            }
            if(RC5.mark[i]==4){
                EXP++;
            }
        }
        if(nowFG && sctcnt>0){
            FGrem+=sctcnt+1;
            guidance.text="Additional "+(sctcnt+1).ToString()+" Free Spin Awarded.";
        }
        if(sctcnt==5){
            atom.anypay=bet*30*MLTP;
            win+=bet*30*MLTP;
            if(!nowFG){
                inFG=true;
                MLTP=5;
            }
        }else if(sctcnt==4){
            atom.anypay=bet*10*MLTP;
            win+=bet*10*MLTP;
            if(!nowFG){
                inFG=true;
                MLTP=2;
            }
        }else if(sctcnt==3){
            atom.anypay=bet*3*MLTP;
            win+=bet*3*MLTP;
            if(!nowFG){
                inFG=true;
                MLTP=1;
            }
        }
        if(EXP>=tgt[FLv] && nowFG){
            BNSP.text="EXP = "+EXP.ToString()+"/"+nxlv[FLv]+"   Free Lv."+FLv.ToString();
            guidance.text="Level Up! Additional Free Spins Awarded.";
            FLv++;
            FGrem+=3;
            nowFG=false;
            snd.PlayOneShot(SFX[14]);
            yield return new WaitForSeconds(2.8f);
            BGMct.ret();
        }
        if(BnsCrs){
            BnsCrs=false;
            win=0;
            inFG=true;
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
            //yield return new WaitForSeconds(1);
            GameMode=256;
            Ftest();
        }else if(GameMode==254){

        }else{
            GameMode=0;
        }
    }

    IEnumerator fin(){
        win=FGwin;
        FGwin=0;
        FLv=0;
        MLTP=1;
        state.text="Total Played Free Spin = "+fplayed.ToString();
        BGMct.StopBGM();
        wmtr.text="WIN\n"+win.ToString();
        if(ftotalwon>=bet*500){
            fres.FR(ftotalwon,3);
            yield return new WaitForSeconds(2);
            indct.SetActive(true);
            snd.PlayOneShot(SFX[25]);
            yield return new WaitForSeconds(15.7f);
        }else if(ftotalwon>=bet*200){
            fres.FR(ftotalwon,2);
            yield return new WaitForSeconds(2);
            indct.SetActive(true);
            snd.PlayOneShot(SFX[27]);
            yield return new WaitForSeconds(7.9f);
        }else if(ftotalwon>=bet*30){
            fres.FR(ftotalwon,1);
            yield return new WaitForSeconds(2);
            indct.SetActive(true);
            snd.PlayOneShot(SFX[15]);
            yield return new WaitForSeconds(6.6f);
        }else{
            fres.FR(ftotalwon,0);
            yield return new WaitForSeconds(2);
            indct.SetActive(true);
            snd.PlayOneShot(SFX[24]);
            yield return new WaitForSeconds(5.3f);
        }
        indct.SetActive(false);
        fres.Canc();
        ftotalwon=0;
        fplayed=0;
        if(win>0){
            StartCoroutine("Counting");
        }else{
            GameMode=0;
            wmtr.text="";
        }
    }
    IEnumerator Counting(){
        state.text="Winner!!";
        wmtr.text="";
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
            if(pd>=100000 && win-pd>=487){
                yield return new WaitForSeconds(0.01f);
                stn.credit+=487;
                pd+=486;
            }else if(pd>=20000 && win-pd>=73){
                yield return new WaitForSeconds(0.01f);
                stn.credit+=73;
                pd+=72;
            }else if(pd>=5000 && win-pd>=26){
                yield return new WaitForSeconds(0.01f);
                stn.credit+=26;
                pd+=25;
            }else if(pd>=1000 && win-pd>=7){
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
        ftotalwon+=win;
        snd.clip=SFX[21];
        SFX[3]=SFX[22];
        snd.Play();
        for(int pd=1;pd<=win;pd++){
            if(skip2){
                wmtr.text="WIN\n"+pd.ToString();
                FGwin+=(win-pd+1);
                break;
            }
            if(pd>=20000 && win-pd>=517){
                yield return new WaitForSeconds(0.01f);
                FGwin+=517;
                pd+=516;
            }else if(pd>=5000 && win-pd>=91){
                yield return new WaitForSeconds(0.01f);
                FGwin+=91;
                pd+=90;
            }else if(pd>=1000 && win-pd>=34){
                yield return new WaitForSeconds(0.01f);
                FGwin+=34;
                pd+=33;
            }else if(pd>=300 && win-pd>=13){
                yield return new WaitForSeconds(0.01f);
                FGwin+=13;
                pd+=12;
            }else if(pd>=150 && win-pd>=2){
                yield return new WaitForSeconds(0.01f);
                FGwin+=2;
                pd++;
            }else if(pd>=100){
                yield return new WaitForSeconds(0.015f);
                FGwin++;
            }else if(pd>=75){
                yield return new WaitForSeconds(0.02f);
                FGwin++;
            }else if(pd>=50){
                yield return new WaitForSeconds(0.03f);
                FGwin++;
            }else if(pd>=30){
                yield return new WaitForSeconds(0.04f);
                FGwin++;
            }else if(pd>=10){
                yield return new WaitForSeconds(0.055f);
                FGwin++;
            }else{
                yield return new WaitForSeconds(0.07f);
                FGwin++;
            }
            wmtr.text="WIN\n"+pd.ToString();
        }
        wmtr.text="WIN\n"+win.ToString();
        snd.Stop();
        snd.PlayOneShot(SFX[3]);
        FGrem--;
        inFG=false;
        yield return new WaitForSeconds(1);
        GameMode=256;
        Ftest();
    }
}
