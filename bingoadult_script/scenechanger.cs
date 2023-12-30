using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scenechanger : MonoBehaviour
{
    public void SC0(){
        SceneManager.LoadScene("title");
    }
    public void SC1(){
        SceneManager.LoadScene("game");
    }
}
