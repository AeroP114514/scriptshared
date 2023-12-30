using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuScr : MonoBehaviour
{
    public GameObject[] Pgs;
    int pg=1;
    public AudioClip[] sfx;
    AudioSource snd;
    void Start()
    {
        Rst();
        Pgs[0].SetActive(true);
        snd=GetComponent<AudioSource>();
    }

    void pChanger(){
        Rst();
        Pgs[pg-1].SetActive(true);
    }

    public void redo(){
        pg++;
        if(pg>Pgs.Length){
            pg=1;
        }
        snd.PlayOneShot(sfx[0]);
        pChanger();
    }
    public void undo(){
        pg--;
        if(pg<1){
            pg=Pgs.Length;
        }
        snd.PlayOneShot(sfx[1]);
        pChanger();
    }

    void Rst(){
        for(int i=0;i<Pgs.Length;i++){
            Pgs[i].SetActive(false);
        }
    }
}
