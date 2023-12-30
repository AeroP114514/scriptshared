using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Lind : MonoBehaviour
{
    public GameObject[] line;
    public int[] won=new int[15];
    public int anypay,cntr;
    public TextMeshProUGUI ind;
    bool flag=false;
    float tm;

    void Start(){
        ind.text="";
        anypay=0;
    }
    void Update(){
        if(flag){
            tm+=Time.deltaTime;
            if(tm>=1.0f){
                tm=0.0f;
                for(int i=0;i<9999;i++){
                    cntr++;
                    cntr%=16;
                    if(cntr==15){
                        if(anypay>0){
                            cancel();
                            ind.text="Scatter Bonus = Pays "+anypay.ToString();
                            break;
                        }
                    }else if(won[cntr]>0 && cntr!=15){
                        ind.text="Line "+(cntr+1).ToString()+" = Pays "+won[cntr].ToString();
                        cancel();
                        line[cntr].SetActive(true);
                        break;
                    }
                }
            }
        }
    }

    public void L1(int f){
        ind.text="";
        cntr=0;
        flag=false;
        for(int i=0;i<line.Length;i++){
            if(i<f){
                line[i].SetActive(true);
            }else{
                line[i].SetActive(false);
            }
        }
    }
    public void L5(){
        tm=0.0f;
        flag=true;
        for(int i=0;i<line.Length;i++){
            if(won[i]>0){
                line[i].SetActive(true);
            }else{
                line[i].SetActive(false);
            }
        }
    }
    public void Loff(){
        ind.text="";
        cntr=0;
        flag=false;
        for(int i=0;i<line.Length;i++){
            won[i]=0;
            line[i].SetActive(false);
        }
    }
    public void cancel(){
        for(int i=0;i<line.Length;i++){
            line[i].SetActive(false);
        }
    }
}
