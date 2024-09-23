using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameMain : _MainBase
{
    int[,] oddsTable=new int[4,15];
    int[] specialBonusBase=new int[15];
    string[] handName=new string[15];

    void Start()
    {
        //TextAssetにcsvを読み込み
        //Datas[x][y]においてcsv上ではx=縦方向、y=横方向に向かってデータを取得する
        csv=Resources.Load<TextAsset>("oddstable");
        StringReader reader=new StringReader(csv.text);
        //行をすべてReadLineする
        while(reader.Peek()!=-1){
            string line=reader.ReadLine();
            //カンマ区切りで文字列で格納
            Datas.Add(line.Split(','));
        }
        //各列で、データをintにして代入
        //凡例：oddsTable[x,y]のとき、「x=ベットレベル、y=役のフラグの値」で配当を設定する
        for(int i=0;i<oddsTable.GetLength(0);i++){
            for(int j=0;j<oddsTable.GetLength(1);j++){
                //A列を無視するため、iのアクセス位置は+1する
                oddsTable[i,j]=int.Parse(Datas[j][i]); //A~D列
            }
        }
        for(int i=0;i<handName.Length;i++){
            handName[i]=Datas[i][4]; //E列
        }
        for(int i=0;i<specialBonusBase.Length;i++){
            specialBonusBase[i]=int.Parse(Datas[i][5]); //F列
        }
        totalBet=0;
        betPerHand=0;
        gameState |= State.BETACCEPT;
        betPerHand=1;
        playHand=25;
        betMultiplier=1;
        totalBet=betPerHand*playHand*betMultiplier;
        cardMax=5;
        doublemode=DoubleStyle.STANDARD;
        sysType[0]="25 hands ";
        sysType[1]="1 Coin ";
        doubleLimit=200000;
        bonusLimit=100000000;
    }

    //繰り返し処理
    void Update()
    {
        CoinIn();
        IndiIntro();
        TextIndicate();
        BetAction();
        Deal();
        RepeatDeal();
        HitCardFlush();
        if(gameState.HasFlag(State.HOLDTIME)){
            HoldAction();
        }
        Collect();
        DoubleGo();
        DoubleSelect();
    }

    //ダブルダウン
    public override void DoubleGo(){
        if(Input.GetKeyDown(KeyCode.Return) && !gameState.HasFlag(State.INDOUBLE) && gameState.HasFlag(State.WINNING)){
            gameState |= State.INDOUBLE;
            originalWin=pubNum.win;
            collectableWin=pubNum.win;
            doubleWager=0;
            nextWin=originalWin*2;
            tryLuck=1;
            savedWin=0;
            dealerCont.FaceChanger(0);
            adSrc.PlayOneShot(systemSound[15]);
            mainTextUnit.SetActive(false);
            doubleGameUnit.SetActive(true);
            DoubleTextIndi();
            SpecialBonusDefault();
            collectmeter.text="\n"+"COLLECTABLE";
            tryLuck=0;
        }else if(Input.GetKeyDown(KeyCode.Return) && gameState.HasFlag(State.INDOUBLE) && gameState.HasFlag(State.WINNING)){
            gameState &= ~State.WINNING;
            gameState |= State.ISDEAL;
            doubleWager=collectableWin;
            collectableWin=0;
            nextWin=doubleWager*2;
            tryLuck++;
            dealerCont.FaceChanger(0);
            adSrc.PlayOneShot(systemSound[14]);
            deckScr.DeckShuffle();
            DoubleTextIndi();
            StartDoubleDown();
        }else if(Input.GetKeyDown(KeyCode.RightShift) && gameState.HasFlag(State.INDOUBLE) && gameState.HasFlag(State.WINNING) && collectableWin>1){
            gameState &= ~State.WINNING;
            gameState |= State.ISDEAL;
            savedWin+=collectableWin/2;
            //端数処理
            if(collectableWin%2==1){
                savedWin+=1;
            }
            doubleWager=collectableWin/2;
            collectableWin=0;
            nextWin=doubleWager*2;
            tryLuck++;
            dealerCont.FaceChanger(0);
            adSrc.PlayOneShot(systemSound[14]);
            deckScr.DeckShuffle();
            DoubleTextIndi();
            StartDoubleDown();
        }
    }
    //役の判定、ジョーカー非考慮
    public override int Judge(int handNum){
        bool outCheck=false,passCheck=false,flushFlag=false,straightFlag=false,royalFlag=false;
        int pairCount=0,hitRank=0,kicker=0,maxPairRank=0;
        //Listに該当ハンドのランクを代入
        List<int> handRank=new List<int>();
        for(int i=0;i<hand[handNum].rank.Length;i++){
            handRank.Add(hand[handNum].rank[i]);
        }
        handRank.Sort();
        //check flush、5枚のスートからペアを10個探し、すべて同一かをチェック
        for(int i=0;i<cardMax;i++){
            for(int j=i+1;j<cardMax;j++){    
                if(hand[handNum].suit[i]!=hand[handNum].suit[j]){
                    outCheck=true;
                    break;
                }
                if(i==3){
                    passCheck=true;
                }
            }
            if(passCheck){
                flushFlag=true;
                break;
            }else if(outCheck){
                break;
            }
        }
        //check straight、1-4番目の隣り合うランクを比較し、差が1でない時点で中断
        for(int i=0;i<3;i++){
            if(handRank[i+1]-handRank[i]!=1){
                break;
            }
            //最終判定
            if(i==2){
                if(handRank[4]-handRank[3]==1 || (handRank[3]==3 && handRank[4]==12)){
                    straightFlag=true;
                    //rankが10を起点としたストレートとなる場合ロイヤルの可能性あり
                    if(handRank[0]==8){
                        royalFlag=true;
                    }
                }
            }
        }
        //check pair
        for(int i=0;i<handRank.Count;i++){
            for(int j=i+1;j<handRank.Count;j++){
                if(handRank[i]==handRank[j]){
                    pairCount++;
                    if(handRank[i]>maxPairRank) maxPairRank=handRank[i];
                }
            }
        }
        //pairCount==6、4Kの際はキッカーの判定。3K2pもどこで成立しているかを判定するが、2pのみhitRankには成立に関係していないrankを代入する
        if(pairCount==6){
            hitRank=handRank[2];
            if(hitRank==handRank[4]){
                kicker=handRank[0];
            }else{
                kicker=handRank[4];
            }
        }else if(pairCount==3){
            hitRank=handRank[2];
        }else if(pairCount==2){
            int wInd=0;
            while(handRank.Count>1){
                if(handRank[wInd]==handRank[wInd+1]){
                    handRank.RemoveRange(wInd,2);
                    wInd=0;
                    continue;
                }
                wInd++;
            }
            hitRank=handRank[0];
        }
        //ここまでの判定から、最終的な役の決定
        if(flushFlag && straightFlag){
            for(int i=0;i<hand[handNum].hitCard.Length;i++){
                hand[handNum].hitCard[i]=true;
            }
            if(royalFlag){
                return 14;
            }else{
                return 13;
            }
        }
        if(pairCount==6){
            if(hitRank==12){
                if(kicker>=0 && kicker<=2){
                    for(int i=0;i<hand[handNum].hitCard.Length;i++){
                        hand[handNum].hitCard[i]=true;
                    }
                    return 12;
                }else{
                    for(int i=0;i<hand[handNum].hitCard.Length;i++){
                        if(hand[handNum].rank[i]==hitRank) hand[handNum].hitCard[i]=true;
                    }
                    return 11;
                }
            }else if(hitRank>=0 && hitRank<=2){
                if((kicker>=0 && kicker<=2) || kicker==12){
                    for(int i=0;i<hand[handNum].hitCard.Length;i++){
                        hand[handNum].hitCard[i]=true;
                    }
                    return 10;
                }else{
                    for(int i=0;i<hand[handNum].hitCard.Length;i++){
                        if(hand[handNum].rank[i]==hitRank) hand[handNum].hitCard[i]=true;
                    }
                    return 9;
                }
            }else{
                if(kicker==12){
                    for(int i=0;i<hand[handNum].hitCard.Length;i++){
                        hand[handNum].hitCard[i]=true;
                    }
                    return 8;
                }else{
                    for(int i=0;i<hand[handNum].hitCard.Length;i++){
                        if(hand[handNum].rank[i]==hitRank) hand[handNum].hitCard[i]=true;
                    }
                    return 7;
                }
            }
        }
        if(pairCount==4){
            for(int i=0;i<hand[handNum].hitCard.Length;i++){
                hand[handNum].hitCard[i]=true;
            }
            return 6;
        }
        if(flushFlag){
            for(int i=0;i<hand[handNum].hitCard.Length;i++){
                hand[handNum].hitCard[i]=true;
            }
            return 5;
        }
        if(straightFlag){
            for(int i=0;i<hand[handNum].hitCard.Length;i++){
                hand[handNum].hitCard[i]=true;
            }
            return 4;
        }
        if(pairCount==3){
            for(int i=0;i<hand[handNum].hitCard.Length;i++){
                if(hand[handNum].rank[i]==hitRank) hand[handNum].hitCard[i]=true;
            }
            return 3;
        }
        if(pairCount==2){
            //ここのみhitRankと一致していないものを対象にする
            for(int i=0;i<hand[handNum].hitCard.Length;i++){
                if(hand[handNum].rank[i]!=hitRank) hand[handNum].hitCard[i]=true;
            }
            return 2;
        }
        if(maxPairRank>=9 && pairCount==1){
            for(int i=0;i<hand[handNum].hitCard.Length;i++){
                if(hand[handNum].rank[i]==maxPairRank) hand[handNum].hitCard[i]=true;
            }
            return 1;
        }

        return 0;
    }
    //spボーナスのデフォルトテキストを表示
    public override void SpecialBonusDefault(){
        spBonusMeter.text="";
        for(int i=0;i<specialBonusBase.Length-1;i++){
            spBonusMeter.text+="BET x";
            spBonusMeter.text+=specialBonusBase[specialBonusBase.Length-(i+1)];
            spBonusMeter.text+="\n";
        }
    }
    //spボーナスのテキストを表示
    public override void SpecialBonusIndi(){
        spBonusMeter.text="";
        for(int i=0;i<specialBonusBase.Length-1;i++){
            spBonusMeter.text+=specialBonusBase[specialBonusBase.Length-(i+1)]*doubleWager;
            spBonusMeter.text+="\n";
        }
    }
    //配当の計算と表示
    public override void Calc(){
        for(int i=0;i<playHand;i++){
            pubNum.win+=oddsTable[betPerHand-1,hand[i].handFlag]*betMultiplier;
            if(hand[i].handFlag>0){
                HandWin(i);
            }
        }
        winmeter.text=pubNum.win.ToString();
    }
    //各ハンドの成立役を表示
    public override void HandNameIndi(int n){
        handNameMeter[n].text=handName[hand[n].handFlag];
    }
    //各ハンドの配当と成立役表示
    public override void HandWin(int n){
        winPerHandMeter[n].text=(oddsTable[betPerHand-1,hand[n].handFlag]*betMultiplier).ToString();
        handNameMeter[n].text=handName[hand[n].handFlag];
    }
    //spボーナス配当の計算
    public override void SpecialBonusCalc(int mpA,int mpB){
        collectableWin+=specialBonusBase[hand[0].handFlag]*doubleWager*mpA/mpB;
        //barObject indicate if you won the SPbonus.
        if(hand[0].handFlag>0){
            GameObject.Find("bar"+hand[0].handFlag.ToString()).GetComponent<Animator>().SetBool("flag",true);
        }
    }
}
