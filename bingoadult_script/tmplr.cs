using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tmplr : MonoBehaviour
{
    Animator an;
    void Start()
    {
        an=GetComponent<Animator>();
    }

    // Update is called once per frame
    public void lampon(){
        an.SetBool("temp",true);
    }
    public void lampoff(){
        an.SetBool("temp",false);
    }
}
