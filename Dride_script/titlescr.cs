using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class titlescr : MonoBehaviour
{
    public AudioClip[] SE; 
    AudioSource snd;
    bool prs;
    public Animator an,a2;

    void Start()
    {
        snd=GetComponent<AudioSource>();
        prs=false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!prs&&(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))){
            prs=true;
            snd.PlayOneShot(SE[0]);
            an.SetTrigger("nx");
            a2.SetTrigger("tr");
            StartCoroutine(sch());
        }
    }
    IEnumerator sch(){
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("game");
    }
}
