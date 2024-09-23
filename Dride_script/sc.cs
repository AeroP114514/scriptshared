using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sc : MonoBehaviour
{
    void deathSC(){
        SceneManager.LoadScene("result");
    }
    public void tgo(){
        SceneManager.LoadScene("title");
    }
}
