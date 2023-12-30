using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dest : MonoBehaviour
{
    GameObject ins;
    gm gmngr;
    void Start()
    {
        GameObject ins=GameObject.Find("shooter");
        gmngr=ins.GetComponent<gm>();
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y<=-9.5f){
            Destroy(this.gameObject);
            gmngr.sht=false;
        }
    }

    void OnTriggerEnter2D(Collider2D f) {
        if(f.gameObject.tag=="breakblock"){
            Destroy(this.gameObject);
        }
    }
}
