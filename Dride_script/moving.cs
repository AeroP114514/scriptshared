using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class moving : MonoBehaviour
{
    public float spd=0.0f;
    float swing=110.0f,spdunit;
    public TextMeshProUGUI[] tx;
    public gm gamemas;
    float rotHorizontal=0;
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            pub.Score++;
        }
        if(gamemas.ingame){
            tx[1].text="SCORE:"+pub.Score.ToString();
            transform.Translate(0.0f,0.0f,spd*Time.deltaTime);
            if(Input.GetMouseButtonDown(2)){
                spd+=5.0f;
                spdunit=spd;
            }
            if(spd>0){
                spd-=(spd*spd/spdunit)*Time.deltaTime+(0.5f*Time.deltaTime);
            }else if(spd<0){
                spd=0.0f;
            }
            if(Input.GetMouseButton(0) && !Input.GetMouseButton(1)){
                transform.Rotate(0.0f,-swing*Time.deltaTime,0.0f);
                rotHorizontal -= 2.0f;
                if(rotHorizontal < -0.3f*spd){ rotHorizontal = -0.3f*spd;}
            }
            if(Input.GetMouseButton(1) && !Input.GetMouseButton(0)){
                transform.Rotate(0.0f,swing*Time.deltaTime,0.0f);
                rotHorizontal += 2.0f;
                if (rotHorizontal > 0.3f*spd) { rotHorizontal = 0.3f*spd; }
            }
            if(Input.GetMouseButton(0) && Input.GetMouseButton(1) && spd>0){
                spd-=15.0f*Time.deltaTime;
            }
            if (rotHorizontal > 0)
            {
                rotHorizontal -= 1.0f;
                if (rotHorizontal < 0) { rotHorizontal = 0; }
            }
            else if (rotHorizontal < 0)
            {
                rotHorizontal += 1.0f;
                if (rotHorizontal > 0) { rotHorizontal = 0;}
            }
            var a = Camera.main.transform.localEulerAngles;
            a.z = rotHorizontal;
            Camera.main.transform.localEulerAngles = a;
        }
    }
    void OnTrigger(Collider ps){
        if(ps.gameObject.tag=="bldg"){
            spd-=16.0f*Time.deltaTime;
        }
    }
}
