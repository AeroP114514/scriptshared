using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pub
{
    public static int credit;
    public static float gameSpeed;
    public static int[] wintableline=new int[11];
    public static int[,] wintableAny={{0,0,0,0,0,0,0,0,0,6},
                                     {0,0,0,0,0,0,1,2,5,18},
                                     {0,0,0,0,0,1,3,10,40,200},
                                     {0,0,0,0,1,2,6,20,80,400},
                                     {0,0,0,0,1,3,12,40,160,800},
                                     {0,0,0,0,1,3,14,50,200,1000},
                                     {0,0,0,1,2,5,20,120,400,2000},
                                     {0,0,0,1,3,10,40,240,800,4000}
                                     };
    //ブランク、チェリー、オレンジ、プラム、スイカ、ベル、ダイヤ、BAR、赤7、青7，ジョーカー
    public static int[] mainReelThreshold=new int[11],centerReelThreshold=new int[11],doubleDealerThreshold=new int[11],doublePlayerThreshold=new int[11],SuperReelThreshold=new int[11];
    public static int maxBet,bonusRen,bonusTotalWon;
}
