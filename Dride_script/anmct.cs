using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class anmct : MonoBehaviour
{
    public Animator[] an;

    public void as1(){
        an[0].SetTrigger("go");
    }
    public void as2(){
        an[0].SetTrigger("nex");
    }
}
