using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sens : MonoBehaviour
{
    public SpriteRenderer spr;
    public Sprite off,on,act;
    public GameObject pnl;
    public int sid;
    bool check=false,inc=false;
    public bool bng=false;
    gm gmngr;
    snchksnd sfxl;
    public sfxgen z;

    void Start()
    {
        GameObject ins=GameObject.Find("shooter");
        gmngr=ins.GetComponent<gm>();
        GameObject kko=GameObject.Find("SNDGN2");
        sfxl=kko.GetComponent<snchksnd>();
        spr=pnl.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(check){
            if(bng){
                spr.sprite=act;
            }else{
                spr.sprite=on;
            }
        }else{
            spr.sprite=off;
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(inc){
            gmngr.lose();
        }
    }
    void OnTriggerExit2D(Collider2D other) {
        if(gmngr.del==false){
            if(gmngr.isplay){
                sfxl.pl();
            }
            check=true;
            inc=true;
            gmngr.inchk(sid);
            gmngr.sht=false;
        }
    }
    public void res(){
        check=false;
        inc=false;
        bng=false;
    } 
    public void slotc(){
        if(!check){
            check=true;
        }else{
            if(gmngr.odds<3){
                gmngr.odds++;
                z.pl();
            }
        }
    }
    public void lucky(){
        inc=false;
    }
}
