using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class pmlbch : MonoBehaviour
{
    public TextMeshProUGUI ct;
    public Sprite gr1,gr2;
    SpriteRenderer sp;

    void Start()
    {
        sp=GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(ct.text==""){
            sp.sprite=gr1;
        }else{
            sp.sprite=gr2;
        }
    }
}
