using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GM3 : MonoBehaviour
{   //Deuces Wild
    public int bet,win,Mbet,handpower,GameMode,orgwin,dgbet,tryYL,saved;
    public bool DrawPoker;
    [SerializeField] int[] cnum=new int[5];
    [SerializeField] int[] suit=new int[5];
    int[] payt=new int[11];
    [SerializeField] bool[] holdflg=new bool[5];
    bool[] marking=new bool[5];
    bool skip2;
    public GameObject[] card;
    public GameObject[] unitUI;
    cardsp sw;
    public dshf cbox;
    public TextMeshProUGUI cmtr,bmtr,wmtr,pmtr,tblmtr,state,orw,dgw,tyl,svd,slc0,slc1,slc2,slc3,slc4,guidance;
    public GameObject[] heldm=new GameObject[5];
    public GameObject[] banr=new GameObject[10];
    public GameObject rtnbutton;
    public bgmMngr BGMct;
    public AudioClip SFX0,SFX1,SFX2,SFX3,SFX4,SFX5,SFX6,SFX7,SFX8,SFX9,SFX10,SFX11,SFX12,SFX13,SFX14,SFX15,SFX16;
    AudioSource snd;
    public scenechanger SC;

    void Start()
    {
        snd=GetComponent<AudioSource>();
        Mbet=10;
        win=0;
        GameMode=0;
        bet=0;
        wmtr.text="";
        pmtr.text="";
        unitUI[0].SetActive(true);
        unitUI[1].SetActive(false);
        ddindres();
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
                    guidance.text="[I/O] Insert 1 Coin/100 Coins\n[B] Bet 1 Credit\n[N] Repeat Bet and Draw (Bet Same Amount of Credit for Previous Game.)";
                }else{
                    guidance.text="[I/O] Insert 1 Coin/100 Coins\n[B] Bet 1 Credit";
                }
                
            }
        }else{
            rtnbutton.SetActive(false);
        }
        if(bet==Mbet){
            payt[10]=bet*1000;
        }else{
            payt[10]=bet*200;
        }
        payt[9]=bet*200;
        payt[8]=bet*20;
        payt[7]=bet*15;
        payt[6]=bet*8;
        payt[5]=bet*4;
        payt[4]=bet*3;
        payt[3]=bet*2;
        payt[2]=bet*2;
        payt[1]=bet*1;
        payt[0]=0;
        tblmtr.text=payt[10].ToString()+"\n"+payt[9].ToString()+"\n"+payt[8].ToString()+"\n"+payt[7].ToString()+"\n"+
                    payt[6].ToString()+"\n"+payt[5].ToString()+"\n"+payt[4].ToString()+"\n"+
                    payt[3].ToString()+"\n"+payt[2].ToString()+"\n"+payt[1].ToString();

        if(Input.GetKeyDown(KeyCode.I) && GameMode<=1 && stn.credit<10000){
            stn.credit++;
            snd.PlayOneShot(SFX0);
        }
        if(Input.GetKeyDown(KeyCode.O) && GameMode<=1 && stn.credit<10000){
            stn.credit+=100;
            snd.PlayOneShot(SFX0);
        }
        if(Input.GetKeyDown(KeyCode.B) && stn.credit>0 && GameMode<=1){
            if(GameMode==0){
                GameMode=1;
                bet=0;
                state.text="Play 1 to "+Mbet.ToString()+" Credits. Bet or Start.";
                guidance.text="[I/O] Insert 1 Coin/100 Coins\n[B] Bet 1 Credit\n[Space] Deal Cards";
                ddindres();
                for(int i=0;i<5;i++){
                    unitUI[0].SetActive(true);
                    unitUI[1].SetActive(false);
                    holdflg[i]=false;
                    heldm[i].SetActive(false);
                    sw=card[i].GetComponent<cardsp>();
                    sw.FCh(0);
                }
            }
            if(bet<Mbet){
                stn.credit--;
                bet++;
                snd.PlayOneShot(SFX4);
                if(bet==Mbet){
                    GameMode=2;
                    wmtr.text="";
                    pmtr.text="";
                    StartCoroutine("gamestart");
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
        if(Input.GetKeyDown(KeyCode.D) && GameMode==3){
            if(holdflg[0]){
                holdflg[0]=false;
                heldm[0].SetActive(false);
                snd.PlayOneShot(SFX3);
                if(marking[0]){
                    sw=card[0].GetComponent<cardsp>();
                    sw.anmflg=true;
                }
            }else{
                holdflg[0]=true;
                heldm[0].SetActive(true);
                snd.PlayOneShot(SFX2);
                if(marking[0]){
                    sw=card[0].GetComponent<cardsp>();
                    sw.anmflg=false;
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.F) && GameMode==3){
            if(holdflg[1]){
                holdflg[1]=false;
                heldm[1].SetActive(false);
                snd.PlayOneShot(SFX3);
                if(marking[1]){
                    sw=card[1].GetComponent<cardsp>();
                    sw.anmflg=true;
                }
            }else{
                holdflg[1]=true;
                heldm[1].SetActive(true);
                snd.PlayOneShot(SFX2);
                if(marking[1]){
                    sw=card[1].GetComponent<cardsp>();
                    sw.anmflg=false;
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.G) && GameMode==3){
            if(holdflg[2]){
                holdflg[2]=false;
                heldm[2].SetActive(false);
                snd.PlayOneShot(SFX3);
                if(marking[2]){
                    sw=card[2].GetComponent<cardsp>();
                    sw.anmflg=true;
                }
            }else{
                holdflg[2]=true;
                heldm[2].SetActive(true);
                snd.PlayOneShot(SFX2);
                if(marking[2]){
                    sw=card[2].GetComponent<cardsp>();
                    sw.anmflg=false;
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.H) && GameMode==3){
            if(holdflg[3]){
                holdflg[3]=false;
                heldm[3].SetActive(false);
                snd.PlayOneShot(SFX3);
                if(marking[3]){
                    sw=card[3].GetComponent<cardsp>();
                    sw.anmflg=true;
                }
            }else{
                holdflg[3]=true;
                heldm[3].SetActive(true);
                snd.PlayOneShot(SFX2);
                if(marking[3]){
                    sw=card[3].GetComponent<cardsp>();
                    sw.anmflg=false;
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.J) && GameMode==3){
            if(holdflg[4]){
                holdflg[4]=false;
                heldm[4].SetActive(false);
                snd.PlayOneShot(SFX3);
                if(marking[4]){
                    sw=card[4].GetComponent<cardsp>();
                    sw.anmflg=true;
                }
            }else{
                holdflg[4]=true;
                heldm[4].SetActive(true);
                snd.PlayOneShot(SFX2);
                if(marking[4]){
                    sw=card[4].GetComponent<cardsp>();
                    sw.anmflg=false;
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.Space) && GameMode==3){
            GameMode=4;
            StartCoroutine("deal");
        }
        if(Input.GetKeyDown(KeyCode.C) && GameMode==5){
            GameMode=99;
            StartCoroutine("collect");
        }
        if(Input.GetKeyDown(KeyCode.N) && stn.credit+win>=bet && bet!=0 && GameMode==5){
            GameMode=99;
            StartCoroutine("collectRS");
        }
        if(Input.GetKeyDown(KeyCode.Space) && GameMode==99){
            skip2=true;
        }
        if((Input.GetKeyDown(KeyCode.L) || Input.GetKeyDown(KeyCode.K)) && GameMode==5 && win<=5000){
            GameMode=50;
            wmtr.text="";
            int dtr=0;
            if(Input.GetKeyDown(KeyCode.K) && win>1){
                dtr=1;
            }else{
                dtr=0;
            }
            StartCoroutine(DUP(dtr));
        }
        if(Input.GetKeyDown(KeyCode.F) && GameMode==51){
            GameMode=52;
            StartCoroutine(Djudge(1));
        }
        if(Input.GetKeyDown(KeyCode.G) && GameMode==51){
            GameMode=52;
            StartCoroutine(Djudge(2));
        }
        if(Input.GetKeyDown(KeyCode.H) && GameMode==51){
            GameMode=52;
            StartCoroutine(Djudge(3));
        }
        if(Input.GetKeyDown(KeyCode.J) && GameMode==51){
            GameMode=52;
            StartCoroutine(Djudge(4));
        }
    }

    IEnumerator gamestart(){
        handpower=0;
        tryYL=0;
        saved=0;
        skip2=false;
        unitUI[0].SetActive(true);
        unitUI[1].SetActive(false);
        ddindres();
        state.text="Good Luck!";
        guidance.text="";
        cbox.ds();
        for(int v=0;v<banr.Length;v++){
            banr[v].SetActive(false);
        }
        for(int v=0;v<5;v++){
            marking[v]=false;
        }
        yield return new WaitForSeconds(0.1f);
        Debug.Log("GAMESTART");
        for(int i=0;i<5;i++){
            sw=card[i].GetComponent<cardsp>();
            sw.cid=cbox.deck[i];
            if(sw.cid%13!=0){
                cnum[i]=sw.cid%13+2;
                suit[i]=Mathf.FloorToInt(sw.cid/13)+1;
            }else{
                cnum[i]=99;
                suit[i]=0;
            }
            sw.FCh(1);
            snd.PlayOneShot(SFX1);
            yield return new WaitForSeconds(0.22f);
        }
        handpower=judge();
        if(DrawPoker){
            if(handpower>0){
                banr[handpower-1].SetActive(true);
                snd.PlayOneShot(SFX5);
                for(int h=0;h<5;h++){
                    if(marking[h]){
                        sw=card[h].GetComponent<cardsp>();
                        sw.anmflg=true;
                    }
                }
            }else{
                for(int h=0;h<5;h++){
                    marking[h]=false;
                }
            }
            GameMode=3;
            state.text="Hold and Draw.";
            guidance.text="[D] Hold to 1st Card\n[F] Hold to 2nd Card\n[G] Hold to 3rd Card\n[H] Hold to 4th Card\n[J] Hold to 5th Card\n[Space] Exchange Cards\nPress Again [D/F/G/H/J]...Cancel of Hold";
        }else{
            win=payt[handpower];
            if(handpower>0){
                banr[handpower-1].SetActive(true);
            }
            if(win>0){
                orgwin=win;
                WINSOUND();
                wmtr.text="WIN\n"+win.ToString();
                StartCoroutine("wait1s");
            }else{
                GameMode=0;
                Debug.Log("GAMEOVER");
            }
        }
    }
    IEnumerator deal(){
        snd.PlayOneShot(SFX9);
        state.text="Good Luck!";
        guidance.text="";
        if(handpower>0){
            banr[handpower-1].SetActive(false);
        }
        for(int q=0;q<5;q++){
            sw=card[q].GetComponent<cardsp>();
            sw.anmflg=false;
        }
        handpower=0;
        int cn=4;
        for(int q=0;q<5;q++){
            if(!holdflg[q]){
                sw=card[q].GetComponent<cardsp>();
                sw.FCh(2);
                yield return new WaitForSeconds(0.22f);
            }
        }
        Debug.Log("DEAL!!!!!");
        for(int q=0;q<5;q++){
            if(!holdflg[q]){
                cn++;
                sw=card[q].GetComponent<cardsp>();
                sw.cid=cbox.deck[cn];
                if(sw.cid%13!=0){
                    cnum[q]=sw.cid%13+2;
                    suit[q]=Mathf.FloorToInt(sw.cid/13)+1;
                }else{
                    cnum[q]=99;
                    suit[q]=0;
                }
                sw.FCh(1);
                snd.PlayOneShot(SFX1);
                yield return new WaitForSeconds(0.22f);
            }
        }
        handpower=judge();
        win=payt[handpower];
        if(handpower>0){
            banr[handpower-1].SetActive(true);
        }
        if(win>0){
            orgwin=win;
            WINSOUND();
            wmtr.text="WIN\n"+win.ToString();
            StartCoroutine("wait1s");
        }else{
            GameMode=0;
            Debug.Log("GAMEOVER");
        }
    }
    IEnumerator wait1s(){
        state.text="Winner!!";
        yield return new WaitForSeconds(0.5f);
        state.text="Winner!! Collect or Double Down";
        if(win<10000){
            guidance.text="[L] Try Double Down Game\n[K] Try Half Double Down Game (Use Half of Winnings at Play.)\n[C] Collect\n[N] Collect and Repeat Bet and Draw";
        }else{
            guidance.text="[C] Collect\n[N] Collect and Repeat Bet and Draw";
        }
        GameMode=5;
    }
    IEnumerator collect(){
        state.text="Winner!!";
        guidance.text="";
        if(saved>0){
            win+=saved;
            wmtr.text="WIN\n"+win.ToString();
        }
        if(win>=200){
            BGMct.StartBGM();
        }
        for(int pd=1;pd<=win;pd++){
            if(skip2){
                pmtr.text="PAID\n"+win.ToString();
                stn.credit+=(win-pd+1);
                break;
            }
            if(win<200){
                snd.PlayOneShot(SFX16);
            }
            if(pd>=3000){
                yield return new WaitForSeconds(0.0005f);
            }else if(pd>=500){
                yield return new WaitForSeconds(0.01f);
            }else{
                yield return new WaitForSeconds(0.05f);
            }
            pmtr.text="PAID\n"+pd.ToString();
            stn.credit++;
        }
        BGMct.StopBGM();
        GameMode=0;
        snd.PlayOneShot(SFX8);
        state.text="Game Over. Insert Coin or [B]Bet!";
    }
    IEnumerator collectRS(){
        state.text="Winner!!";
        guidance.text="";
        if(saved>0){
            win+=saved;
            wmtr.text="WIN\n"+win.ToString();
        }
        if(win>=200){
            BGMct.StartBGM();
        }
        for(int pd=1;pd<=win;pd++){
            if(skip2){
                pmtr.text="PAID\n"+win.ToString();
                stn.credit+=(win-pd+1);
                break;
            }
            if(win<200){
                snd.PlayOneShot(SFX16);
            }
            if(pd>=3000){
                yield return new WaitForSeconds(0.0005f);
            }else if(pd>=500){
                yield return new WaitForSeconds(0.01f);
            }else{
                yield return new WaitForSeconds(0.05f);
            }
            pmtr.text="PAID\n"+pd.ToString();
            stn.credit++;
        }
        BGMct.StopBGM();
        snd.PlayOneShot(SFX8);
        yield return new WaitForSeconds(0.2f);
        REPEATSTART();
    }
    IEnumerator DUP(int kf){
        snd.PlayOneShot(SFX4);
        for(int v=0;v<banr.Length;v++){
            banr[v].SetActive(false);
        }
        state.text="Good Luck!";
        guidance.text="";
        ddindres();
        cbox.ds();
        switch(kf){
            case 1:saved+=Mathf.CeilToInt(win/2); dgbet=Mathf.FloorToInt(win/2); tryYL=dgbet*2; break;
            default:dgbet=win; tryYL=dgbet*2; break;
        }
        for(int v=0;v<5;v++){
            heldm[v].SetActive(false);
        }
        orw.text=orgwin.ToString();
        ddtx();
        unitUI[0].SetActive(false);
        unitUI[1].SetActive(true);
        for(int q=0;q<5;q++){
            sw=card[q].GetComponent<cardsp>();
            sw.FCh(2);
            yield return new WaitForSeconds(0.22f);
        }
        slc0.text="DEALER";
        sw=card[0].GetComponent<cardsp>();
        sw.cid=cbox.deck[0];
        if(sw.cid%13!=0){
            cnum[0]=sw.cid%13+2;
        }else{
            cnum[0]=99;
        }
        sw.FCh(1);
        snd.PlayOneShot(SFX1);
        yield return new WaitForSeconds(0.22f);
        state.text="Select a Card. Which is Higher Than Dealer's?";
        guidance.text="1st Card is Dealer's Card. You Must Overcome To Dealer!!\n2 is Highest and 3 is Lowest. If Your Select is Higher, You are the Winner.\n[F] Select to 2nd Card\n[G] Select to 3rd Card\n[H] Select to 4th Card\n[J] Select to 5th Card";
        GameMode=51;
    }
    IEnumerator Djudge(int sc){
        snd.PlayOneShot(SFX9);
        state.text="Good Luck!";
        guidance.text="";
        int[] sel1={2,3,4,1},sel2={1,3,4,2},sel3={1,2,4,3},sel4={1,2,3,4};
        switch(sc){
            case 1:
                slc1.text="PLAYER";
                for(int i=0;i<3;i++){
                    sw=card[sel1[i]].GetComponent<cardsp>();
                    sw.cid=cbox.deck[sel1[i]];
                    sw.FCh(1);
                    snd.PlayOneShot(SFX1);
                    yield return new WaitForSeconds(0.22f);
                } break;
            case 2:
                slc2.text="PLAYER";
                for(int i=0;i<3;i++){
                    sw=card[sel2[i]].GetComponent<cardsp>();
                    sw.cid=cbox.deck[sel2[i]];
                    sw.FCh(1);
                    snd.PlayOneShot(SFX1);
                    yield return new WaitForSeconds(0.22f);
                } break;
            case 3:
                slc3.text="PLAYER";
                for(int i=0;i<3;i++){
                    sw=card[sel3[i]].GetComponent<cardsp>();
                    sw.cid=cbox.deck[sel3[i]];
                    sw.FCh(1);
                    snd.PlayOneShot(SFX1);
                    yield return new WaitForSeconds(0.22f);
                } break;
            case 4:
                slc4.text="PLAYER";
                for(int i=0;i<3;i++){
                    sw=card[sel4[i]].GetComponent<cardsp>();
                    sw.cid=cbox.deck[sel4[i]];
                    sw.FCh(1);
                    snd.PlayOneShot(SFX1);
                    yield return new WaitForSeconds(0.22f);
                } break;
        }
        yield return new WaitForSeconds(0.22f);
        sw=card[sc].GetComponent<cardsp>();
        sw.cid=cbox.deck[sc];
        if(sw.cid%13!=0){
            cnum[sc]=sw.cid%13+2;
        }else{
            cnum[sc]=99;
        }
        sw.FCh(1);
        snd.PlayOneShot(SFX1);
        yield return new WaitForSeconds(0.22f);
        if(cnum[0]<cnum[sc]){
            win=tryYL;
            tryYL=win*2;
            if(win>=200){
                snd.PlayOneShot(SFX15);
            }else{
                snd.PlayOneShot(SFX12);
            }
            ddtx();
            wmtr.text="WIN\n"+win.ToString();
            state.text="You Win!!";
            if(win<5000){
                guidance.text="[L] Try Double Down Game\n[K] Try Half Double Down Game (Use Half of Winnings at Play.)\n[C] Collect\n[N] Collect and Repeat Bet and Draw";
            }else{
                guidance.text="Maximum Double Down Wager is 5000 Credits.\n[C] Collect\n[N] Collect and Repeat Bet and Draw";
            }
            GameMode=5;
        }else if(cnum[0]==cnum[sc]){
            win=dgbet;
            snd.PlayOneShot(SFX14);
            wmtr.text="WIN\n"+win.ToString();
            state.text="Push! Try Again.";
            guidance.text="[L] Try Double Down Game\n[K] Try Half Double Down Game (Use Half of Winnings at Play.)\n[C] Collect\n[N] Collect and Repeat Bet and Draw";
            GameMode=5;
        }else{
            win=0;
            snd.PlayOneShot(SFX13);
            if(saved>0){
                state.text="Saved Win.";
                win+=saved;
                ddtx();
                wmtr.text="WIN\n"+win.ToString();
                GameMode=99;
                if(win>=200){
                    BGMct.StartBGM();
                }
                for(int pd=1;pd<=win;pd++){
                    if(skip2){
                        pmtr.text="PAID\n"+win.ToString();
                        stn.credit+=(win-pd+1);
                        break;
                    }
                    if(win<200){
                        snd.PlayOneShot(SFX16);
                    }
                    yield return new WaitForSeconds(0.04f);
                    pmtr.text="PAID\n"+pd.ToString();
                    stn.credit++;
                }
                BGMct.StopBGM();
                snd.PlayOneShot(SFX8);
            }
            GameMode=0;
        }
    }

    int judge(){
        int yk=0,pairs=0,Mrn=0,cMax=0,cMin=15,inJO=0;
        bool isStr=false,isFls=false,isRyl=false;
        for(int a=0;a<4;a++){
            for(int b=a+1;b<5;b++){
                if(cnum[a]==cnum[b] && cnum[a]!=99){
                    pairs++;
                    marking[a]=true;
                    marking[b]=true;
                    if(cnum[a]>Mrn){
                        Mrn=cnum[a];
                    }
                }
            }
        }
        for(int n=0;n<5;n++){
            if(cnum[n]==99){
                inJO++;
                marking[n]=true;
            }
        }
        if(inJO==4){
            yk=9;
        }else if(inJO==3){
            switch(pairs){
                case 1: yk=7; break;
                case 0:
                    for(int u=0;u<5;u++){
                        if(cnum[u]>cMax && cnum[u]!=99){
                            cMax=cnum[u];
                        }
                        if(cnum[u]<cMin){
                            cMin=cnum[u];
                        }
                    }
                    if(cMax-cMin>=1 && cMax-cMin<=4){
                        isStr=true;
                        if(cMax==14 || (cMax==13 && (cMin>=10 && cMin<=12)) || (cMax==12 && (cMin==10 || cMin==11)) || (cMax==11 && cMin==10)){
                            isRyl=true;
                        }
                    }else{
                        int passed=0;
                        for(int m=0;m<5;m++){
                            if(cnum[m]==14 || cnum[m]==99 || (cnum[m]<=5 && cnum[m]>=2)){
                                passed++;
                            }
                        }
                        if(passed==5){
                            isStr=true;
                        }
                    }
                    int thru=0,chkst=0;
                    for(int i=0;i<5;i++){
                        if(suit[i]==chkst || suit[i]==0){
                            thru++;
                        }else if(chkst==0){
                            thru++;
                            chkst=suit[i];
                        }else{
                            break;
                        }
                    }
                    if(thru==5){
                        isFls=true;
                    }
                    if(isStr || isFls){
                        for(int c=0;c<5;c++){
                            marking[c]=true;
                        }
                    }
                    if(isStr && isFls){
                        if(isRyl){
                            yk=8;
                        }else{
                            yk=6;
                        }
                    }
                    if(yk==0){
                        yk=5;
                        for(int d=0;d<5;d++){
                            if(cnum[d]==cMax){
                                marking[d]=true;
                            }
                        }
                    }
                break;
            }
        }else if(inJO==2){
            switch(pairs){
                case 3: yk=7; break;
                case 1: yk=5; break;
                case 0:
                    for(int u=0;u<5;u++){
                        if(cnum[u]>cMax && cnum[u]!=99){
                            cMax=cnum[u];
                        }
                        if(cnum[u]<cMin){
                            cMin=cnum[u];
                        }
                    }
                    if(cMax-cMin>=2 && cMax-cMin<=4){
                        isStr=true;
                        if(cMax==14 || (cMax==13 && (cMin==10 || cMin==11)) || (cMax==12 && cMin==10)){
                            isRyl=true;
                        }
                    }else{
                        int passed=0;
                        for(int m=0;m<5;m++){
                            if(cnum[m]==14 || cnum[m]==99 || (cnum[m]<=5 && cnum[m]>=2)){
                                passed++;
                            }
                        }
                        if(passed==5){
                            isStr=true;
                        }
                    }
                    int thru=0,chkst=0;
                    for(int i=0;i<5;i++){
                        if(suit[i]==chkst || suit[i]==0){
                            thru++;
                        }else if(chkst==0){
                            thru++;
                            chkst=suit[i];
                        }else{
                            break;
                        }
                    }
                    if(thru==5){
                        isFls=true;
                    }
                    if(isStr || isFls){
                        for(int c=0;c<5;c++){
                            marking[c]=true;
                        }
                    }
                    if(isStr && isFls){
                        if(isRyl){
                            yk=8;
                        }else{
                            yk=6;
                        }
                    }else if(isFls){
                        yk=3;
                    }else if(isStr){
                        yk=2;
                    }
                    if(yk==0){
                        yk=1;
                        for(int d=0;d<5;d++){
                            if(cnum[d]==cMax){
                                marking[d]=true;
                            }
                        }
                    }
                break;
            }
        }else if(inJO==1){
            switch(pairs){
                case 6: yk=7; break;
                case 3: yk=5; break;
                case 2: yk=4; break;
                case 1: yk=1; break;
                case 0:
                    for(int u=0;u<5;u++){
                        if(cnum[u]>cMax && cnum[u]!=99){
                            cMax=cnum[u];
                        }
                        if(cnum[u]<cMin){
                            cMin=cnum[u];
                        }
                    }
                    if(cMax-cMin==4 || cMax-cMin==3){
                        isStr=true;
                        if(cMax==14 || (cMax==13 && cMin==10)){
                            isRyl=true;
                        }
                    }else{
                        int passed=0;
                        for(int m=0;m<5;m++){
                            if(cnum[m]==14 || cnum[m]==99 || (cnum[m]<=5 && cnum[m]>=2)){
                                passed++;
                            }
                        }
                        if(passed==5){
                            isStr=true;
                        }
                    }
                    int thru=0,chkst=0;
                    for(int i=0;i<5;i++){
                        if(suit[i]==chkst || suit[i]==0){
                            thru++;
                        }else if(chkst==0){
                            thru++;
                            chkst=suit[i];
                        }else{
                            break;
                        }
                    }
                    if(thru==5){
                        isFls=true;
                    }
                    if(isStr || isFls){
                        for(int c=0;c<5;c++){
                            marking[c]=true;
                        }
                    }
                    if(isStr && isFls){
                        if(isRyl){
                            yk=8;
                        }else{
                            yk=6;
                        }
                    }else if(isFls){
                        yk=3;
                    }else if(isStr){
                        yk=2;
                    }
                break;
            }
        }else{
            switch(pairs){
                case 6: yk=5; break;
                case 4: yk=4; break;
                case 3: yk=1; break;
                case 0:
                    for(int u=0;u<5;u++){
                        if(cnum[u]>cMax){
                            cMax=cnum[u];
                        }
                        if(cnum[u]<cMin){
                            cMin=cnum[u];
                        }
                    }
                    if(cMax-cMin==4){
                        isStr=true;
                        if(cMax==14){
                            isRyl=true;
                        }
                    }else{
                        int passed=0;
                        for(int m=0;m<5;m++){
                            if(cnum[m]==14 || (cnum[m]<=5 && cnum[m]>=2)){
                                passed++;
                            }
                        }
                        if(passed==5){
                            isStr=true;
                        }
                    }
                    if(suit[0]==suit[1] && suit[0]==suit[2] && suit[0]==suit[3] && suit[0]==suit[4]){
                        isFls=true;
                    }
                    if(isStr || isFls){
                        for(int c=0;c<5;c++){
                            marking[c]=true;
                        }
                    }
                    if(isStr && isFls){
                        if(isRyl){
                            yk=10;
                        }else{
                            yk=6;
                        }
                    }else if(isFls){
                        yk=3;
                    }else if(isStr){
                        yk=2;
                    }
                break;
            }
        }
        
        return yk;
    }
    void REPEATSTART(){
        stn.credit-=bet;
            snd.PlayOneShot(SFX4);
            for(int i=0;i<5;i++){
                holdflg[i]=false;
                heldm[i].SetActive(false);
                sw=card[i].GetComponent<cardsp>();
                sw.FCh(0);
            }
            GameMode=2;
            wmtr.text="";
            pmtr.text="";
            StartCoroutine("gamestart");
    }
    void WINSOUND(){
        if(win>=200){
            snd.PlayOneShot(SFX11);
        }else if(win>=50){
            snd.PlayOneShot(SFX10);
        }else{
            snd.PlayOneShot(SFX6);
        }
    }
    void ddtx(){
        skip2=false;
        dgw.text=dgbet.ToString();
        tyl.text=tryYL.ToString();
        svd.text=saved.ToString();
    }
    void ddindres(){
        slc0.text="";
        slc1.text="";
        slc2.text="";
        slc3.text="";
        slc4.text="";
    }
}
