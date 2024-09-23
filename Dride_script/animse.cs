using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animse : MonoBehaviour
{
    public AudioClip[] SE; 
    AudioSource snd;

    void Start()
    {
        snd=GetComponent<AudioSource>();
    }

    public void sest(){
        snd.PlayOneShot(SE[0]);
    }
}
