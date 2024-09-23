using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameMain : MonoBehaviour
{
    int mode,win,paid,totalBet,bigBet,betAnker,reelIndex,randNum,dgbet,collectable,tryCounter,saved,selectReel,specialBonusLimit;
    int centerWin,triggerGameWin,spinPays,gameWin,freeSpinWin,freeSpinRemaining,chanceRemaining; //spinPaysはフリーのペイアウト、gameWinはフリーのそのゲームの配当、freeSpinWinはフリー中の総配当
    float timer,intervalPayTime,waitTime;
    int[] lineBet=new int[8],linewin=new int[8],anywin=new int[8],symbolResult=new int[9],dsymp=new int[4]; //symbolResult[8]がセンターリール
    int[,] lineCheckIndex={{3,4,5},{0,1,2},{6,7,8},{0,4,8},{6,4,2},{0,3,6},{1,4,7},{2,5,8}};
    public int[] stopRoot={0,1,2,5,8,7,6,3,4},defaultStopRoot={0,1,2,5,8,7,6,3,4},specialBonusMultiplier={1,2,3,4,5,6,8,10,20,40,1500};
    public reelscr[] reels,dreel;
    public int playerMaxBet; //この値は最大ベット初期値であり、pub.maxBetでゲーム中の最大ベットを決める
    public float gameSpd=1.0f;
    public TextMeshProUGUI creditmeter,betmeter,winmeter,paidmeter,orgwin,clctmeter,ddbetmeter,trymeter,nxwinmeter,svdmeter,winLimitMeter;
    public TextMeshProUGUI[] lineBetMeter,lineWinMeter,wameter,specialBonusMeter,sel;
    public AudioClip[] SFX;
    AudioSource snd;
    public bgmMngr bgms;
    public GameObject indicatorMaster;
    bool[] lpchk=new bool[9];
    public lampctr[] windowlamp,txlmp;
    bool skipFlag;
    public Camera cam;
    public mtxctrlr mainTextControler;
    public FeatureBgmPlay ftBGM;
    public Sprite centerSuper,centerNormal;
    public enum FreeState{
        MAINGAME,TRIGGEREDNORMAL,FREESPIN,TRIGGEREDSUPER,SUPERFREE
    }
    public enum ChanceState{
        NORMAL,CHANCEMODE,HIGHCHANCE
    }
    public FreeState freeState;
    public ChanceState chanceState;

    void Start()
    {
        mode=0;
        bigBet=10;
        betAnker=0;
        saved=0;
        chanceRemaining=0;
        freeState=FreeState.MAINGAME;
        chanceState=ChanceState.NORMAL;
        snd=GetComponent<AudioSource>();
        timer=0.0f;
        paidmeter.text="POWER-ON";
        specialBonusLimit=10000000;
        playerMaxBet=pub.maxBet;
        Time.timeScale=1.0f;
    }

    void Update()
    {
        intervalPayTime+=Time.deltaTime;
        creditmeter.text=pub.credit.ToString();
        betmeter.text=totalBet.ToString();
        if(freeState==FreeState.MAINGAME || freeState==FreeState.TRIGGEREDNORMAL || freeState==FreeState.TRIGGEREDSUPER){
            if(win>0){
                winmeter.text=win.ToString();
            }else{
                winmeter.text="";
            }
        }
        if(Input.GetKeyDown(KeyCode.I)){
            pub.credit++;
            snd.PlayOneShot(SFX[0]);
            GameoverText();
        }
        if(Input.GetKeyDown(KeyCode.U)){
            pub.credit+=10;
            snd.PlayOneShot(SFX[0]);
            GameoverText();
        }
        if(Input.GetKeyDown(KeyCode.Y)){
            pub.credit+=1000;
            snd.PlayOneShot(SFX[0]);
            GameoverText();
        }
        if(Input.GetKeyDown(KeyCode.B) && pub.credit>0){ //1bet
            if(mode==0){
                BetReset();
                Bet();
                BetSound();
            }else if(mode==1 && totalBet<pub.maxBet){
                Bet();
                BetSound();
            }
        }
        if(Input.GetKeyDown(KeyCode.V)){ //bigBetの値だけベット
            if(mode<=1 && pub.credit>0){
                if(mode==0){
                    BetReset();
                }
                if(totalBet<=pub.maxBet-bigBet && pub.credit>bigBet-1){
                    pub.credit-=bigBet;
                    totalBet+=bigBet;
                    for(int i=0;i<lineBet.Length;i++){
                        lineBetMeter[i].text=totalBet.ToString();
                    }
                    BetSound();
                }else if(totalBet<pub.maxBet && pub.credit>0){
                    while(pub.credit>0 && totalBet!=pub.maxBet){
                        Bet();
                    }
                    BetSound();
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.M) && pub.credit>0 && (mode==0||(mode==1 && totalBet!=pub.maxBet))){ //maxbet
            if(mode==0){
                BetReset();
            }
            while(pub.credit>0 && totalBet!=pub.maxBet){
                Bet();
            }
            BetSound();
        }
        if(Input.GetKeyDown(KeyCode.Return)){ //gamestart
            if(mode==1){
                StartMainReel();
                snd.PlayOneShot(SFX[8]);
            }
            skipFlag=true;
        }
        if(Input.GetKeyDown(KeyCode.Space) && mode==0 && pub.credit>=totalBet && totalBet!=0 && freeState==FreeState.MAINGAME){ //repeat
            pub.credit-=totalBet;
            indicatorMaster.SetActive(true);
            cam.transform.position=new Vector3(0.0f,0.5f,-10.0f);
            BetSound();
            StartMainReel();
        }
        if(Input.GetKeyDown(KeyCode.Space) && (mode==4 || mode==5) && (pub.credit+win+collectable+saved)>=totalBet && totalBet!=0 && intervalPayTime>=0.3f && freeState==FreeState.MAINGAME){ //repeat
            mode=50;
            if(tryCounter==0){
                cam.transform.position=new Vector3(0.0f,0.5f,-10.0f);
                indicatorMaster.SetActive(true);
            }
            for(int i=0;i<9;i++){
                windowlamp[i].lampchange(false);
            }
            mainTextControler.paids();
            StartCoroutine(CoinCollect(true));
        }
        if(Input.GetKeyDown(KeyCode.C) && (mode==4 || mode==5) && freeState==FreeState.MAINGAME){ //collect
            mode=50;
            if(tryCounter==0){
                cam.transform.position=new Vector3(0.0f,0.5f,-10.0f);
                indicatorMaster.SetActive(true);
            }
            for(int i=0;i<9;i++){
                windowlamp[i].lampchange(false);
            }
            mainTextControler.paids();
            StartCoroutine(CoinCollect(false));
        }
        //ダブル進むか判定
        if((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.K))&& mode==4 && freeState==FreeState.MAINGAME){
            mode=5;
            DoubleStart();
        }else if((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.K))&& mode==5 && freeState==FreeState.MAINGAME){ //startdouble
            if(tryCounter==0){
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
            tryCounter++;
            trymeter.text=tryCounter.ToString();
            for(int i=0;i<specialBonusMultiplier.Length;i++){
                txlmp[i].lampchange(false);
                if(dgbet*specialBonusMultiplier[i]>specialBonusLimit){
                    specialBonusMeter[i].text=specialBonusLimit.ToString();
                }else{
                    specialBonusMeter[i].text=(dgbet*specialBonusMultiplier[i]).ToString();
                }
            }
            win=0;
        }
        //リール停止の処理
        if(mode==2 && freeState==FreeState.MAINGAME){
            timer+=Time.deltaTime;
            if(Input.GetKey(KeyCode.Return) && timer>=0.43f){
                mode=3;
                for(int i=reelIndex;i<reels.Length;i++){
                    reels[stopRoot[i]].ReelStop(symbolResult[i],false);
                }
            }
            if(timer>=0.95f){
                timer=0.53f;
                reels[stopRoot[reelIndex]].ReelStop(symbolResult[reelIndex],true);
                reelIndex++;
                if(reelIndex>=reels.Length){
                    mode=3;
                    timer=0.0f;
                }
            }
        }
        if(mode==2 && (freeState==FreeState.FREESPIN || freeState==FreeState.SUPERFREE)){
            timer+=Time.deltaTime;
            if(Input.GetKey(KeyCode.Return) && timer>=0.43f){
                mode=3;
                for(int i=reelIndex;i<reels.Length-2;i++){
                    reels[stopRoot[i]].ReelStop(symbolResult[i],false);
                }
                reels[stopRoot[7]].ReelStop(symbolResult[7],true);
            }
            if(timer>=0.95f){
                timer=0.53f;
                reels[stopRoot[reelIndex]].ReelStop(symbolResult[reelIndex],true);
                reelIndex++;
                if(reelIndex+1>=reels.Length){
                    mode=3;
                    timer=0.0f;
                }
            }
        }
        //リール全停止まで確認
        if(mode==3){
            if(reels[0].stopchk && reels[1].stopchk && reels[2].stopchk && reels[3].stopchk && reels[4].stopchk && reels[5].stopchk && reels[6].stopchk && reels[7].stopchk && reels[8].stopchk){
                bgms.StopBGM();
                Judge();
            }
        }
        //配当表示の処理
        if(mode==7){
            timer+=Time.deltaTime;
            if(timer>=0.03f){
                timer=0.0f;
                mode=4;
                mainTextControler.wnr();
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
        //当選後時間経過での処理
        if((mode==4 || mode==98) && freeState==FreeState.MAINGAME){
            timer+=Time.deltaTime;
            if(timer>=6.5f && timer<=10.0f){
                for(int i=0;i<9;i++){
                    windowlamp[i].lampchange(false);
                }
                if(mode==98){
                    mode=50;
                    mainTextControler.paids();
                    StartCoroutine(CoinCollect(false));
                }else{
                    timer=20.0f;
                    mainTextControler.pldd();
                }
            }
        }else if((mode==4 || mode==98) && (freeState==FreeState.FREESPIN || freeState==FreeState.SUPERFREE)){
            waitTime+=Time.deltaTime;
            winmeter.text=win.ToString();
            if(waitTime>1.8f){
                mode=30;
                for(int i=0;i<9;i++){
                    windowlamp[i].lampchange(false);
                }
                StartCoroutine(FreeWinCountUp());
            }
        }
        //フリー当選処理
        if((mode==4 || mode==98) && freeState==FreeState.TRIGGEREDNORMAL){
            timer+=Time.deltaTime;
            if(timer>=6.5f){
                timer=0.0f;
                for(int i=0;i<9;i++){
                    windowlamp[i].lampchange(false);
                }
                //mainTextControler.pldd();
                mode=8;
                pub.bonusRen=1;
                pub.bonusTotalWon=0;
                triggerGameWin=win;
                ftBGM.SoundStop();
                snd.PlayOneShot(SFX[9]);
                freeState=FreeState.FREESPIN;
            }
        }
        if((mode==4 || mode==98) && freeState==FreeState.TRIGGEREDSUPER){
            timer+=Time.deltaTime;
            if(timer>=6.5f){
                timer=0.0f;
                for(int i=0;i<9;i++){
                    windowlamp[i].lampchange(false);
                }
                //mainTextControler.pldd();
                mode=9;
                pub.bonusRen++;
                triggerGameWin=win;
                ftBGM.SoundStop();
                snd.PlayOneShot(SFX[9]);
                freeState=FreeState.SUPERFREE;
            }
        }
        if(mode==8){
            timer+=Time.deltaTime;
            if(timer>3.0f){
                timer=0.0f;
                StartCoroutine(FeatureSetUp(false,3));
            }
        }
        if(mode==9){
            timer+=Time.deltaTime;
            if(timer>3.0f){
                timer=0.0f;
                StartCoroutine(FeatureSetUp(true,3));
            }
        }
        //フリーの残りゲーム数チェックと継続の判定処理
        if(mode==20){
            freeSpinRemaining--;
            if(freeSpinRemaining>0){
                StartMainReel();
                mainTextControler.FeatureCount(freeSpinRemaining);
            }else{
                StartCoroutine(FeatureFinish());
            }
        }
        //以降、ダブルダウン処理
        if(mode==11){
            timer+=Time.deltaTime;
            if(timer>=1.05f){
                dsymp[0]=RotteryDealerSymbol();
                for(int i=1;i<=3;i++){
                    dsymp[i]=RotteryPlayerSymbol();
                }
                dreel[0].ReelStop(dsymp[0],true);
                timer=0.0f;
                mainTextControler.srl();
                mode=12;
            }
        }
        if(mode==12){
            if(Input.GetKeyDown(KeyCode.G)){
                selectReel=1;
                mode=13;
                snd.PlayOneShot(SFX[8]);
                sel[0].text="▲PLAYER▲";
                StartCoroutine("selectL");
            }else if(Input.GetKeyDown(KeyCode.H)){
                selectReel=2;
                mode=13;
                snd.PlayOneShot(SFX[8]);
                sel[1].text="▲PLAYER▲";
                StartCoroutine("selectC");
            }else if(Input.GetKeyDown(KeyCode.J)){
                selectReel=3;
                mode=13;
                snd.PlayOneShot(SFX[8]);
                sel[2].text="▲PLAYER▲";
                StartCoroutine("selectR");
            }
        }
        if(mode==13){
            if(dreel[1].stopchk && dreel[2].stopchk && dreel[3].stopchk){
                bgms.StopBGM();
                DoubleJudge();
            }
        }
    }

    IEnumerator CoinCollect(bool n){
        mode=50;
        skipFlag=false;
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
            if(skipFlag){
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
            snd.PlayOneShot(SFX[5]);
            paidmeter.text="PAID:"+paid.ToString();
        }
        if(n){
            yield return new WaitForSeconds(0.08f);
            pub.credit-=totalBet;
            indicatorMaster.SetActive(true);
            cam.transform.position=new Vector3(0.0f,0.5f,-10.0f);
            BetSound();
            StartMainReel();
        }else{
            mode=0;
            mainTextControler.maintx();
        }
    }
    IEnumerator FreeWinCountUp(){
        mode=30;
        waitTime=0.0f;
        skipFlag=false;
        spinPays=0;
        gameWin=win;
        for(int i=0;i<7;i++){
            txlmp[i].lampchange(false);
        }
        while(spinPays<gameWin){
            yield return new WaitForSeconds(0.08f);
            if(skipFlag){
                freeSpinWin+=gameWin-spinPays;
                spinPays+=gameWin-spinPays;
            }else if(spinPays>=970){
                if(gameWin-spinPays<63){
                    freeSpinWin+=gameWin-spinPays;
                    spinPays+=gameWin-spinPays;
                }else{
                    freeSpinWin+=63;
                    spinPays+=63;
                }
            }else if(spinPays>=390){
                if(gameWin-spinPays<29){
                    freeSpinWin+=gameWin-spinPays;
                    spinPays+=gameWin-spinPays;
                }else{
                    freeSpinWin+=29;
                    spinPays+=29;
                }
            }else if(spinPays>=130){
                if(gameWin-spinPays<13){
                    freeSpinWin+=gameWin-spinPays;
                    spinPays+=gameWin-spinPays;
                }else{
                    freeSpinWin+=13;
                    spinPays+=13;
                }
            }else if(spinPays>=60){
                if(gameWin-spinPays<7){
                    freeSpinWin+=gameWin-spinPays;
                    spinPays+=gameWin-spinPays;
                }else{
                    freeSpinWin+=7;
                    spinPays+=7;
                }
            }else if(spinPays>=30){
                if(gameWin-spinPays<3){
                    freeSpinWin+=gameWin-spinPays;
                    spinPays+=gameWin-spinPays;
                }else{
                    freeSpinWin+=3;
                    spinPays+=3;
                }
            }else{
                freeSpinWin++;
                spinPays++;
            }
            snd.PlayOneShot(SFX[5]);
            winmeter.text=(freeSpinWin+triggerGameWin).ToString();
        }
        yield return new WaitForSeconds(1.0f);
        mode=20;
    }
    IEnumerator selectL(){
        mainTextControler.gl();
        yield return new WaitForSeconds(0.3f);
        dreel[2].ReelStop(dsymp[2],true);
        yield return new WaitForSeconds(0.2f);
        dreel[3].ReelStop(dsymp[3],true);
        yield return new WaitForSeconds(0.5f);
        dreel[1].ReelStop(dsymp[1],true);
    }
    IEnumerator selectC(){
        mainTextControler.gl();
        yield return new WaitForSeconds(0.3f);
        dreel[1].ReelStop(dsymp[1],true);
        yield return new WaitForSeconds(0.2f);
        dreel[3].ReelStop(dsymp[3],true);
        yield return new WaitForSeconds(0.5f);
        dreel[2].ReelStop(dsymp[2],true);
    }
    IEnumerator selectR(){
        mainTextControler.gl();
        yield return new WaitForSeconds(0.3f);
        dreel[1].ReelStop(dsymp[1],true);
        yield return new WaitForSeconds(0.2f);
        dreel[2].ReelStop(dsymp[2],true);
        yield return new WaitForSeconds(0.5f);
        dreel[3].ReelStop(dsymp[3],true);
    }
    //ボーナスの開始と終了の処理
    IEnumerator FeatureSetUp(bool super,int game){
        freeSpinWin=0;
        yield return new WaitForSeconds(0.25f);
        ftBGM.FreeSoundPlay();
        mainTextControler.FeatureIndicate();
        for(int i=0;i<game;i++){
            freeSpinRemaining++;
            mainTextControler.FeatureCount(freeSpinRemaining);
            yield return new WaitForSeconds(0.25f);
        }
        if(super){
            StartMainReel();
        }else{
            StartMainReel();
        }
    }
    IEnumerator FeatureFinish(){
        mode=21;
        ftBGM.SoundStop();
        yield return new WaitForSeconds(0.1f);
        ftBGM.FreeFinish(0);
        mainTextControler.FeatureReset();
        yield return new WaitForSeconds(5.0f);
        win=(freeSpinWin+triggerGameWin);
        mode=7;
        pub.bonusTotalWon+=win;
        chanceRemaining=8;
        pub.maxBet=totalBet;
        mainTextControler.ChanceZoneCount(chanceRemaining);
        if(freeState==FreeState.FREESPIN){
            chanceState=ChanceState.CHANCEMODE;
        }else if(freeState==FreeState.SUPERFREE){
            chanceState=ChanceState.HIGHCHANCE;
        }
        freeState=FreeState.MAINGAME;
        ftBGM.ChanceSoundPlay();
    }
    IEnumerator ChanceFinish(){
        ftBGM.ChanceOver();
        yield return new WaitForSeconds(1.2f);
        chanceState=ChanceState.NORMAL;
        pub.maxBet=playerMaxBet;
        pub.bonusRen=0;
    }

    void GameoverText(){
        if(mode==0){
            if(pub.credit>0){
                mainTextControler.tlvc(1);
            }else{
                mainTextControler.tlvc(0);
            }
        }
    }
    void BetReset(){
        mode=1;
        totalBet=0;
        cam.transform.position=new Vector3(0.0f,0.5f,-10.0f);
        indicatorMaster.SetActive(true);
        mainTextControler.inscbs();
    }
    void Bet(){
        pub.credit--;
        totalBet++;
        for(int i=0;i<lineBet.Length;i++){
            lineBetMeter[i].text=totalBet.ToString();
        }
    }
    void BetSound(){
        if(totalBet==pub.maxBet){
            snd.PlayOneShot(SFX[2]);
        }else{
            snd.PlayOneShot(SFX[1]);
        }
    }
    //リール始動の諸処理
    void StartMainReel(){
        indicatorMaster.SetActive(true);
        mainTextControler.gl();
        for(int i=0;i<lineBet.Length;i++){
            lineWinMeter[i].text="";
            wameter[i].text="";
        }
        timer=0.0f;
        mode=2;
        reelIndex=0;
        win=0;
        paidmeter.text="";
        bgms.StopBGM();
        if(chanceState!=ChanceState.NORMAL && freeState==FreeState.MAINGAME){
            ftBGM.ChanceSoundPlay();
            chanceRemaining--;
        }
        //絵柄を変える
        GameObject[] wildSymbol=GameObject.FindGameObjectsWithTag("Changeable");
        foreach(GameObject ws in wildSymbol){
            SpriteRenderer sr=ws.GetComponent<SpriteRenderer>();
            if(chanceState!=ChanceState.NORMAL){
                sr.sprite=centerSuper;
            }else{
                sr.sprite=centerNormal;
            }
        }
        int effectJokerChance=0;
        for(int i=0;i<reels.Length;i++){
            lpchk[i]=false;
            stopRoot[i]=defaultStopRoot[i];
            symbolResult[i]=RotterySymbol(i);
            if(freeState==FreeState.MAINGAME){
                reels[i].ReelStart();
            }
            if((freeState==FreeState.FREESPIN || freeState==FreeState.SUPERFREE) && i!=4){
                reels[i].ReelStart();
            }
            if(symbolResult[i]==10){
                effectJokerChance++;
            }
        }
        if(effectJokerChance>0 && freeState==FreeState.MAINGAME){
            randNum=Random.Range(0,4+(effectJokerChance*2));
            if(randNum<(effectJokerChance*2)){
                for(int j=0;j<6;j++){
                    int k=stopRoot.Length-1;
                    while(k>0){
                        k--;
                        int f=Random.Range(0,k+1);
                        int y=stopRoot[f];
                        stopRoot[f]=stopRoot[k];
                        stopRoot[k]=y;
                    }
                }
            }
        }
        pub.gameSpeed=gameSpd;
        snd.PlayOneShot(SFX[3]);
    }
    int RotterySymbol(int reelNum){
        int a=0;
        randNum=Random.Range(0,60000);
        randNum=randNum%600;
        if(reelNum==8){
            for(int i=0;i<pub.centerReelThreshold.Length;i++){
                randNum-=pub.centerReelThreshold[i];
                if(randNum<=0){
                    a=i;
                    break;
                }
            }
            randNum=Random.Range(0,36000);
            if((randNum%159==10 && chanceState==ChanceState.NORMAL) || (randNum%12==10 && chanceState==ChanceState.CHANCEMODE) || (randNum%4==1 && chanceState==ChanceState.HIGHCHANCE)){
                a=10;
            }
        }else{
            if(freeState==FreeState.SUPERFREE){
                for(int i=0;i<pub.SuperReelThreshold.Length;i++){
                    randNum-=pub.SuperReelThreshold[i];
                    if(randNum<=0){
                        a=i;
                        break;
                    }
                }
            }else{
                for(int i=0;i<pub.mainReelThreshold.Length;i++){
                    randNum-=pub.mainReelThreshold[i];
                    if(randNum<=0){
                        a=i;
                        break;
                    }
                }
            }
        }
        return a;
    }
    int RotteryDealerSymbol(){
        int a=0;
        randNum=Random.Range(0,60000);
        randNum=randNum%600;
        for(int i=0;i<pub.doubleDealerThreshold.Length;i++){
            randNum-=pub.doubleDealerThreshold[i];
            if(randNum<=0){
                a=i;
                break;
            }
        }
        return a;
    }
    int RotteryPlayerSymbol(){
        int a=0;
        randNum=Random.Range(0,60000);
        randNum=randNum%600;
        for(int i=0;i<pub.doublePlayerThreshold.Length;i++){
            randNum-=pub.doublePlayerThreshold[i];
            if(randNum<=0){
                a=i;
                break;
            }
        }
        return a;
    }
    
    void Judge(){
        int[] anySymbolChecker={1,2,3,4,5,6,7,88};
        int counter,fruitsCheckLimit=7;
        bool[] blinkReel=new bool[9];
        //line判定
        for(int i=0;i<lineBet.Length;i++){
            if(reels[lineCheckIndex[i,0]].symbol==10 && reels[lineCheckIndex[i,1]].symbol==10 && reels[lineCheckIndex[i,2]].symbol==10){
                linewin[i]=totalBet*pub.wintableline[10];
            }else if(reels[lineCheckIndex[i,0]].sevenFlag && reels[lineCheckIndex[i,1]].sevenFlag && reels[lineCheckIndex[i,2]].sevenFlag){
                if((reels[lineCheckIndex[i,0]].symbol==9 || reels[lineCheckIndex[i,0]].symbol==10) && (reels[lineCheckIndex[i,1]].symbol==9 || reels[lineCheckIndex[i,1]].symbol==10) && (reels[lineCheckIndex[i,2]].symbol==9 || reels[lineCheckIndex[i,2]].symbol==10)){
                    linewin[i]=totalBet*pub.wintableline[9];
                }else if((reels[lineCheckIndex[i,0]].symbol==8 || reels[lineCheckIndex[i,0]].symbol==10) && (reels[lineCheckIndex[i,1]].symbol==8 || reels[lineCheckIndex[i,1]].symbol==10) && (reels[lineCheckIndex[i,2]].symbol==8 || reels[lineCheckIndex[i,2]].symbol==10)){
                    linewin[i]=totalBet*pub.wintableline[8];
                }else{
                    linewin[i]=totalBet*pub.wintableline[7];
                }
            }else{
                linewin[i]=0;
                //成立絵柄に応じた判定
                for(int j=1;j<=fruitsCheckLimit;j++){
                    if((reels[lineCheckIndex[i,0]].symbol==j || reels[lineCheckIndex[i,0]].symbol==10) && (reels[lineCheckIndex[i,1]].symbol==j || reels[lineCheckIndex[i,1]].symbol==10) && (reels[lineCheckIndex[i,2]].symbol==j || reels[lineCheckIndex[i,2]].symbol==10)){
                        linewin[i]=totalBet*pub.wintableline[j-1];
                    }
                }
            }
            if(linewin[i]>0){
                for(int j=0;j<3;j++){
                    lpchk[lineCheckIndex[i,j]]=true;
                }
            }
        }
        //any判定
        for(int i=0;i<anySymbolChecker.Length;i++){
            counter=0;
            for(int j=0;j<reels.Length;j++){
                if(anySymbolChecker[i]==88){
                    if(reels[j].sevenFlag){
                        counter++;
                        blinkReel[j]=true;
                    }
                }else{
                    if(reels[j].symbol==anySymbolChecker[i]){
                        counter++;
                        if(anySymbolChecker[i]!=0){
                            blinkReel[j]=true;
                        }
                    }else if(reels[j].symbol==10){
                        counter++;
                        blinkReel[j]=true;
                    }
                }
            }
            anywin[i]=totalBet*pub.wintableAny[i,counter];
            if(anywin[i]>0){
                for(int l=0;l<9;l++){
                    if(blinkReel[l]){
                        lpchk[l]=blinkReel[l];
                        blinkReel[l]=false;
                    }
                }
            }else{
                for(int l=0;l<9;l++){
                    blinkReel[l]=false;
                }
            }
        }
        if(reels[4].symbol==10){
            centerWin=totalBet;
            lpchk[4]=true;
        }else{
            centerWin=0;
        }
        Calc();
    }
    //配当計算
    void Calc(){
        Time.timeScale=1.0f;
        win+=centerWin;
        for(int i=0;i<linewin.Length;i++){
            win+=linewin[i];
            if(linewin[i]>0){
                lineWinMeter[i].text=linewin[i].ToString();
            }
        }
        for(int i=0;i<anywin.Length;i++){
            win+=anywin[i];
            if(anywin[i]>0){
                wameter[i].text=anywin[i].ToString();
            }
        }
        if(win>totalBet*50000){
            win=totalBet*50000;
        }
        if(centerWin>0){
            if(freeState==FreeState.MAINGAME && chanceState==ChanceState.NORMAL){
                freeState=FreeState.TRIGGEREDNORMAL;
            }else if(freeState==FreeState.MAINGAME && chanceState!=ChanceState.NORMAL){
                freeState=FreeState.TRIGGEREDSUPER;
            }
        }else{
            mainTextControler.ChanceZoneCount(chanceRemaining);
            if(chanceRemaining<=0 && freeState==FreeState.MAINGAME){
                StartCoroutine(ChanceFinish());
            } 
        }
        if(win>0){
            timer=0.0f;
            intervalPayTime=0.0f;
            mode=7;
        }else{
            mode=0;
            mainTextControler.maintx();
        }
    }
    //ダブル遷移処理
    void DoubleStart(){
        bgms.StopBGM();
        for(int i=0;i<9;i++){
            windowlamp[i].lampchange(false);
        }
        indicatorMaster.SetActive(false);
        mainTextControler.pldd();
        tryCounter=0;
        saved=0;
        collectable=win;
        orgwin.text=win.ToString();
        clctmeter.text=collectable.ToString();
        ddbetmeter.text="0";
        nxwinmeter.text=(win*2).ToString();
        svdmeter.text="0";
        trymeter.text="1";
        //初期テキスト表示
        for(int i=0;i<specialBonusMeter.Length;i++){
            specialBonusMeter[i].text="BET x"+specialBonusMultiplier[i].ToString();
        }
        cam.transform.position=new Vector3(10.0f,0.5f,-10.0f);
        snd.PlayOneShot(SFX[4]);
    }
    void startdreel(){
        timer=0.0f;
        mode=11;
        bgms.StopBGM();
        mainTextControler.gl();
        for(int i=0;i<dreel.Length;i++){
            if(i==0){
                dsymp[i]=RotteryDealerSymbol();
            }else{
                dsymp[i]=RotteryPlayerSymbol();
            }
            dreel[i].ReelStart();
        }
        for(int j=0;j<3;j++){
            sel[j].text="";
        }
        snd.PlayOneShot(SFX[3]);
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
    void DoubleJudge(){
        Time.timeScale=1.0f;
        if(dreel[0].symbol<dreel[selectReel].symbol){
            DoubleWin(selectReel);
        }else if(dreel[0].symbol==dreel[selectReel].symbol){
            DoublePush(selectReel);
        }else{
            collectable=0;
        }
        collectable+=checkSpecialBonus();
        if(collectable>dgbet){
            mode=5;
            winsound(collectable);
            clctmeter.text=collectable.ToString();
            if(mode==5){
                nxwinmeter.text=(collectable*2).ToString();
            }
        }else if(collectable==dgbet){
            mode=5;
            if(dreel[0].symbol==dreel[selectReel].symbol){
                snd.PlayOneShot(SFX[6]);
                mainTextControler.ps();
            }else{
                winsound(collectable);
                mainTextControler.lb();
            }
            clctmeter.text=collectable.ToString();
        }else{
            if(saved>0){
                mainTextControler.svdw();
                StartCoroutine(CoinCollect(false));
            }else{
                mode=0;
                mainTextControler.maintx();
            }
        }
    }
    void DoubleWin(int p){
        collectable*=2;
        mode=5;
        mainTextControler.DoubleWinText();
    }
    void DoublePush(int p){
        mode=5;
    }
    int checkSpecialBonus(){
        int rel=0;
        if(dreel[1].symbol==10 && dreel[2].symbol==10 && dreel[3].symbol==10){
            rel=int.Parse(specialBonusMeter[10].text); txlmp[10].lampchange(true);
        }else{
            if((dreel[1].symbol==9 || dreel[1].symbol==10) && (dreel[2].symbol==9 || dreel[2].symbol==10) && (dreel[3].symbol==9 || dreel[3].symbol==10)){
                rel=int.Parse(specialBonusMeter[9].text); txlmp[9].lampchange(true);
            }else if((dreel[1].symbol==8 || dreel[1].symbol==10) && (dreel[2].symbol==8 || dreel[2].symbol==10) && (dreel[3].symbol==8 || dreel[3].symbol==10)){
                rel=int.Parse(specialBonusMeter[8].text); txlmp[8].lampchange(true);
            }else if((dreel[1].symbol==8 || dreel[1].symbol==9 || dreel[1].symbol==10) && (dreel[2].symbol==8 || dreel[2].symbol==9 || dreel[2].symbol==10) && (dreel[3].symbol==8 || dreel[3].symbol==9 || dreel[3].symbol==10)){
                rel=int.Parse(specialBonusMeter[7].text); txlmp[7].lampchange(true);
            }else{
                //symbol=7から数えたいので
                for(int i=7;i>0;i--){
                    if((dreel[1].symbol==i || dreel[1].symbol==10) && (dreel[2].symbol==i || dreel[2].symbol==10) && (dreel[3].symbol==i || dreel[3].symbol==10)){
                        rel=int.Parse(specialBonusMeter[i-1].text); txlmp[i-1].lampchange(true);
                        break;
                    }
                }
            }
        }
        if(rel>specialBonusLimit){
            rel=specialBonusLimit;
        }
        if(rel>0){
            mainTextControler.wb();
        }
        return rel;
    }
}
