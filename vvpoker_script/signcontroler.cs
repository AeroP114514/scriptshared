using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class signcontroler : MonoBehaviour
{
    public float timer,tgtime;
    SpriteRenderer spr;
    Animator anim;
    public Sprite[] illust;
    void Start()
    {
        ReSetup();
        spr=GetComponent<SpriteRenderer>();
        anim=GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(spr.sprite==illust[0]){
            timer+=Time.deltaTime;
            if(timer>tgtime){
                ReSetup();
                StartCoroutine(Anm());
            }
        }
    }
    //瞬きの時間再設定
    void ReSetup(){
        timer=0.0f;
        tgtime=Random.Range(2.7f,5.6f);
    }
    //瞬きするアニメーション
    IEnumerator Anm(){
        spr.sprite=illust[1];
        yield return new WaitForSeconds(0.06f);
        if(spr.sprite==illust[1]) spr.sprite=illust[0];
    }
    //顔を変える
    public void FaceChanger(int id){
        spr.sprite=illust[id];
    }
    //ジャンプする
    public void JumpingAnim(){
        anim.SetTrigger("jumptr");
    }
}
