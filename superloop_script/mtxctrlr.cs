using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class mtxctrlr : MonoBehaviour
{
    public TextMeshProUGUI t1,t2;
    public TextMeshProUGUI feature,countMeter;
    bool flg;
    float timer,waitTime;
    int textlv=0;

    void Start()
    {
        timer=1.0f;
        waitTime=0.0f;
        flg=false;
        if(pub.credit>0){
            textlv=1;
        }else{
            textlv=0;
        }
    }

    void Update(){
        switch(textlv){
            case 0:
                timer+=Time.deltaTime;
                t1.alignment=TextAlignmentOptions.Center;
                t1.fontSize=100.0f;
                if(timer>0.7f){
                    timer=0.0f;
                    if(t1.text=="Insert Coins"){
                        t1.text="<color=#888888>Insert Coins</color>";
                    }else{
                        t1.text="Insert Coins";
                    }
                }
                t2.text="Game Over.";
                break;
            case 1:
                timer+=Time.deltaTime;
                t1.alignment=TextAlignmentOptions.Center;
                t1.fontSize=80.0f;
                if(timer>0.7f){
                    timer=0.0f;
                    if(t1.text=="Insert Coin or<color=#888888> Bet</color>"){
                        t1.text="<color=#888888>Insert Coin</color> or Bet";
                    }else{
                        t1.text="Insert Coin or<color=#888888> Bet</color>";
                    }
                }
                t2.text="Play Now.";
                break;
            case 2:
                timer+=Time.deltaTime;
                t1.alignment=TextAlignmentOptions.Top;
                t1.fontSize=85.0f;
                if(timer>0.7f){
                    timer=0.0f;
                    if(t1.text=="Insert Coin"+"\n"+"Bet or <color=#888888>Start</color>"){
                        t1.text="Insert Coin"+"\n"+"<color=#888888>Bet</color> or Start";
                    }else{
                        t1.text="Insert Coin"+"\n"+"Bet or <color=#888888>Start</color>";
                    }
                }
                t2.text="Play 1 to "+pub.maxBet.ToString()+" credits.";
                break;
            case 4:
                timer+=Time.deltaTime;
                if(timer>0.1f){
                    timer=0.0f;
                    if(t1.text=="<color=#FFFF00>WIN!!</color>"){
                        t1.text="<color=#00FFFF>WIN!!</color>";
                    }else{
                        t1.text="<color=#FFFF00>WIN!!</color>";
                    }
                }
                break;
            case 5:
                timer+=Time.deltaTime;
                t1.alignment=TextAlignmentOptions.Center;
                t1.fontSize=75.0f;
                if(timer>0.7f){
                    timer=0.0f;
                    if(t1.text=="Play Double Down!"+"\n"+"or <color=#888888>Collect</color>"){
                        t1.text="Play <color=#888888>Double Down!</color>"+"\n"+"or Collect</color>";
                    }else{
                        t1.text="Play Double Down!"+"\n"+"or <color=#888888>Collect</color>";
                    }
                }
                break;
            case 6:
                timer+=Time.deltaTime;
                t1.fontSize=120.0f;
                if(timer>0.08f){
                    timer=0.0f;
                    if(t1.text=="<color=#FF0000>W<color=#FF7700>I<color=#FFFF00>N<color=#00FF00>N<color=#00FFFF>E<color=#0000FF>R<color=#7700FF>!<color=#FF00FF>!</color>"){
                        t1.text="<color=#FF7700>W<color=#FFFF00>I<color=#00FF00>N<color=#00FFFF>N<color=#0000FF>E<color=#7700FF>R<color=#FF00FF>!<color=#FF0000>!</color>";
                    }else if(t1.text=="<color=#FF7700>W<color=#FFFF00>I<color=#00FF00>N<color=#00FFFF>N<color=#0000FF>E<color=#7700FF>R<color=#FF00FF>!<color=#FF0000>!</color>"){
                        t1.text="<color=#FFFF00>W<color=#00FF00>I<color=#00FFFF>N<color=#0000FF>N<color=#7700FF>E<color=#FF00FF>R<color=#FF0000>!<color=#FF7700>!</color>";
                    }else if(t1.text=="<color=#FFFF00>W<color=#00FF00>I<color=#00FFFF>N<color=#0000FF>N<color=#7700FF>E<color=#FF00FF>R<color=#FF0000>!<color=#FF7700>!</color>"){
                        t1.text="<color=#00FF00>W<color=#00FFFF>I<color=#0000FF>N<color=#7700FF>N<color=#FF00FF>E<color=#FF0000>R<color=#FF7700>!<color=#FFFF00>!</color>";
                    }else if(t1.text=="<color=#00FF00>W<color=#00FFFF>I<color=#0000FF>N<color=#7700FF>N<color=#FF00FF>E<color=#FF0000>R<color=#FF7700>!<color=#FFFF00>!</color>"){
                        t1.text="<color=#00FFFF>W<color=#0000FF>I<color=#7700FF>N<color=#FF00FF>N<color=#FF0000>E<color=#FF7700>R<color=#FFFF00>!<color=#00FF00>!</color>";
                    }else if(t1.text=="<color=#00FFFF>W<color=#0000FF>I<color=#7700FF>N<color=#FF00FF>N<color=#FF0000>E<color=#FF7700>R<color=#FFFF00>!<color=#00FF00>!</color>"){
                        t1.text="<color=#0000FF>W<color=#7700FF>I<color=#FF00FF>N<color=#FF0000>N<color=#FF7700>E<color=#FFFF00>R<color=#00FF00>!<color=#00FFFF>!</color>";
                    }else if(t1.text=="<color=#0000FF>W<color=#7700FF>I<color=#FF00FF>N<color=#FF0000>N<color=#FF7700>E<color=#FFFF00>R<color=#00FF00>!<color=#00FFFF>!</color>"){
                        t1.text="<color=#7700FF>W<color=#FF00FF>I<color=#FF0000>N<color=#FF7700>N<color=#FFFF00>E<color=#00FF00>R<color=#00FFFF>!<color=#0000FF>!</color>";
                    }else if(t1.text=="<color=#7700FF>W<color=#FF00FF>I<color=#FF0000>N<color=#FF7700>N<color=#FFFF00>E<color=#00FF00>R<color=#00FFFF>!<color=#0000FF>!</color>"){
                        t1.text="<color=#FF00FF>W<color=#FF0000>I<color=#FF7700>N<color=#FFFF00>N<color=#00FF00>E<color=#00FFFF>R<color=#0000FF>!<color=#7700FF>!</color>";
                    }else{
                        t1.text="<color=#FF0000>W<color=#FF7700>I<color=#FFFF00>N<color=#00FF00>N<color=#00FFFF>E<color=#0000FF>R<color=#7700FF>!<color=#FF00FF>!</color>";
                    }
                }
                break;
            case 7:
                waitTime+=Time.deltaTime;
                if(waitTime>1.0f){
                    pldd();
                }
                break;
            case 10:
                timer+=Time.deltaTime;
                waitTime+=Time.deltaTime;
                if(timer>0.1f){
                    timer=0.0f;
                    if(t1.text=="<color=#FFFF00>WIN!!</color>"){
                        t1.text="<color=#00FFFF>WIN!!</color>";
                    }else{
                        t1.text="<color=#FFFF00>WIN!!</color>";
                    }
                }
                if(waitTime>1.0f){
                    pldd();
                }
                break;

            default: break;
        }

    }

    public void tlvc(int n){
        if(textlv!=n){
            timer=10.0f;
        }
        textlv=n;
    }
    public void htlvc(int n){
        timer=10.0f;
        textlv=n;
    }
    public void maintx(){
        if(!flg){
            flg=true;
            timer=0.0f;
        }
        if(pub.credit>0){
            textlv=1;
            t1.text="Insert Coin or<color=#888888> Bet</color>";
            t1.fontSize=80.0f;
        }else{
            textlv=0;
            t1.text="Insert Coins";
            t1.fontSize=95.0f;
        }
    }

    public void inscbs(){
        if(!flg){
            flg=true;
            timer=0.0f;
        }
        textlv=2;
        t1.alignment=TextAlignmentOptions.Top;
        t1.text="Insert Coin"+"\n"+"Bet or <color=#888888>Start</color>";
        t1.fontSize=85.0f;
        t2.text="Play 1 to "+pub.maxBet.ToString()+" credits.";
    }
    public void gl(){
        flg=false;
        timer=0.0f;
        waitTime=0.0f;
        textlv=3;
        t1.alignment=TextAlignmentOptions.Center;
        t1.text="<color=#FF7700>GOOD LUCK!</color>";
        t1.fontSize=100.0f;
        t2.text="";
    }
    public void wnr(){
        if(!flg){
            flg=true;
            timer=0.0f;
        }
        textlv=4;
        t1.fontSize=100.0f;
        t1.text="<color=#FFFF00>WIN!!</color>";
    }
    public void pldd(){
        flg=false;
        timer=0.0f;
        textlv=5;
        t1.text="Play Double Down!"+"\n"+"or <color=#888888>Collect</color>";
        t1.fontSize=75.0f;
    }
    public void paids(){
        flg=false;
        timer=0.0f;
        textlv=6;
        t1.text="<color=#FF0000>W<color=#FF7700>I<color=#FFFF00>N<color=#00FF00>N<color=#00FFFF>E<color=#0000FF>R<color=#7700FF>!<color=#FF00FF>!</color>";
        t1.fontSize=120.0f;
    }
    public void svdw(){
        t1.text="<color=#00FF00>SAVED WIN</color>";
        t1.fontSize=85.0f;
    }
    public void srl(){
        t1.text="Select a Reel";
        t1.fontSize=90.0f;
        t2.text="Which is higher than Dealer's?";
    }
    public void ps(){
        waitTime=0.0f;
        t1.text="PUSH";
        t1.fontSize=90.0f;
        textlv=7;
    }
    public void wb(){
        t1.text="WIN+BONUS!!";
        textlv=9;
        t1.fontSize=95.0f;
    }
    public void lb(){
        t1.text="BONUS GET!!";
        textlv=8;
        t1.fontSize=90.0f;
    }
    public void DoubleWinText(){
        if(!flg){
            flg=true;
            timer=0.0f;
        }
        waitTime=0.0f;
        textlv=10;
        t1.text="<color=#FFFF00>WIN!!</color>";
    }
    //テキストを消す
    public void TextClear(){
        t1.text="";
        t2.text="";
    }
    public void FeatureIndicate(){
        if(pub.bonusRen>1){
            t1.fontSize=80.0f;
            t1.text="只今<color=#FFFF00>"+pub.bonusRen.ToString()+"</color>連チャン中!";
        }else{
            t1.text="<color=#FF00FF>FREE SPIN!</color>";
        }
    }
    public void FeatureCount(int n){
        textlv=11;
        if(n>0){
            feature.text="REMAINING"+'\n'+'\n'+"FREE SPIN";
            countMeter.text=n.ToString();
        }else{
            feature.text="";
            countMeter.text="";
        }
    }
    public void ChanceZoneCount(int n){
        if(n>0){
            feature.text="REMAINING"+'\n'+'\n'+"CHANCE GAME";
            countMeter.text=n.ToString();
        }else{
            feature.text="";
            countMeter.text="";
        }
    }
    public void FeatureReset(){
        textlv=12;
        feature.text="";
        countMeter.text="";
        t1.fontSize=70.0f;
        t1.text="FREE SPIN IS OVER.";
    }
}