using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fdCtrl : MonoBehaviour
{
    public Texture[] fd;
    public RawImage[] ri;
    public Animator[] an;
    int[] cntr=new int[2];
    int[] k={2,1,1,1,1,1};

    public void setsumei(){
        cntr[0]=0;
        cntr[1]=0;
        ri[0].texture=fd[0];
        ri[1].texture=fd[12];
        StartCoroutine(stwt());
    }
    public void next(){
        for(int i=0;i<2;i++){
            if(cntr[i]<6){
                cntr[i]++;
            }
        }
        if(cntr[0]<6){
            ri[0].texture=fd[cntr[0]];
            ri[1].texture=fd[10+k[cntr[1]]];
            an[0].SetTrigger("okuri");
            an[1].SetTrigger("okuri");
        }else{
            gm mas=GameObject.Find("Player").GetComponent<gm>();
            mas.gamemode=2;
        }
    }
    IEnumerator stwt(){
        yield return new WaitForSeconds(9.5f);
        an[0].SetTrigger("okuri");
        an[1].SetTrigger("okuri");
    }
}
