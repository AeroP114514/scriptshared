using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lampcont : MonoBehaviour
{
    Animator an;
    void Start()
    {
        an=GetComponent<Animator>();
    }

    public void lampon(){
        an.SetBool("lamp",true);
    }
    public void lampoff(){
        an.SetBool("lamp",false);
    }
}
