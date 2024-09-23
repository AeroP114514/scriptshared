using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class dataload : MonoBehaviour
{
    TextAsset csv;
    List<string[]> Datas=new List<string[]>();

    public int[] reelSymbolMain,reelSymbolCenter;
    void Awake()
    {
        pub.maxBet=800;
        //TextAssetにcsvを読み込み
        csv=Resources.Load<TextAsset>("reeldata");
        StringReader reader=new StringReader(csv.text);
        //行をすべてReadLineする
        while(reader.Peek()!=-1){
            string line=reader.ReadLine();
            //カンマ区切りで文字列で格納
            Datas.Add(line.Split(','));
        }
        //データをintにして代入
        for(int i=0;i<Datas.Count;i++){
            reelSymbolMain[i]=int.Parse(Datas[i][0]); //A列
            reelSymbolCenter[i]=int.Parse(Datas[i][1]); //B列
            //Debug.Log(reelSymbolMain[i]);
        }
        for(int i=0;i<pub.wintableline.Length;i++){
            pub.wintableline[i]=int.Parse(Datas[i][3]); //D列
        }
        for(int i=0;i<pub.wintableAny.GetLength(0);i++){
            for(int j=0;j<pub.wintableAny.GetLength(1);j++){
                pub.wintableAny[i,j]=int.Parse(Datas[j][i+5]); //F~M列
            }
        }
        for(int i=0;i<pub.mainReelThreshold.Length;i++){
            pub.mainReelThreshold[i]=int.Parse(Datas[i][14]); //O列
            pub.centerReelThreshold[i]=int.Parse(Datas[i][15]); //P列
            pub.doublePlayerThreshold[i]=int.Parse(Datas[i][16]); //Q列
            pub.doubleDealerThreshold[i]=int.Parse(Datas[i][17]); //R列
            pub.SuperReelThreshold[i]=int.Parse(Datas[i][19]); //T列
            //Debug.Log(pub.mainReelThreshold[i]);
        }
    }
}
