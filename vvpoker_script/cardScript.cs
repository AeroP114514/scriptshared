using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cardScript : MonoBehaviour
{
    public Sprite backillust,blank;
    public Sprite[] image;
    public int index;
    Animator anim;

    void Start()
    {
        anim=GetComponent<Animator>();
    }
 

    //カードの裏面を表示
    public void DefaultCard(){
        GetComponent<SpriteRenderer>().sprite=backillust;
    }
    //ブランク表示
    public void BlankCard(){
        GetComponent<SpriteRenderer>().sprite=blank;
    }
    //カードアニメーション
    public void CardAnim(){
        anim.SetTrigger("flag");
    }
    //カードアニメーション（ダブル時）
    public void DoubleCardAnim(){
        anim.SetTrigger("dcopen");
    }
    //カードアニメーション（スローオープン）
    public void SlowCard(){
        anim.SetTrigger("slowplay");
    }
    //indexの値のカードイラストに切り替え
    public void CardIndicate(){
        GetComponent<SpriteRenderer>().sprite=image[index];
    }
}
