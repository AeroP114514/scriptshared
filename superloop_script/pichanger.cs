using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pichanger : MonoBehaviour
{
    public Sprite[] gr;
    SpriteRenderer spr;
    int f;

    void Start()
    {
        spr=GetComponent<SpriteRenderer>();
        f=1;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z)){
            f--;
            if(f<0){
                f=gr.Length-1;
            }
            spr.sprite=gr[f];
        }
        if(Input.GetKeyDown(KeyCode.X)){
            f++;
            if(f>=gr.Length){
                f=0;
            }
            spr.sprite=gr[f];
        }
    }
}
