using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nvgt : MonoBehaviour
{
    public Transform[] snpos;
    int tg;
    Transform tr;
    bool on=true;

    void Start(){
        tr=GetComponent<Transform>();
    }

    void Update()
    {
        if(on){
            tr.LookAt(snpos[tg]);
        }
    }

    public void nvst(int s){
        tg=s;
        on=true;
    }
    public void nvoff(){
        on=false;
    }
}
