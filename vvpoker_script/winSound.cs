using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class winSound : MonoBehaviour
{
    //操作で切れるタイプの音を鳴らすコード
    public AudioClip[] sound;
    public AudioSource adSrc;

    //引数iのsoundを再生
    public void SoundPlay(int i){
        adSrc.clip=sound[i];
        adSrc.Play();
    }
}
