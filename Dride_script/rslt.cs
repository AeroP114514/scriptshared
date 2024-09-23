using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class rslt : MonoBehaviour
{
    public TextMeshProUGUI sctx;
    public Texture[] cm;
    public RawImage[] ra;
    public bgmgen1 bu;

    void Start()
    {

    }

    void Update()
    {
        sctx.text=pub.Score.ToString()+":";
        if(pub.Score>=20){
            ra[0].texture=cm[2];
            ra[1].texture=cm[6];
            ra[2].texture=cm[9];
            sctx.faceColor=new Color(1.0f,0.0f,0.0f);
            sctx.text+="S";
            bu.sndPlay(3);
        }else if(pub.Score>=10){
            ra[0].texture=cm[1];
            ra[1].texture=cm[5];
            ra[2].texture=cm[8];
            sctx.faceColor=new Color(1.0f,0.5f,0.0f);
            sctx.text+="A";
            bu.sndPlay(2);
        }else if(pub.Score>=5){
            ra[0].texture=cm[1];
            ra[1].texture=cm[4];
            ra[2].texture=cm[8];
            sctx.faceColor=new Color(1.0f,1.0f,0.0f);
            sctx.text+="B";
            bu.sndPlay(1);
        }else{
            ra[0].texture=cm[0];
            ra[1].texture=cm[3];
            ra[2].texture=cm[7];
            sctx.faceColor=new Color(0.3f,0.3f,0.3f);
            sctx.text+="C";
            bu.sndPlay(0);
        }
    }
}
