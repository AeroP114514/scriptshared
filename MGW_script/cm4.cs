using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cm4 : MonoBehaviour
{
    Image sr;
    bool f;
    void Start()
    {
        sr=GetComponent<Image>();
    }

    void Update(){
        if(f){
            sr.color = Color.HSVToRGB((Time.time/4)%1.0f,1,0.4f);
        }else{
            sr.color=new Color32(150,21,86,184);
        }
    }

    public void NoG(){
        f=false;
    }
    public void FrG(){
        f=true;
    }
}
