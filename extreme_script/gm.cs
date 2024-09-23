using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class gm : MonoBehaviour
{
    int mode,win,paid,totalbet,ank,flgm,ra,dgbet,collectable,trycnt,saved,selectreel,spbonuslimit;
    float timer,cptim;
    int[] betline=new int[8],linewin=new int[8],anywin=new int[8],sympat=new int[9],dsymp=new int[4];
    public int[] stoproot={0,1,2,5,8,7,6,3,4},defsr={0,1,2,5,8,7,6,3,4};
    public reelscr[] reels,dreel;
    public int mbet;
    public float gameSpd=1.0f;
    public TextMeshProUGUI creditmeter,betmeter,winmeter,paidmeter,orgwin,clctmeter,ddbetmeter,trymeter,nxwinmeter,svdmeter,lmtmeter;
    public TextMeshProUGUI[] lbmeter,wlmeter,wameter,spbmeter,sel;
    public AudioClip[] se;
    AudioSource snd;
    public bgmMngr bgms;
    public GameObject bimas;
    bool[] lpchk=new bool[9];
    public lampctr[] windowlamp,txlmp;
    bool clsk;
    public Camera cam;
    public mtxctrlr mtc;

    void Start()
    {
        mode=0;
        ank=0;
        saved=0;
        snd=GetComponent<AudioSource>();
        timer=0.0f;
        paidmeter.text="POWER-ON";
        spbonuslimit=10000000;
        pub.maxwager=mbet;
        Time.timeScale=1.0f;
        /*if(pub.credit>0){
            mtc.htlvc(1);
        }else{
            mtc.htlvc(0);
        }*/
    }

    void Update()
    {
        cptim+=Time.deltaTime;
        creditmeter.text=pub.credit.ToString();
        betmeter.text=totalbet.ToString();
        if(win>0){
            winmeter.text=win.ToString();
        }else{
            winmeter.text="";
        }
        if(totalbet>0){
            lmtmeter.text=(totalbet*50000).ToString("N0");
        }else{
            lmtmeter.text="";
        }
        if(Input.GetKeyDown(KeyCode.I)){
            pub.credit++;
            snd.PlayOneShot(se[0]);
            gotx();
        }
        if(Input.GetKeyDown(KeyCode.U)){
            pub.credit+=10;
            snd.PlayOneShot(se[0]);
            gotx();
        }
        if(Input.GetKeyDown(KeyCode.Y)){
            pub.credit+=1000;
            snd.PlayOneShot(se[0]);
            gotx();
        }
        if(Input.GetKeyDown(KeyCode.B) && pub.credit>0){ //1bet
            if(mode==0){
                betres();
                bet();
                betsnd();
            }else if(mode==1 && totalbet<mbet){
                bet();
                betsnd();
            }
        }
        if(Input.GetKeyDown(KeyCode.V)){ //repeatbet
            if(mode<=1 && pub.credit>0){
                if(mode==0){
                    betres();
                }
                if(totalbet<=mbet-8 && pub.credit>7){
                    pub.credit-=8;
                    for(int i=0;i<betline.Length;i++){
                        betline[i]++;
                        lbmeter[i].text=betline[i].ToString();
                    }
                    betsnd();
                }else if(totalbet<mbet && pub.credit>0){
                    while(pub.credit>0 && totalbet!=mbet){
                        bet();
                    }
                    betsnd();
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.M) && pub.credit>0 && mode<=1){ //maxbet
            if(mode==0){
                betres();
            }
            while(pub.credit>0 && totalbet!=mbet){
                bet();
            }
            betsnd();
        }
        if(Input.GetKeyDown(KeyCode.Return)){ //gamestart
            if(mode==1){
                startmreel();
                snd.PlayOneShot(se[8]);
            }
            clsk=true;
        }
        if(Input.GetKeyDown(KeyCode.Space) && mode==0 && pub.credit>=totalbet && totalbet!=0){ //repeat
            pub.credit-=totalbet;
            bimas.SetActive(true);
            cam.transform.position=new Vector3(0.0f,0.5f,-10.0f);
            betsnd();
            startmreel();
        }
        if(Input.GetKeyDown(KeyCode.Space) && (mode==4 || mode==5) && (pub.credit+win+collectable+saved)>=totalbet && totalbet!=0 && cptim>=0.3f){ //repeat
            mode=50;
            if(trycnt==0){
                cam.transform.position=new Vector3(0.0f,0.5f,-10.0f);
                bimas.SetActive(true);
            }
            for(int i=0;i<9;i++){
                windowlamp[i].lampchange(false);
            }
            mtc.paids();
            StartCoroutine(coincollect(true));
        }
        if(Input.GetKeyDown(KeyCode.C) && (mode==4 || mode==5)){ //collect
            mode=50;
            if(trycnt==0){
                cam.transform.position=new Vector3(0.0f,0.5f,-10.0f);
                bimas.SetActive(true);
            }
            for(int i=0;i<9;i++){
                windowlamp[i].lampchange(false);
            }
            mtc.paids();
            StartCoroutine(coincollect(false));
        }
        if((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.K))&& (mode==4 || mode==5)){ //startdouble
            if(mode==4){
                mode=5;
                godd();
            }
            if(trycnt==0){
                if(Input.GetKeyDown(KeyCode.K) && win>1){
                    dgbet=win/2;
                    collectable/=2;
                    if(win%2==0){
                        saved+=win/2;
                    }else{
                        saved+=(win/2)+1;
                    }
                }else{
                    dgbet=win;
                }
            }else{
                if(Input.GetKeyDown(KeyCode.K) && collectable>1){
                    dgbet=collectable/2;
                    if(collectable%2==0){
                        saved+=collectable/2;
                    }else{
                        saved+=(collectable/2)+1;
                    }
                    collectable/=2;
                }else{
                    dgbet=collectable;
                }
            }
            startdreel();
            ddbetmeter.text=dgbet.ToString();
            nxwinmeter.text=(dgbet*2).ToString();
            clctmeter.text="0";
            svdmeter.text=saved.ToString();
            trycnt++;
            trymeter.text=trycnt.ToString();
            int[] sbcn={1,2,3,4,7,15,2000};
            for(int i=0;i<7;i++){
                txlmp[i].lampchange(false);
                if(dgbet*sbcn[i]>spbonuslimit){
                    spbmeter[i].text=spbonuslimit.ToString();
                }else{
                    spbmeter[i].text=(dgbet*sbcn[i]).ToString();
                }
            }
            win=0;
        }
        if(mode==2){
            timer+=Time.deltaTime;
            if(Input.GetKey(KeyCode.Return) && timer>=0.43f){
                mode=3;
                for(int i=flgm;i<reels.Length;i++){
                    reels[stoproot[i]].reelstop(sympat[i],false);
                }
            }
            if(timer>=0.95f){
                timer=0.53f;
                reels[stoproot[flgm]].reelstop(sympat[flgm],true);
                flgm++;
                if(flgm>=reels.Length){
                    mode=3;
                    timer=0.0f;
                }
            }
        }
        if(mode==3){
            if(reels[0].stopchk && reels[1].stopchk && reels[2].stopchk && reels[3].stopchk && reels[4].stopchk && reels[5].stopchk && reels[6].stopchk && reels[7].stopchk && reels[8].stopchk){
                bgms.StopBGM();
                judge();
            }
        }
        if(mode==7){
            timer+=Time.deltaTime;
            if(timer>=0.01f){
                timer=0.0f;
                mode=4;
                mtc.wnr();
                for(int i=0;i<9;i++){
                    if(lpchk[i]){
                        windowlamp[i].lampchange(true);
                    }else{
                        windowlamp[i].lampchange(false);
                    }
                }
                winsound(win);
            }
        }
        if(mode==4 || mode==98){
            timer+=Time.deltaTime;
            if(timer>=6.5f){
                if(mode==98){
                    mode=50;
                    for(int i=0;i<9;i++){
                        windowlamp[i].lampchange(false);
                    }
                    mtc.paids();
                    StartCoroutine(coincollect(false));
                }else{
                    mode=5;
                    godd();
                }
            }
        }
        if(mode==11){
            timer+=Time.deltaTime;
            if(timer>=1.05f){
                dsymp[0]=rotdsym();
                for(int i=1;i<=3;i++){
                    dsymp[i]=rotpsym();
                }
                dreel[0].reelstop(dsymp[0],true);
                timer=0.0f;
                mtc.srl();
                mode=12;
            }
        }
        if(mode==12){
            if(Input.GetKeyDown(KeyCode.G)){
                selectreel=1;
                mode=13;
                snd.PlayOneShot(se[8]);
                sel[0].text="▲PLAYER▲";
                StartCoroutine("selectL");
            }else if(Input.GetKeyDown(KeyCode.H)){
                selectreel=2;
                mode=13;
                snd.PlayOneShot(se[8]);
                sel[1].text="▲PLAYER▲";
                StartCoroutine("selectC");
            }else if(Input.GetKeyDown(KeyCode.J)){
                selectreel=3;
                mode=13;
                snd.PlayOneShot(se[8]);
                sel[2].text="▲PLAYER▲";
                StartCoroutine("selectR");
            }
        }
        if(mode==13){
            if(dreel[1].stopchk && dreel[2].stopchk && dreel[3].stopchk){
                bgms.StopBGM();
                djudge();
            }
        }
    }

    IEnumerator coincollect(bool n){
        mode=50;
        clsk=false;
        paid=0;
        paidmeter.text="PAID:"+paid.ToString();
        if(collectable>0){
            win=collectable;
            collectable=0;
        }
        if(saved>0){
            win+=saved;
            saved=0;
        }
        for(int i=0;i<7;i++){
            txlmp[i].lampchange(false);
        }
        while(paid<win){
            yield return new WaitForSeconds(0.08f);
            if(clsk){
                pub.credit+=win-paid;
                paid+=win-paid;
            }else if(paid>=970){
                if(win-paid<63){
                    pub.credit+=win-paid;
                    paid+=win-paid;
                }else{
                    pub.credit+=63;
                    paid+=63;
                }
            }else if(paid>=390){
                if(win-paid<29){
                    pub.credit+=win-paid;
                    paid+=win-paid;
                }else{
                    pub.credit+=29;
                    paid+=29;
                }
            }else if(paid>=130){
                if(win-paid<13){
                    pub.credit+=win-paid;
                    paid+=win-paid;
                }else{
                    pub.credit+=13;
                    paid+=13;
                }
            }else if(paid>=60){
                if(win-paid<7){
                    pub.credit+=win-paid;
                    paid+=win-paid;
                }else{
                    pub.credit+=7;
                    paid+=7;
                }
            }else if(paid>=30){
                if(win-paid<3){
                    pub.credit+=win-paid;
                    paid+=win-paid;
                }else{
                    pub.credit+=3;
                    paid+=3;
                }
            }else{
                pub.credit++;
                paid++;
            }
            snd.PlayOneShot(se[5]);
            paidmeter.text="PAID:"+paid.ToString();
        }
        if(n){
            yield return new WaitForSeconds(0.08f);
            pub.credit-=totalbet;
            bimas.SetActive(true);
            cam.transform.position=new Vector3(0.0f,0.5f,-10.0f);
            betsnd();
            startmreel();
        }else{
            mode=0;
            mtc.maintx();
        }
    }
    IEnumerator selectL(){
        mtc.gl();
        yield return new WaitForSeconds(0.3f);
        dreel[2].reelstop(dsymp[2],true);
        yield return new WaitForSeconds(0.2f);
        dreel[3].reelstop(dsymp[3],true);
        yield return new WaitForSeconds(0.5f);
        dreel[1].reelstop(dsymp[1],true);
    }
    IEnumerator selectC(){
        mtc.gl();
        yield return new WaitForSeconds(0.3f);
        dreel[1].reelstop(dsymp[1],true);
        yield return new WaitForSeconds(0.2f);
        dreel[3].reelstop(dsymp[3],true);
        yield return new WaitForSeconds(0.5f);
        dreel[2].reelstop(dsymp[2],true);
    }
    IEnumerator selectR(){
        mtc.gl();
        yield return new WaitForSeconds(0.3f);
        dreel[1].reelstop(dsymp[1],true);
        yield return new WaitForSeconds(0.2f);
        dreel[2].reelstop(dsymp[2],true);
        yield return new WaitForSeconds(0.5f);
        dreel[3].reelstop(dsymp[3],true);
    }

    void gotx(){
        if(mode==0){
            if(pub.credit>0){
                mtc.tlvc(1);
            }else{
                mtc.tlvc(0);
            }
        }else{

        }
    }
    void betres(){
        mode=1;
        ank=0;
        cam.transform.position=new Vector3(0.0f,0.5f,-10.0f);
        bimas.SetActive(true);
        for(int i=0;i<betline.Length;i++){
            betline[i]=0;
        }
        totalbet=betline[0]+betline[1]+betline[2]+betline[3]+betline[4]+betline[5]+betline[6]+betline[7];
        mtc.inscbs();
    }
    void bet(){
        pub.credit--;
        betline[ank]++;
        totalbet=betline[0]+betline[1]+betline[2]+betline[3]+betline[4]+betline[5]+betline[6]+betline[7];
        ank++;
        if(ank>7){
            ank=0;
        }
        for(int i=0;i<betline.Length;i++){
            lbmeter[i].text=betline[i].ToString();
        }
    }
    void betsnd(){
        totalbet=betline[0]+betline[1]+betline[2]+betline[3]+betline[4]+betline[5]+betline[6]+betline[7];
        if(totalbet==mbet){
            snd.PlayOneShot(se[2]);
        }else{
            snd.PlayOneShot(se[1]);
        }
    }
    void startmreel(){
        bimas.SetActive(true);
        mtc.gl();
        for(int i=0;i<betline.Length;i++){
            wlmeter[i].text="";
            wameter[i].text="";
        }
        timer=0.0f;
        mode=2;
        flgm=0;
        win=0;
        paidmeter.text="";
        bgms.StopBGM();
        int efjcn=0;
        for(int i=0;i<reels.Length;i++){
            lpchk[i]=false;
            stoproot[i]=defsr[i];
            sympat[i]=rotsymbol();
            reels[i].reelstart();
            if(sympat[i]==999){
                efjcn++;
            }
        }
        if(efjcn>0){
            ra=Random.Range(0,6+(efjcn*2));
            if(ra<(efjcn*2)){
                for(int j=0;j<6;j++){
                    int k=stoproot.Length-1;
                    while(k>0){
                        k--;
                        int f=Random.Range(0,k+1);
                        int y=stoproot[f];
                        stoproot[f]=stoproot[k];
                        stoproot[k]=y;
                    }
                }
            }
        }
        pub.gameSpeed=gameSpd;
        snd.PlayOneShot(se[3]);
    }
    int rotsymbol(){
        int a;
        ra=Random.Range(0,39800);
        ra=ra%398;
        if(ra>=390){
            a=999;
        }else if(ra>=380){
            a=20;
        }else if(ra>=365){
            a=10;
        }else if(ra>=335){
            a=3;
        }else if(ra>=290){
            a=2;
        }else if(ra>=215){
            a=1;
        }else{
            a=0;
        }
        return a;
    }
    int rotdsym(){
        int a;
        ra=Random.Range(0,28800);
        ra=ra%288;
        if(ra>=278){
            a=999;
        }else if(ra>=263){
            a=20;
        }else if(ra>=240){
            a=10;
        }else if(ra>=210){
            a=3;
        }else if(ra>=163){
            a=2;
        }else if(ra>=108){
            a=1;
        }else{
            a=0;
        }
        return a;
    }
    int rotpsym(){
        int a;
        ra=Random.Range(0,28800);
        ra=ra%288;
        if(ra>=285){
            a=999;
        }else if(ra>=277){
            a=20;
        }else if(ra>=265){
            a=10;
        }else if(ra>=240){
            a=3;
        }else if(ra>=202){
            a=2;
        }else if(ra>=145){
            a=1;
        }else{
            a=0;
        }
        return a;
    }
    
    void judge(){
        int[,] lts={{3,4,5},{0,1,2},{6,7,8},{0,4,8},{6,4,2},{0,3,6},{1,4,7},{2,5,8}};
        int[] ans={0,6,1,2,3,66,10,20};
        int counter,injo;
        bool[] apcn=new bool[9];
        for(int i=0;i<betline.Length;i++){
            if(reels[lts[i,0]].sym==999 && reels[lts[i,1]].sym==999 && reels[lts[i,2]].sym==999){
                linewin[i]=betline[i]*pub.wintableline[7];
            }else if(reels[lts[i,0]].fac && reels[lts[i,1]].fac && reels[lts[i,2]].fac){
                if((reels[lts[i,0]].sym==3 || reels[lts[i,0]].sym==999) && (reels[lts[i,1]].sym==3 || reels[lts[i,1]].sym==999) && (reels[lts[i,2]].sym==3 || reels[lts[i,2]].sym==999)){
                    linewin[i]=betline[i]*pub.wintableline[3];
                    for(int j=0;j<3;j++){
                        if(reels[lts[i,j]].sym==999){
                            linewin[i]*=7;
                        }
                    }
                }else if((reels[lts[i,0]].sym==2 || reels[lts[i,0]].sym==999) && (reels[lts[i,1]].sym==2 || reels[lts[i,1]].sym==999) && (reels[lts[i,2]].sym==2 || reels[lts[i,2]].sym==999)){
                    linewin[i]=betline[i]*pub.wintableline[2];
                    for(int j=0;j<3;j++){
                        if(reels[lts[i,j]].sym==999){
                            linewin[i]*=7;
                        }
                    }
                }else if((reels[lts[i,0]].sym==1 || reels[lts[i,0]].sym==999) && (reels[lts[i,1]].sym==1 || reels[lts[i,1]].sym==999) && (reels[lts[i,2]].sym==1 || reels[lts[i,2]].sym==999)){
                    linewin[i]=betline[i]*pub.wintableline[1];
                    for(int j=0;j<3;j++){
                        if(reels[lts[i,j]].sym==999){
                            linewin[i]*=7;
                        }
                    }
                }else{
                    linewin[i]=betline[i]*pub.wintableline[0];
                    for(int j=0;j<3;j++){
                        if(reels[lts[i,j]].sym==999){
                            linewin[i]*=7;
                        }
                    }
                }
            }else if(reels[lts[i,0]].sev && reels[lts[i,1]].sev && reels[lts[i,2]].sev){
                if((reels[lts[i,0]].sym==20 || reels[lts[i,0]].sym==999) && (reels[lts[i,1]].sym==20 || reels[lts[i,1]].sym==999) && (reels[lts[i,2]].sym==20 || reels[lts[i,2]].sym==999)){
                    linewin[i]=betline[i]*pub.wintableline[6];
                    for(int j=0;j<3;j++){
                        if(reels[lts[i,j]].sym==999){
                            linewin[i]*=7;
                        }
                    }
                }else if((reels[lts[i,0]].sym==10 || reels[lts[i,0]].sym==999) && (reels[lts[i,1]].sym==10 || reels[lts[i,1]].sym==999) && (reels[lts[i,2]].sym==10 || reels[lts[i,2]].sym==999)){
                    linewin[i]=betline[i]*pub.wintableline[5];
                    for(int j=0;j<3;j++){
                        if(reels[lts[i,j]].sym==999){
                            linewin[i]*=7;
                        }
                    }
                }else{
                    linewin[i]=betline[i]*pub.wintableline[4];
                    for(int j=0;j<3;j++){
                        if(reels[lts[i,j]].sym==999){
                            linewin[i]*=7;
                        }
                    }
                }
            }else{
                linewin[i]=0;
            }
            if(linewin[i]>0){
                for(int j=0;j<3;j++){
                    lpchk[lts[i,j]]=true;
                }
            }
        }
        for(int i=0;i<ans.Length;i++){
            counter=0;
            injo=0;
            for(int j=0;j<9;j++){
                if(ans[i]==6){
                    if(reels[j].fac){
                        counter++;
                        apcn[j]=true;
                    }
                    if(reels[j].sym==999){
                        injo++;
                    }
                }else if(ans[i]==66){
                    if(reels[j].sev){
                        counter++;
                        apcn[j]=true;
                    }
                    if(reels[j].sym==999){
                        injo++;
                    }
                }else{
                    if(reels[j].sym==ans[i]){
                        counter++;
                        if(ans[i]!=0){
                            apcn[j]=true;
                        }
                    }else if(reels[j].sym==999){
                        counter++;
                        injo++;
                        apcn[j]=true;
                    }
                }
            }
            anywin[i]=betline[i]*pub.wintableany[i,counter];
            for(int k=0;k<injo;k++){
                anywin[i]*=7;
            }
            if(anywin[i]>0){
                for(int l=0;l<9;l++){
                    if(apcn[l]){
                        lpchk[l]=apcn[l];
                        apcn[l]=false;
                    }
                }
            }else{
                for(int l=0;l<9;l++){
                    apcn[l]=false;
                }
            }
        }
        calc();
    }
    void calc(){
        Time.timeScale=1.0f;
        for(int i=0;i<linewin.Length;i++){
            win+=linewin[i];
            if(linewin[i]>0){
                wlmeter[i].text=linewin[i].ToString();
            }
        }
        for(int i=0;i<anywin.Length;i++){
            win+=anywin[i];
            if(anywin[i]>0){
                wameter[i].text=anywin[i].ToString();
            }
        }
        if(win>totalbet*50000){
            win=totalbet*50000;
        }
        if(win>0){
            timer=0.0f;
            cptim=0.0f;
            mode=7;
        }else{
            mode=0;
            mtc.maintx();
        }
    }

    void godd(){
        bgms.StopBGM();
        for(int i=0;i<9;i++){
            windowlamp[i].lampchange(false);
        }
        bimas.SetActive(false);
        mtc.pldd();
        trycnt=0;
        saved=0;
        collectable=win;
        orgwin.text=win.ToString();
        clctmeter.text=collectable.ToString();
        ddbetmeter.text="0";
        nxwinmeter.text=(win*2).ToString();
        svdmeter.text="0";
        trymeter.text="1";
        spbmeter[0].text="1 for 1"; spbmeter[1].text="2 for 1"; spbmeter[2].text="3 for 1"; spbmeter[3].text="4 for 1"; 
        spbmeter[4].text="7 for 1"; spbmeter[5].text="15 for 1"; spbmeter[6].text="2000 for 1";
        cam.transform.position=new Vector3(10.0f,0.5f,-10.0f);
        snd.PlayOneShot(se[4]);
    }
    void startdreel(){
        timer=0.0f;
        mode=11;
        bgms.StopBGM();
        mtc.gl();
        for(int i=0;i<dreel.Length;i++){
            dsymp[i]=rotsymbol();
            dreel[i].reelstart();
        }
        for(int j=0;j<3;j++){
            sel[j].text="";
        }
        snd.PlayOneShot(se[3]);
    }
    void winsound(int getmedal){
        if(getmedal>=10000){
            bgms.fanf(8);
            if((getmedal+saved)>100000){
                mode=98;
            }
        }else if(getmedal>=5000){
            timer=1.1f;
            bgms.fanf(7);
        }else if(getmedal>=2000){
            timer=2.1f;
            bgms.fanf(6);
        }else if(getmedal>=1000){
            timer=0.2f;
            bgms.fanf(5);
        }else if(getmedal>=500){
            timer=2.4f;
            bgms.fanf(4);
        }else if(getmedal>=200){
            timer=3.5f;
            bgms.fanf(3);
        }else if(getmedal>=100){
            timer=1.7f;
            bgms.fanf(2);
        }else if(getmedal>=50){
            timer=4.2f;
            bgms.fanf(1);
        }else{
            timer=4.4f;
            bgms.fanf(0);
        }
    }
    void djudge(){
        Time.timeScale=1.0f;
        if(dreel[0].sym<dreel[selectreel].sym){
            dwin(selectreel);
        }else if(dreel[0].sym==dreel[selectreel].sym){
            dpush(selectreel);
        }else{
            collectable=0;
        }
        collectable+=checkspbonus();
        if(collectable>dgbet){
            mode=5;
            winsound(collectable);
            clctmeter.text=collectable.ToString();
            if(mode==5){
                nxwinmeter.text=(collectable*2).ToString();
            }
        }else if(collectable==dgbet){
            mode=5;
            if(dreel[0].sym==dreel[selectreel].sym){
                snd.PlayOneShot(se[6]);
                mtc.ps();
            }else{
                winsound(collectable);
                mtc.lb();
            }
            clctmeter.text=collectable.ToString();
        }else{
            if(saved>0){
                mtc.svdw();
                StartCoroutine(coincollect(false));
            }else{
                mode=0;
                mtc.maintx();
            }
        }
    }
    void dwin(int p){
        collectable*=2;
        mode=5;
        mtc.wnr();
        if(dreel[p].sym==999){
            collectable*=7;
        }
    }
    void dpush(int p){
        mode=5;
        if(dreel[p].sym==999){
            collectable*=7;
        }
    }
    int checkspbonus(){
        int rel=0;
        if(dreel[1].sym==999 && dreel[2].sym==999 && dreel[3].sym==999){
            rel=int.Parse(spbmeter[6].text); txlmp[6].lampchange(true);
        }else{
            if((dreel[1].sym==20 || dreel[1].sym==999) && (dreel[2].sym==20 || dreel[2].sym==999) && (dreel[3].sym==20 || dreel[3].sym==999)){
                rel=int.Parse(spbmeter[5].text); txlmp[5].lampchange(true);
            }else if((dreel[1].sym==10 || dreel[1].sym==999) && (dreel[2].sym==10 || dreel[2].sym==999) && (dreel[3].sym==10 || dreel[3].sym==999)){
                rel=int.Parse(spbmeter[4].text); txlmp[4].lampchange(true);
            }else if((dreel[1].sym==10 || dreel[1].sym==20 || dreel[1].sym==999) && (dreel[2].sym==10 || dreel[2].sym==20 || dreel[2].sym==999) && (dreel[3].sym==10 || dreel[3].sym==20 || dreel[3].sym==999)){
                rel=int.Parse(spbmeter[3].text); txlmp[3].lampchange(true);
            }else if((dreel[1].sym==3 || dreel[1].sym==999) && (dreel[2].sym==3 || dreel[2].sym==999) && (dreel[3].sym==3 || dreel[3].sym==999)){
                rel=int.Parse(spbmeter[2].text); txlmp[2].lampchange(true);
            }else if((dreel[1].sym==2 || dreel[1].sym==999) && (dreel[2].sym==2 || dreel[2].sym==999) && (dreel[3].sym==2 || dreel[3].sym==999)){
                rel=int.Parse(spbmeter[1].text); txlmp[1].lampchange(true);
            }else if((dreel[1].sym==1 || dreel[1].sym==999) && (dreel[2].sym==1 || dreel[2].sym==999) && (dreel[3].sym==1 || dreel[3].sym==999)){
                rel=int.Parse(spbmeter[0].text); txlmp[0].lampchange(true);
            }
            for(int i=1;i<=3;i++){
                if(dreel[i].sym==999){
                    rel*=7;
                }
            }
        }
        if(rel>spbonuslimit){
            rel=spbonuslimit;
        }
        if(rel>0){
            mtc.wb();
        }
        return rel;
    }
}
