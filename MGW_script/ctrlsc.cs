using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ctrlsc : MonoBehaviour
{
    public TextMeshProUGUI cmtr;
    public AudioClip SFX0,SFX1;
    public int pluscount;
    AudioSource snd;

    void Start()
    {
        stn.credit=PlayerPrefs.GetInt("Credits",0);
        snd=GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        cmtr.text="CREDIT\n"+stn.credit.ToString();
        if(Input.GetKeyDown(KeyCode.I)){
            stn.credit++;
            snd.PlayOneShot(SFX0);
        }
        if(Input.GetKeyDown(KeyCode.O)){
            stn.credit+=100;
            snd.PlayOneShot(SFX0);
        }
        if(Input.GetKeyDown(KeyCode.Y)){
            stn.credit+=pluscount;
            snd.PlayOneShot(SFX0);
        }
    }
    void OnApplicationQuit() {
        PlayerPrefs.SetInt("Credits",stn.credit);
        PlayerPrefs.Save();
    }
}
