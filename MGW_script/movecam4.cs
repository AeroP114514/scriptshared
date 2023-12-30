using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movecam4 : MonoBehaviour
{
    public int f=0;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(f==1){
            transform.Translate(0,20.0f*Time.deltaTime,0);
            if(transform.position.y>=14.0f){
                f=0;
                transform.position=new Vector3(transform.position.x,14.0f,0);
            }
        }else if(f==2){
            transform.Translate(0,-20.0f*Time.deltaTime,0);
            if(transform.position.y<=0.6f){
                f=0;
                transform.position=new Vector3(transform.position.x,0.6f,0);
            }
        }
    }
}
