using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FreeResult : MonoBehaviour
{
    public TextMeshProUGUI meter;
    public Animator an;
    public Sprite[] txtr;
    SpriteRenderer sprn;

    void Start()
    {
        sprn=GetComponent<SpriteRenderer>();
    }

    public void FR(int p,int i){
        sprn.sprite=txtr[i];
        meter.text=p.ToString("N0");
        an.SetBool("fl", true);
    }
    public void Canc(){
        an.SetBool("fl", false);
    }
}
