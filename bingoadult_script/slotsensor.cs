using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slotsensor : MonoBehaviour
{
    public GameObject i;
    reelm e;
    private void Start() {
        e=i.GetComponent<reelm>();
    }

    void OnTriggerEnter2D(Collider2D other){
        e.A();
    }
}
