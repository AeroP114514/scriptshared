using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class indicatorBpnl : MonoBehaviour
{
    public GameObject[] pnls;
    gm ins;
    void Start()
    {
        GameObject o=GameObject.Find("shooter");
        ins=o.GetComponent<gm>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(ins.bingom){
            case 0:
                for(int i=0;i<pnls.Length;i++){
                    pnls[i].SetActive(false);
                } break;
            case 1:
                pnls[0].SetActive(true); break;
            case 2:
                pnls[1].SetActive(true);
                pnls[0].SetActive(false); break;
            case 3:
                pnls[2].SetActive(true);
                pnls[1].SetActive(false);
                pnls[0].SetActive(false); break;
            case 4:
                pnls[3].SetActive(true);
                pnls[2].SetActive(false);
                pnls[1].SetActive(false);
                pnls[0].SetActive(false); break;
            case 5:
                pnls[4].SetActive(true);
                for(int j=0;j<4;j++){
                    pnls[j].SetActive(false);
                } break;
            case 6:
                pnls[5].SetActive(true);
                for(int h=0;h<5;h++){
                    pnls[h].SetActive(false);
                } break;
            case 8:
                pnls[6].SetActive(true);
                for(int k=0;k<6;k++){
                    pnls[k].SetActive(false);
                } break;
            default: break;
        }
    }
}
