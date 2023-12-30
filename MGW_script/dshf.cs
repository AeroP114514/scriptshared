using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dshf : MonoBehaviour
{
    public List<int> deck;
    public int NofCard;
    void Start()
    {
        ds();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ds(){
        if(deck==null){
            deck=new List<int>();
        }else{
            deck.Clear();
        }
        for(int n=0;n<NofCard;n++){
            deck.Add(n);
        }
        for(int d=0;d<8;d++){
            int p=deck.Count;
            while(p>1){
                p--;
                int f=Random.Range(0,p+1);
                int y=deck[f];
                deck[f]=deck[p];
                deck[p]=y;
            }
        }
    }
}
