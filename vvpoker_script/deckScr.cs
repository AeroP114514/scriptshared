using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deckScr : MonoBehaviour
{
    const int shuffleAmount=9;
    public List<int> deck,nextDeck;
    public int cardAmount;
    void Start()
    {
        DeckShuffle();
    }
    //山札シャッフル
    public void DeckShuffle(){
        if(deck==null){
            deck=new List<int>();
        }else{
            deck.Clear();
        }
        //カード数が4刻みで2からの各ランクに対応、スートはc>d>h>sの順でカード番号52以降はジョーカー
        for(int n=0;n<cardAmount;n++){
            deck.Add(n);
        }
        //shuffleAmountの回数だけシャッフルする
        for(int d=0;d<shuffleAmount;d++){
            int p=deck.Count;
            while(p>1){
                p--;
                int f=Random.Range(0,p+1);
                int y=deck[f];
                deck[f]=deck[p];
                deck[p]=y;
            }
        }
        MakeSecondDeck();
    }
    //最初の5枚以降のカード用に、シャッフル可能とするためデッキ化する
    public void MakeSecondDeck(){
        if(nextDeck==null){
            nextDeck=new List<int>();
        }else{
            nextDeck.Clear();
        }
        for(int i=5;i<cardAmount;i++){
            nextDeck.Add(deck[i]);
        }
    }
    //nextDeckのシャッフル処理
    public void ShuffleSecondDeck(){
        //shuffleAmountの回数だけシャッフルする
        for(int d=0;d<shuffleAmount;d++){
            int p=nextDeck.Count;
            while(p>1){
                p--;
                int f=Random.Range(0,p+1);
                int y=nextDeck[f];
                nextDeck[f]=nextDeck[p];
                nextDeck[p]=y;
            }
        }
    }
}
