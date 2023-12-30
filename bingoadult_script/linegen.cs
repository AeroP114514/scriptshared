using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class linegen : MonoBehaviour
{
    public GameObject[] spl=new GameObject[8];
    int z,id=0;
    public AudioClip[] SFX;
    AudioSource snd;

    void Start(){
        snd=GetComponent<AudioSource>();
    }

    public void gs(int n){
        z=n;
        StartCoroutine("linerot");
    }

    IEnumerator linerot(){
        snd.PlayOneShot(SFX[0]);
        for(int i=0;i<11;i++){
            spl[id].SetActive(false);
            id+=Random.Range(1,8);
            if(id>7){
                id-=8;
            }
            spl[id].SetActive(true);
            yield return new WaitForSeconds(0.1f);
        }
        snd.Stop();
        snd.PlayOneShot(SFX[1]);
        spl[id].SetActive(false);
        spl[z].SetActive(true);
        yield return new WaitForSeconds(0.1f);
    }

    public void res(){
        spl[z].SetActive(false);
    }
}
