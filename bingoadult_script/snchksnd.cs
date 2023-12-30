using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snchksnd : MonoBehaviour
{
    public AudioClip SFX;
    AudioSource snd;
    void Start()
    {
        snd=GetComponent<AudioSource>();
    }
    public void pl(){
        snd.PlayOneShot(SFX);    
    }
}
