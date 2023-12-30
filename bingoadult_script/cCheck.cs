using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cCheck : MonoBehaviour
{
    gm scr;
    void Start()
    {
        GameObject mys=GameObject.Find("shooter");
        scr=mys.GetComponent<gm>();
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y<-8.6f){
            scr.credit++;
            Destroy(this.gameObject);
        }
    }
}
