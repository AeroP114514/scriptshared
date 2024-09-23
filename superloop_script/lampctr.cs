using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lampctr : MonoBehaviour
{
    public Animator an;

    void Start()
    {
        an=GetComponent<Animator>();
    }

    public void lampchange(bool f){
        an.SetBool("flag",f);
    }
}
