using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sdchk : MonoBehaviour
{    
    void OnTriggerEnter2D(Collider2D other) {
        GameObject obj=GameObject.Find("shooter");
        gm di=obj.GetComponent<gm>();
        if(di.odds<3){
            GameObject sdg=GameObject.Find("SNDGN1");
            sfxgen ins=sdg.GetComponent<sfxgen>();
            ins.pl();
            di.odds++;
            this.gameObject.SetActive(false);
        }
    }
}
