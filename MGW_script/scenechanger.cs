using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scenechanger : MonoBehaviour
{
    public void SC0(){
        PlayerPrefs.SetInt("Credits",stn.credit);
        PlayerPrefs.Save();
        SceneManager.LoadScene("TITLE");
    }
    public void SC1(){
        PlayerPrefs.SetInt("Credits",stn.credit);
        PlayerPrefs.Save();
        SceneManager.LoadScene("JacksORBETTER");
    }
    public void SC2(){
        PlayerPrefs.SetInt("Credits",stn.credit);
        PlayerPrefs.Save();
        SceneManager.LoadScene("JokersWild");
    }
    public void SC3(){
        PlayerPrefs.SetInt("Credits",stn.credit);
        PlayerPrefs.Save();
        SceneManager.LoadScene("DeucesWild");
    }
    public void SC4(){
        PlayerPrefs.SetInt("Credits",stn.credit);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Freedeal5TP");
    }
    public void SC5(){
        PlayerPrefs.SetInt("Credits",stn.credit);
        PlayerPrefs.Save();
        SceneManager.LoadScene("SDW");
    }
    public void SC6(){
        PlayerPrefs.SetInt("Credits",stn.credit);
        PlayerPrefs.Save();
        SceneManager.LoadScene("FDTT");
    }
    public void SC7(){
        PlayerPrefs.SetInt("Credits",stn.credit);
        PlayerPrefs.Save();
        SceneManager.LoadScene("INSJO");
    }
    public void SC101(){
        PlayerPrefs.SetInt("Credits",stn.credit);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Slot");
    }
    public void SC102(){
        PlayerPrefs.SetInt("Credits",stn.credit);
        PlayerPrefs.Save();
        SceneManager.LoadScene("JokerSwing");
    }
    public void SC103(){
        PlayerPrefs.SetInt("Credits",stn.credit);
        PlayerPrefs.Save();
        SceneManager.LoadScene("FWBB");
    }
    public void SC104(){
        PlayerPrefs.SetInt("Credits",stn.credit);
        PlayerPrefs.Save();
        SceneManager.LoadScene("FWMM");
    }
}
