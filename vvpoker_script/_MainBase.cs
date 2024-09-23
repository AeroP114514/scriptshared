using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class _MainBase : MonoBehaviour
{
    public int[] coinLevel;
    public int totalBet,betPerHand,betMultiplier,playHand,cardMax,deckIndex,predrawMaxHand;
    public int originalWin,collectableWin,doubleWager,nextWin,tryLuck,savedWin,doubleLimit,bonusLimit;
    public GameObject[] cardObject=new GameObject[5],doubleParts=new GameObject[3];
    public GameObject introduct,mainTextUnit,doubleGameUnit,barObj;
    public bool[] holdCheck=new bool[5];
    public bool cardFlash,speedOpen;
    public string[] sysType=new string[2];
    public TextMeshProUGUI guideText,crdmeter,betmeter,winmeter,paidmeter,dModeText,doubleIndicator,spBonusMeter,collectmeter,extraText;
    public TextMeshProUGUI[] holdmeter,winPerHandMeter,handNameMeter,doubleUnderCardMeter;
    public deckScr deckScr;
    public signcontroler dealerCont;
    public winSound winsound;
    public AudioSource adSrc;
    public AudioClip[] betSound,systemSound; 
    public class handData{
        public int[] rank=new int[5];
        public int[] suit=new int[5];
        public bool[] hitCard=new bool[5];
        public int handFlag;
        public void CardLampOn(int num){
            for(int i=0;i<hitCard.Length;i++){
                if(hitCard[i]){
                    GameObject.Find("card"+(i+1).ToString()+" ("+num.ToString()+")").GetComponent<SpriteRenderer>().color=new Color(1.0f,0.8f,1.0f,1.0f);
                }
            }
        }
    }
    public enum State{
        IDLE=0,
        BETACCEPT=1,
        ISDEAL=2,
        HOLDTIME=4,
        WINNING=8,
        INTRODUCTION=16,
        INDOUBLE=32
    }
    public enum DoubleStyle{
        STANDARD,HIGHLOW,REDBLACK
    }
    public handData[] hand=new handData[25];
    public State gameState;
    public DoubleStyle doublemode;
    public TextAsset csv;
    public List<string[]> Datas=new List<string[]>();
    
    //メダル投入処理
    public void CoinIn(){
        if(Input.GetKeyDown(KeyCode.I)){
            pubNum.credit+=coinLevel[0];
            adSrc.PlayOneShot(systemSound[7]);
        }
        if(Input.GetKeyDown(KeyCode.O)){
            pubNum.credit+=coinLevel[1];
            adSrc.PlayOneShot(systemSound[7]);
        }
    }
    //テキスト表示処理
    public void TextIndicate(){
        crdmeter.text=pubNum.credit.ToString();
        betmeter.text=totalBet.ToString();
        for(int i=0;i<holdCheck.Length;i++){
            holdmeter[i].text=holdCheck[i] ? "<color=#00FFFF>HELD</color>" : "";
        }
    }
    //ベット処理
    public void BetAction(){
        if(gameState.HasFlag(State.BETACCEPT) && !gameState.HasFlag(State.INTRODUCTION)){
            if(Input.GetKeyDown(KeyCode.F)){
                playHand=1;
                sysType[0]="1 hand ";
                adSrc.PlayOneShot(betSound[0]);
            }
            if(Input.GetKeyDown(KeyCode.G)){
                playHand=4;
                sysType[0]="4 hands ";
                adSrc.PlayOneShot(betSound[1]);
            }
            if(Input.GetKeyDown(KeyCode.H)){
                playHand=7;
                sysType[0]="7 hands ";
                adSrc.PlayOneShot(betSound[2]);
            }
            if(Input.GetKeyDown(KeyCode.J)){
                playHand=10;
                sysType[0]="10 hands ";
                adSrc.PlayOneShot(betSound[3]);
            }
            if(Input.GetKeyDown(KeyCode.K)){
                playHand=25;
                sysType[0]="25 hands ";
                adSrc.PlayOneShot(betSound[4]);
            }
            if(Input.GetKeyDown(KeyCode.UpArrow) && betMultiplier<9){
                betMultiplier++;
            }
            if(Input.GetKeyDown(KeyCode.DownArrow) && betMultiplier>1){
                betMultiplier--;
            }
            if(Input.GetKeyDown(KeyCode.RightArrow) && betPerHand<4){
                betPerHand++;
                if(betPerHand==1){
                    sysType[1]="1 Coin ";
                }else{
                    sysType[1]=betPerHand.ToString()+" Coins ";
                }
            }
            if(Input.GetKeyDown(KeyCode.LeftArrow) && betPerHand>1){
                betPerHand--;
                if(betPerHand==1){
                    sysType[1]="1 Coin ";
                }else{
                    sysType[1]=betPerHand.ToString()+" Coins ";
                }
            }
            guideText.text="Play <color=#FFFF00>"+sysType[0]+sysType[1]+"Bet Per Hand.</color> Bet Multiplier = <color=#00FF00>"+betMultiplier.ToString()+"</color>";
            totalBet=betPerHand*playHand*betMultiplier;
        }
    }
    //配当表など表示
    public void IndiIntro(){
        if(Input.GetKeyDown(KeyCode.Q) && gameState.HasFlag(State.BETACCEPT)){
            if(gameState.HasFlag(State.INTRODUCTION)){
                gameState &= ~State.INTRODUCTION;
                introduct.SetActive(false);
                adSrc.PlayOneShot(systemSound[4]);
            }else{
                gameState |= State.INTRODUCTION;
                introduct.SetActive(true);
                adSrc.PlayOneShot(systemSound[3]);
            }
        }
    }
    //カードのドロー及び、スピードオープン設定
    public void Deal(){
        //speed open
        if(Input.GetKeyDown(KeyCode.Space) && gameState.HasFlag(State.ISDEAL)){
            speedOpen=true;
        }
        //最初のドロー
        if(Input.GetKeyDown(KeyCode.Space) && gameState.HasFlag(State.BETACCEPT) && !gameState.HasFlag(State.INTRODUCTION) && pubNum.credit>=totalBet){
            gameState &= ~State.BETACCEPT;
            gameState |= State.ISDEAL;
            pubNum.credit-=totalBet;
            winsound.adSrc.Stop();
            dealerCont.FaceChanger(0);
            deckScr.DeckShuffle();
            ReverseCard();
            adSrc.PlayOneShot(betSound[5]);
            StartCoroutine(FirstDeal());
        }
        //交換後のドロー
        if(Input.GetKeyDown(KeyCode.Space) && gameState.HasFlag(State.HOLDTIME) && !gameState.HasFlag(State.ISDEAL)){
            gameState &= ~State.HOLDTIME;
            gameState |= State.ISDEAL;
            adSrc.PlayOneShot(systemSound[14]);
            StartCoroutine(SecondDeal());
        }
    }
    //リピートベット
    public void RepeatDeal(){
        if(Input.GetKeyDown(KeyCode.Space) && gameState.HasFlag(State.WINNING) && pubNum.credit+pubNum.win>=totalBet){
            gameState &= ~State.WINNING;
            gameState &= ~State.INDOUBLE;
            winsound.adSrc.Stop();
            gameState |= State.ISDEAL;
            pubNum.win+=savedWin;
            pubNum.credit+=pubNum.win;
            pubNum.credit-=totalBet;
            dealerCont.FaceChanger(0);
            deckScr.DeckShuffle();
            ReverseCard();
            adSrc.PlayOneShot(betSound[5]);
            StartCoroutine(FirstDeal());
        }
    }
    //カードを裏向きにする
    public void ReverseCard(){
        for(int i=0;i<cardMax;i++){
            holdCheck[i]=false;
            cardObject[i].GetComponent<cardScript>().DefaultCard();
            GameObject[] objs=GameObject.FindGameObjectsWithTag("card"+(i+1).ToString());
            foreach(GameObject cardObj in objs){
                cardObj.GetComponent<cardScript>().DefaultCard();
                cardObj.GetComponent<SpriteRenderer>().color=new Color(1.0f,1.0f,1.0f,1.0f);
            }
        }
    }
    //カードのホールド
    public void HoldAction(){
        if(Input.GetKeyDown(KeyCode.F)){
            HoldCopy(0);
        }
        if(Input.GetKeyDown(KeyCode.G)){
            HoldCopy(1);
        }
        if(Input.GetKeyDown(KeyCode.H)){
            HoldCopy(2);
        }
        if(Input.GetKeyDown(KeyCode.J)){
            HoldCopy(3);
        }
        if(Input.GetKeyDown(KeyCode.K)){
            HoldCopy(4);
        }
    }
    //ホールドするカードをコピーする
    public void HoldCopy(int n){
        holdCheck[n]=!holdCheck[n];
        adSrc.PlayOneShot(systemSound[2]);
        if(playHand==25){
            GameObject[] objs=GameObject.FindGameObjectsWithTag("card"+(n+1).ToString());
            foreach(GameObject cardObj in objs){
                if(holdCheck[n]){
                    cardObj.GetComponent<cardScript>().index=deckScr.deck[n];
                    cardObj.GetComponent<cardScript>().CardIndicate();
                }else{
                    cardObj.GetComponent<cardScript>().DefaultCard();
                }
            }
        }else if(playHand>1){
            for(int i=1;i<playHand;i++){
                GameObject obj=GameObject.Find("card"+(n+1).ToString()+" ("+i.ToString()+")");
                if(holdCheck[n]){
                    obj.GetComponent<cardScript>().index=deckScr.deck[n];
                    obj.GetComponent<cardScript>().CardIndicate();
                }else{
                    obj.GetComponent<cardScript>().DefaultCard();
                }
            }
        }
    }
    //カードのランクを返す変数、decRank+2のランクを示し、12はエース、13はジョーカー
    public int RankSearcher(int cn){
        int decRank=0;
        while(cn>=0){
            cn-=4;
            if(cn<0){
                break;
            }
            decRank++;
        }
        return decRank;
    }
    //カードのスートを返す変数、0から順にc,d,h,s、4はジョーカー
    public int SuitSearcher(int cn){
        int decSuit;
        //カード番号52以上はジョーカーのため
        if(cn>=52){
            decSuit=4;
        }else{
            decSuit=cn%4;
        }
        return decSuit;
    }
    //カードを点滅させるアニメーションを起動するかの処理
    public void HitCardFlush(){
        if(cardFlash){
            for(int i=0;i<cardObject.Length;i++){
                if(hand[0].hitCard[i]){
                    if(holdCheck[i]){
                        cardObject[i].GetComponent<Animator>().SetBool("hitcard",false);
                    }else{
                        cardObject[i].GetComponent<Animator>().SetBool("hitcard",true);
                    }
                }
            }
        }
    }
    //コレクト
    public void Collect(){
        if(Input.GetKeyDown(KeyCode.C) && gameState.HasFlag(State.WINNING)){
            pubNum.win+=savedWin;
            pubNum.credit+=pubNum.win;
            winmeter.text=pubNum.win.ToString();
            paidmeter.text="PAID : "+pubNum.win.ToString();
            gameState &= ~State.WINNING;
            gameState &= ~State.INDOUBLE;
            gameState |= State.BETACCEPT;
            doubleGameUnit.SetActive(false);
            mainTextUnit.SetActive(true);
        }
        if(pubNum.win>doubleLimit && gameState.HasFlag(State.WINNING)){
            pubNum.win+=savedWin;
            pubNum.credit+=pubNum.win;
            winmeter.text=pubNum.win.ToString();
            paidmeter.text="PAID : "+pubNum.win.ToString();
            gameState &= ~State.WINNING;
            gameState &= ~State.INDOUBLE;
            gameState |= State.BETACCEPT;
            collectmeter.text="\n"+"COLLECTED";
        }
    }
    //ダブル時のテキストを表示
    public void DoubleTextIndi(){
        doubleIndicator.text=originalWin.ToString()+"\n"+collectableWin.ToString()+"\n"+doubleWager.ToString()+"\n"+nextWin.ToString()+"\n"+tryLuck.ToString()+"\n"+savedWin.ToString();
    }
    //winsound
    public void WinSoundPlay(int md){
        //md=0 -> メインゲーム、1 -> ダブル
        if(md==0){
            if(pubNum.win>=10*totalBet){
                winsound.SoundPlay(4);
            }else if(pubNum.win>=5*totalBet){
                winsound.SoundPlay(3);
            }else if(pubNum.win>=2*totalBet){
                winsound.SoundPlay(2);
            }else{
                winsound.SoundPlay(1);
            }
        }else if(md==1){
            if(pubNum.win>=100*totalBet){
                winsound.SoundPlay(4);
            }else if(pubNum.win>=20*totalBet){
                winsound.SoundPlay(3);
            }else if(pubNum.win>=5*totalBet){
                winsound.SoundPlay(2);
            }else{
                winsound.SoundPlay(1);
            }
        }
    }
    //ダブル種別設定
    public void DoubleSelect(){
        if(gameState.HasFlag(State.WINNING) && gameState.HasFlag(State.INDOUBLE)){
            //スタンダード
            if(Input.GetKeyDown(KeyCode.G)){
                doublemode=DoubleStyle.STANDARD;
                dModeText.text="-STANDARD DOUBLE-";
                ChangeMode(0);
            }
            //ハイロー
            if(Input.GetKeyDown(KeyCode.H)){
                doublemode=DoubleStyle.HIGHLOW;
                ChangeMode(1);
                dModeText.text="-HIGH&LOW DOUBLE-";
            }
            //レッドブラック
            if(Input.GetKeyDown(KeyCode.J)){
                doublemode=DoubleStyle.REDBLACK;
                ChangeMode(2);
                dModeText.text="-RED&BLACK DOUBLE-";
            }
        }
    }
    //ダブルゲーム表示切替処理
    public void ChangeMode(int n){
        for(int i=0;i<doubleParts.Length;i++){
            doubleParts[i].SetActive(false);
        }
        doubleParts[n].SetActive(true);
    }
    //設定したダブルダウンのコルーチンを開始するための処理
    public void StartDoubleDown(){
        switch(doublemode){
            case DoubleStyle.STANDARD: StartCoroutine(StandardDouble()); break;
            case DoubleStyle.HIGHLOW: StartCoroutine(HiLoDouble()); break;
            case DoubleStyle.REDBLACK: StartCoroutine(RedBlackDouble()); break;
            default: break;
        }
    }
    //役の判定、ジョーカー非考慮
    public virtual int Judge(int handNum){
        //上書きする
        return 0;
    }
    //spボーナスのテキストを表示
    public virtual void SpecialBonusIndi(){
        //上書きする
    }
    //ダブルダウン
    public virtual void DoubleGo(){
        //上書きする
    }
    //配当の計算と表示
    public virtual void Calc(){
        //上書きする
    }
    //各ハンドの成立役を表示
    public virtual void HandNameIndi(int n){
        //上書きする
    }
    //各ハンドの配当と成立役表示
    public virtual void HandWin(int n){
        //上書きする
    }
    //spボーナス配当の計算
    public virtual void SpecialBonusCalc(int mpA,int mpB){
        //上書きする
    }
    //spボーナスのデフォルトテキストを表示
    public virtual void SpecialBonusDefault(){
        //上書きする
    }

    //最初のカードを配る処理
    public IEnumerator FirstDeal(){
        doubleGameUnit.SetActive(false);
        mainTextUnit.SetActive(true);
        pubNum.win=0;
        savedWin=0;
        winmeter.text="0";
        paidmeter.text="";
        guideText.text="Good Luck!";
        deckIndex=0;
        for(int i=0;i<hand.Length;i++){
            hand[i]=new handData();
            winPerHandMeter[i].text="";
            handNameMeter[i].text="";
        }
        yield return new WaitForSeconds(0.1f);
        gameState |= State.HOLDTIME;
        for(int i=0;i<cardObject.Length;i++){
            cardScript cScr=cardObject[i].GetComponent<cardScript>();
            cScr.index=deckScr.deck[deckIndex];
            hand[0].rank[i]=RankSearcher(cScr.index);
            hand[0].suit[i]=SuitSearcher(cScr.index);
            cScr.CardAnim();
            deckIndex++;
            adSrc.PlayOneShot(systemSound[0]);
            yield return new WaitForSeconds(0.15f);
        }
        hand[0].handFlag=Judge(0);
        predrawMaxHand=hand[0].handFlag;
        yield return new WaitForSeconds(0.16f);
        if(Judge(0)>0){
            HandNameIndi(0);
            cardFlash=true;
            adSrc.PlayOneShot(systemSound[17]);
        }
        gameState &= ~State.ISDEAL;
        guideText.text="Hold and Draw.";
    }
    //次のカードを配る処理、各カードのホールドも確定させる
    public IEnumerator SecondDeal(){
        speedOpen=false;
        cardFlash=false;
        handNameMeter[0].text="";
        guideText.text="Good Luck!";
        for(int i=0;i<cardObject.Length;i++){
            cardObject[i].GetComponent<Animator>().SetBool("hitcard",false);
            if(!holdCheck[i]){
                cardObject[i].GetComponent<cardScript>().DefaultCard();
            }
        }
        yield return new WaitForSeconds(0.05f);
        for(int i=0;i<cardObject.Length;i++){
            if(!holdCheck[i]){
                cardScript cScr=cardObject[i].GetComponent<cardScript>();
                cScr.index=deckScr.deck[deckIndex];
                hand[0].rank[i]=RankSearcher(cScr.index);
                hand[0].suit[i]=SuitSearcher(cScr.index);
                cScr.CardAnim();
                deckIndex++;
                adSrc.PlayOneShot(systemSound[0]);
                yield return new WaitForSeconds(0.15f);
            }
        }
        hand[0].handFlag=Judge(0);
        if(hand[0].handFlag>0){
            yield return new WaitForSeconds(0.1f);
            HandNameIndi(0);
            if(hand[0].handFlag>6){
                adSrc.PlayOneShot(systemSound[10]);
            }else if(hand[0].handFlag>3){
                adSrc.PlayOneShot(systemSound[9]);
            }else{
                adSrc.PlayOneShot(systemSound[8]);
            }
            yield return new WaitForSeconds(0.4f);
        }
        for(int h=1;h<playHand;h++){
            deckScr.ShuffleSecondDeck();
            for(int i=0;i<cardObject.Length;i++){
                GameObject obj=GameObject.Find("card"+(i+1).ToString()+" ("+h.ToString()+")");
                cardScript cScr=obj.GetComponent<cardScript>();
                if(!holdCheck[i]){
                    cScr.index=deckScr.nextDeck[i];
                    hand[h].rank[i]=RankSearcher(cScr.index);
                    hand[h].suit[i]=SuitSearcher(cScr.index);
                    cScr.CardIndicate();
                    adSrc.PlayOneShot(systemSound[1]);
                    if(speedOpen){
                        yield return new WaitForSeconds(0.016f);
                    }else{
                        yield return new WaitForSeconds(0.1f);
                    }
                }else{
                    hand[h].rank[i]=RankSearcher(cScr.index);
                    hand[h].suit[i]=SuitSearcher(cScr.index);
                }
            }
            hand[h].handFlag=Judge(h);
            if(hand[h].handFlag>0){
                HandNameIndi(h);
                hand[h].CardLampOn(h);
                if(hand[h].handFlag>predrawMaxHand){
                    if(hand[h].handFlag>6){
                        adSrc.PlayOneShot(systemSound[10]);
                    }else if(hand[h].handFlag>3){
                        adSrc.PlayOneShot(systemSound[9]);
                    }else{
                        adSrc.PlayOneShot(systemSound[8]);
                    }
                    yield return new WaitForSeconds(0.5f);
                }
            }
        }
        yield return new WaitForSeconds(0.1f);
        Calc();
        gameState &= ~State.ISDEAL;
        if(pubNum.win>0){
            gameState |= State.WINNING;
            guideText.text="Play Double Down or Collect.";
            if(pubNum.win>=20*totalBet){
                dealerCont.FaceChanger(5);
            }else if(pubNum.win>=5*totalBet){
                dealerCont.FaceChanger(4);
            }else if(pubNum.win>=1*totalBet){
                dealerCont.FaceChanger(3);
            }else{
                dealerCont.FaceChanger(2);
            }
            WinSoundPlay(0);
        }else{
            gameState |= State.BETACCEPT;
            dealerCont.FaceChanger(6);
        }
    }
    //スタンダードダブル開始
    public IEnumerator StandardDouble(){
        guideText.text="Good Luck!";
        int selectedCardNum=0;
        for(int i=0;i<doubleUnderCardMeter.Length;i++){
            doubleUnderCardMeter[i].text="";
        }
        for(int i=0;i<cardObject.Length;i++){
            cardObject[i].GetComponent<cardScript>().DefaultCard();
            adSrc.PlayOneShot(systemSound[0]);
            yield return new WaitForSeconds(0.1f);
        }
        //メインの処理
        cardScript cScr=cardObject[0].GetComponent<cardScript>();
        cScr.index=deckScr.deck[0];
        hand[0].rank[0]=RankSearcher(cScr.index);
        adSrc.PlayOneShot(systemSound[0]);
        doubleUnderCardMeter[0].text="<color=#FFAA00>DEALER</color>";
        cScr.CardAnim();
        yield return new WaitForSeconds(0.3f);
        guideText.text="Which is Higher Than Dealer's?";
        if(hand[0].rank[0]>10){
            dealerCont.FaceChanger(12);
        }else if(hand[0].rank[0]>8){
            dealerCont.FaceChanger(10);
        }else if(hand[0].rank[0]<3){
            dealerCont.FaceChanger(8);
        }
        //G~Kのどれかを入力されるまで待機
        yield return new WaitUntil(() => (Input.GetKeyDown(KeyCode.G) || Input.GetKeyDown(KeyCode.H) || Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.K)));
        adSrc.PlayOneShot(systemSound[2]);
        guideText.text="Good Luck!";
        if(Input.GetKey(KeyCode.G)){
            selectedCardNum=1;
        }else if(Input.GetKey(KeyCode.H)){
            selectedCardNum=2;
        }else if(Input.GetKey(KeyCode.J)){
            selectedCardNum=3;
        }else if(Input.GetKey(KeyCode.K)){
            selectedCardNum=4;
        }
        doubleUnderCardMeter[selectedCardNum].text="<color=#00FF33>PLAYER</color>";
        //選んだカード以外表示
        for(int i=1;i<cardObject.Length;i++){
            if(i!=selectedCardNum){
                cScr=cardObject[i].GetComponent<cardScript>();
                cScr.index=deckScr.deck[i];
                adSrc.PlayOneShot(systemSound[0]);
                cScr.CardAnim();
                yield return new WaitForSeconds(0.15f);
            }
        }
        yield return new WaitForSeconds(0.3f);
        cScr=cardObject[selectedCardNum].GetComponent<cardScript>();
        cScr.index=deckScr.deck[selectedCardNum];
        hand[0].rank[selectedCardNum]=RankSearcher(cScr.index);
        adSrc.PlayOneShot(systemSound[0]);
        cScr.DoubleCardAnim();
        yield return new WaitForSeconds(0.5f);
        //判定
        if(hand[0].rank[selectedCardNum]>hand[0].rank[0]){
            guideText.text="You Win!";
            collectableWin=doubleWager*2;
            //9...rankがJack以上の時は勝った時に一緒に喜ぶ
            if(hand[0].rank[0]>=9){
                dealerCont.JumpingAnim();
            }
            pubNum.win=collectableWin;
            WinSoundPlay(1);
            nextWin=collectableWin*2;
            DoubleTextIndi();
            gameState &= ~State.ISDEAL;
            gameState |= State.WINNING;
            dealerCont.FaceChanger(9);
        }else if(hand[0].rank[selectedCardNum]<hand[0].rank[0]){
            pubNum.win=0;
            pubNum.win+=savedWin;
            if(pubNum.win>0){
                winmeter.text=pubNum.win.ToString();
                paidmeter.text="PAID : "+pubNum.win.ToString();
                pubNum.credit+=pubNum.win;
            }
            winsound.SoundPlay(0);
            gameState &= ~State.ISDEAL;
            gameState &= ~State.INDOUBLE;
            gameState |= State.BETACCEPT;
            dealerCont.FaceChanger(7);
            collectmeter.text="\n"+"COLLECTED";
        }else{
            guideText.text="Double Down or Collect.";
            adSrc.PlayOneShot(systemSound[16]);
            collectableWin=doubleWager;
            hand[0].handFlag=Judge(0);
            pubNum.win=collectableWin;
            nextWin=collectableWin*2;
            DoubleTextIndi();
            gameState &= ~State.ISDEAL;
            gameState |= State.WINNING;
            dealerCont.FaceChanger(6);
        }
        winmeter.text=pubNum.win.ToString();
    }
    //ハイローダブル開始
    public IEnumerator HiLoDouble(){
        guideText.text="Good Luck!";
        SpecialBonusIndi();
        int passCount=0;
        int extra=2;
        extraText.text="";
        GameObject[] objs=GameObject.FindGameObjectsWithTag("spbar");
        foreach(GameObject barObj in objs){
            barObj.GetComponent<Animator>().SetBool("flag",false);
        }
        for(int i=0;i<doubleUnderCardMeter.Length;i++){
            doubleUnderCardMeter[i].text="";
            cardObject[i].GetComponent<cardScript>().BlankCard();
        }
        for(int i=0;i<cardObject.Length;i++){
            if(i<2){
                cardObject[i].GetComponent<cardScript>().DefaultCard();
                adSrc.PlayOneShot(systemSound[0]);
                yield return new WaitForSeconds(0.1f);
            }else{
                cardObject[i].GetComponent<cardScript>().BlankCard();
            }
        }
        yield return new WaitForSeconds(0.1f);
        //メインの処理
        for(int i=0;i<cardObject.Length;i++){
            cardObject[i].GetComponent<cardScript>().DefaultCard();
            if(i!=0){
                if(i==4){
                    switch(hand[0].rank[i-1]){
                        case 8: extraText.text="HIT With <color=#00FFFF>A,K,Q or J</color>\n↓\nDouble Bet <color=#FFFF00>x1.5</color>"; break;
                        case 9: extraText.text="HIT With <color=#00FFFF>A,K or Q</color>\n↓\nDouble Bet <color=#FFBB00>x2</color>"; break;
                        case 10: extraText.text="HIT With <color=#00FFFF>A or K</color>\n↓\nDouble Bet <color=#FF8800>x2.5</color>"; break;
                        case 11: extraText.text="HIT With <color=#00FFFF>A</color>\n↓\nDouble Bet <color=#FF3300>x4</color>"; break;
                        case 4: extraText.text="HIT With <color=#00FFFF>2,3,4 or 5</color>\n↓\nDouble Bet <color=#FFFF00>x1.5</color>"; break;
                        case 3: extraText.text="HIT With <color=#00FFFF>2,3 or 4</color>\n↓\nDouble Bet <color=#FFBB00>x2</color>"; break;
                        case 2: extraText.text="HIT With <color=#00FFFF>2 or 3</color>\n↓\nDouble Bet <color=#FF8800>x2.5</color>"; break;
                        case 1: extraText.text="HIT With <color=#00FFFF>2</color>\n↓\nDouble Bet <color=#FF3300>x4</color>"; break;
                        default: extraText.text=""; break;
                    }
                }
                guideText.text="Choose HIGH or LOW.";
                yield return new WaitUntil(() => (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.G)));
                adSrc.PlayOneShot(systemSound[2]);
            }
            guideText.text="Good Luck!";
            //ここではholdCheck=trueでHIGH、falseでLOWの選択として扱う
            if(Input.GetKey(KeyCode.J) && i!=0){
                holdCheck[i]=true;
                doubleUnderCardMeter[i].text="<color=#FF6600>HIGH</color>";
            }else if(Input.GetKey(KeyCode.G) && i!=0){
                holdCheck[i]=false;
                doubleUnderCardMeter[i].text="<color=#0066FF>LOW</color>";
            }
            cardScript cScr=cardObject[i].GetComponent<cardScript>();
            cScr.index=deckScr.deck[i];
            hand[0].rank[i]=RankSearcher(cScr.index);
            hand[0].suit[i]=SuitSearcher(cScr.index);
            adSrc.PlayOneShot(systemSound[0]);
            if(i==4 && hand[0].rank[3]!=0 && hand[0].rank[3]!=12){
                cScr.SlowCard();
                yield return new WaitForSeconds(2.1f);
            }else if(i==0){
                cScr.CardAnim();
                yield return new WaitForSeconds(0.3f);
            }else{
                cScr.DoubleCardAnim();
                yield return new WaitForSeconds(0.5f);
            }
            //追加テキスト表示などの処理を加える予定
            if(i!=0){
                //選択に合わせて処理を変更
                if(holdCheck[i]){
                    if(hand[0].rank[i]>=hand[0].rank[i-1]){
                        passCount++;
                        //スペシャル倍率
                        if(i==4 && hand[0].rank[i]>hand[0].rank[i-1]){
                            if(hand[0].rank[i-1]==8){
                                extra++;
                            }else if(hand[0].rank[i-1]==9){
                                extra+=2;
                            }else if(hand[0].rank[i-1]==10){
                                extra+=3;
                            }else if(hand[0].rank[i-1]==11){
                                extra+=6;
                            }
                        }
                    }else{
                        break;
                    }
                }else{
                    if(hand[0].rank[i]<=hand[0].rank[i-1]){
                        passCount++;
                        //スペシャル倍率
                        if(i==4 && hand[0].rank[i]<hand[0].rank[i-1]){
                            if(hand[0].rank[i-1]==4){
                                extra++;
                            }else if(hand[0].rank[i-1]==3){
                                extra+=2;
                            }else if(hand[0].rank[i-1]==2){
                                extra+=3;
                            }else if(hand[0].rank[i-1]==1){
                                extra+=6;
                            }
                        }
                    }else{
                        break;
                    }
                }
                adSrc.PlayOneShot(systemSound[12]);
            }
        }
        //Debug.Log("finish");
        //passCount=4で完走、3でセーブ、2以下は失敗
        if(passCount==4){
            guideText.text="You Win!";
            extra*=2;
            collectableWin=doubleWager*extra/2;
            hand[0].handFlag=Judge(0);
            SpecialBonusCalc(extra,2);
            if(collectableWin>doubleWager*2){
                dealerCont.JumpingAnim();
            }
            pubNum.win=collectableWin;
            WinSoundPlay(1);
            nextWin=collectableWin*2;
            DoubleTextIndi();
            gameState &= ~State.ISDEAL;
            gameState |= State.WINNING;
            dealerCont.FaceChanger(9);
        }else if(passCount==3){
            guideText.text="Double Down or Collect.";
            adSrc.PlayOneShot(systemSound[16]);
            collectableWin=doubleWager*extra/2;
            hand[0].handFlag=Judge(0);
            SpecialBonusCalc(1,1);
            if(collectableWin>doubleWager){
                dealerCont.JumpingAnim();
            }
            pubNum.win=collectableWin;
            nextWin=collectableWin*2;
            DoubleTextIndi();
            gameState &= ~State.ISDEAL;
            gameState |= State.WINNING;
            dealerCont.FaceChanger(3);
        }else{
            adSrc.PlayOneShot(systemSound[13]);
            pubNum.win=0;
            pubNum.win+=savedWin;
            if(pubNum.win>0){
                winmeter.text=pubNum.win.ToString();
                paidmeter.text="PAID : "+pubNum.win.ToString();
                pubNum.credit+=pubNum.win;
            }
            winsound.SoundPlay(0);
            gameState &= ~State.ISDEAL;
            gameState &= ~State.INDOUBLE;
            gameState |= State.BETACCEPT;
            dealerCont.FaceChanger(7);
            collectmeter.text="\n"+"COLLECTED";
        }
        winmeter.text=pubNum.win.ToString();
    }
    //レッドブラックダブル開始
    public IEnumerator RedBlackDouble(){
        guideText.text="Good Luck!";
        int selectedCardNum=0;
        //この値はRED=1,BLACK=0であることを示す
        int dealerColor,playerColor;
        for(int i=0;i<doubleUnderCardMeter.Length;i++){
            doubleUnderCardMeter[i].text="";
        }
        for(int i=0;i<cardObject.Length;i++){
            cardObject[i].GetComponent<cardScript>().DefaultCard();
            adSrc.PlayOneShot(systemSound[0]);
            yield return new WaitForSeconds(0.1f);
        }
        //メインの処理
        cardScript cScr=cardObject[0].GetComponent<cardScript>();
        cScr.index=deckScr.deck[0];
        hand[0].suit[0]=SuitSearcher(cScr.index);
        if(hand[0].suit[0]==1 || hand[0].suit[0]==2){
            dealerColor=1;
        }else{
            dealerColor=0;
        }
        adSrc.PlayOneShot(systemSound[0]);
        doubleUnderCardMeter[0].text="<color=#FFAA00>DEALER</color>";
        cScr.CardAnim();
        yield return new WaitForSeconds(0.3f);
        guideText.text="Which Card is the Same Color as the Dealer's?";
        //G~Kのどれかを入力されるまで待機
        yield return new WaitUntil(() => (Input.GetKeyDown(KeyCode.G) || Input.GetKeyDown(KeyCode.H) || Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.K)));
        adSrc.PlayOneShot(systemSound[2]);
        guideText.text="Good Luck!";
        if(Input.GetKey(KeyCode.G)){
            selectedCardNum=1;
        }else if(Input.GetKey(KeyCode.H)){
            selectedCardNum=2;
        }else if(Input.GetKey(KeyCode.J)){
            selectedCardNum=3;
        }else if(Input.GetKey(KeyCode.K)){
            selectedCardNum=4;
        }
        doubleUnderCardMeter[selectedCardNum].text="<color=#00FF33>PLAYER</color>";
        //選んだカード以外表示
        for(int i=1;i<cardObject.Length;i++){
            if(i!=selectedCardNum){
                cScr=cardObject[i].GetComponent<cardScript>();
                cScr.index=deckScr.deck[i];
                hand[0].suit[i]=SuitSearcher(cScr.index);
                adSrc.PlayOneShot(systemSound[0]);
                cScr.CardAnim();
                yield return new WaitForSeconds(0.15f);
            }
        }
        yield return new WaitForSeconds(0.3f);
        cScr=cardObject[selectedCardNum].GetComponent<cardScript>();
        cScr.index=deckScr.deck[selectedCardNum];
        hand[0].suit[selectedCardNum]=SuitSearcher(cScr.index);
        if(hand[0].suit[selectedCardNum]==1 || hand[0].suit[selectedCardNum]==2){
            playerColor=1;
        }else{
            playerColor=0;
        }
        adSrc.PlayOneShot(systemSound[0]);
        cScr.DoubleCardAnim();
        yield return new WaitForSeconds(0.5f);
        //判定
        if(playerColor==dealerColor){
            guideText.text="You Win!";
            collectableWin=doubleWager*2;
            //flush check
            for(int i=1;i<cardObject.Length;i++){
                if(hand[0].suit[0]!=hand[0].suit[i]){
                    break;
                }
                if(i==4){
                    collectableWin=doubleWager*10;
                    //フラッシュ成立時は勝った時に一緒に喜ぶ
                    dealerCont.JumpingAnim();
                }
            }
            pubNum.win=collectableWin;
            WinSoundPlay(1);
            nextWin=collectableWin*2;
            DoubleTextIndi();
            gameState &= ~State.ISDEAL;
            gameState |= State.WINNING;
            dealerCont.FaceChanger(9);
        }else{
            pubNum.win=0;
            pubNum.win+=savedWin;
            if(pubNum.win>0){
                winmeter.text=pubNum.win.ToString();
                paidmeter.text="PAID : "+pubNum.win.ToString();
                pubNum.credit+=pubNum.win;
            }
            winsound.SoundPlay(0);
            gameState &= ~State.ISDEAL;
            gameState &= ~State.INDOUBLE;
            gameState |= State.BETACCEPT;
            dealerCont.FaceChanger(7);
            collectmeter.text="\n"+"COLLECTED";
        }
        winmeter.text=pubNum.win.ToString();
    }
}
