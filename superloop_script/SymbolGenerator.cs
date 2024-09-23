using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolGenerator : MonoBehaviour
{
    public GameObject[] symbols=new GameObject[10];
    public reelscr reel;
    int margine=6;
    dataload database;
    GameObject symbolObject;
    public void ReelInstantiate()
    {
        //dataloadのオブジェクト取得
        GameObject obj=GameObject.Find("Datareader");
        database=obj.GetComponent<dataload>();
        //シンボル配置
        for(int i=0;i<reel.reelSymbolNum+(margine*2);i++){
            if(reel.reelid==0){
                if(database.reelSymbolMain[i]!=0){
                    symbolObject=Instantiate(symbols[database.reelSymbolMain[i]-1],this.transform);
                }else{
                    continue;
                }
            }else{
                if(database.reelSymbolCenter[i]!=0){
                    symbolObject=Instantiate(symbols[database.reelSymbolCenter[i]-1],this.transform);
                    if(database.reelSymbolCenter[i]==10 && reel.reelid==1){
                        symbolObject.tag="Changeable";
                    }
                }else{
                    continue;
                }
            }
            symbolObject.GetComponent<Renderer>().sortingOrder=reel.reelStage;
            switch(reel.reelStage){
                case -10: symbolObject.transform.localPosition=new Vector2(0,i-margine); break;
                case -28: symbolObject.transform.localPosition=new Vector2(0,(i-margine)-1.4f); break;
                case -55: symbolObject.transform.localPosition=new Vector2(0,(i-margine)-2.8f); break;
            }
        }
    }
}
