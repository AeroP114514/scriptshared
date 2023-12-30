using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wgrcntr : MonoBehaviour
{
    GameObject i;
    gm s;
    bool f=true;
    void Start()
    {
        i=GameObject.Find("shooter");
        s=i.GetComponent<gm>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow) && s.isplay==false && s.del==false){
            s.BUP();
        }
        if(Input.GetKeyDown(KeyCode.DownArrow) && s.isplay==false && s.del==false){
            s.BDN();
        }
        if(Input.GetKey(KeyCode.U) && f){
            f=false;
            StartCoroutine("inc");
        }

    }
    IEnumerator inc(){
            s.coinin();
            yield return new WaitForSeconds(0.02f);
            f=true;
    }
}
