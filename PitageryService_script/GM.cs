using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GM : MonoBehaviour
{
    public int credit=0,mode,point,tgpoint,maxbet=4;
    int[] bet=new int[7],winml={2,4,6,8,10,30,0};
    int[] pos1={5,0,3,1,0,4,0,1,3,0,2,0,1,4,0,1,2,6,1,3,0,2,0,1,0,4,0,2,1,0,2,0,3,0,1,6},de={0,1,5,10,20,30,40};
    float[] rd={0.0f,-10.0f,-20.0f,-30.0f,-40.0f,-50.0f,-60.0f,-70.0f,-80.0f,
                -90.0f,-100.0f,-110.0f,-120.0f,-130.0f,-140.0f,-150.0f,-160.0f,-170.0f,
                180.0f,170.0f,160.0f,150.0f,140.0f,130.0f,120.0f,110.0f,100.0f,
                90.0f,80.0f,70.0f,60.0f,50.0f,40.0f,30.0f,20.0f,10.0f};
    int win=0,r=0,r2=0,pos2=0,ddbet=0;
    float rpspd=0.02f;
    public TextMeshProUGUI crdmtr,winmtr;
    public TextMeshProUGUI[] bmtr;
    public AudioClip[] clp=new AudioClip[10];
    AudioSource snd;
    public lampcont LC,LC2;
    // Start is called before the first frame update
    void Start()
    {
        snd=GetComponent<AudioSource>();
        mode=0;
        point=0;
    }

    // Update is called once per frame
    void Update()
    {
        crdmtr.text=credit.ToString();
        winmtr.text=win.ToString();
        for(int i=0;i<bmtr.Length;i++){
            bmtr[i].text=bet[i].ToString();
        }
        if(Input.GetKeyDown(KeyCode.I)){
            credit++;
        }
        if((Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.T) || Input.GetKeyDown(KeyCode.Y))&&mode==0&&credit>0){
            win=0;
            for(int i=0;i<bmtr.Length;i++){
                bet[i]=0;
            }
            mode=1;
        }
        if(mode==1){
            betacp();
        }
        if(Input.GetKeyDown(KeyCode.S)&&mode==1){
            mode=2;
            rott();
        }
        if(mode==3){
            if(pos1[tgpoint]!=6){
                win=bet[pos1[tgpoint]]*winml[pos1[tgpoint]];
            }
            if(win>0){
                mode=4;
                snd.PlayOneShot(clp[2]);
                lcon();
            }else{
                snd.PlayOneShot(clp[4]);
                mode=0;
            }
        }
        takesc();
        selectmode();
    }
    void betacp(){
        if(Input.GetKeyDown(KeyCode.Q)&&credit>0&&bet[0]<maxbet){
            upc(0);
        }
        if(Input.GetKeyDown(KeyCode.W)&&credit>0&&bet[1]<maxbet){
            upc(1);
        }
        if(Input.GetKeyDown(KeyCode.E)&&credit>0&&bet[2]<maxbet){
            upc(2);
        }
        if(Input.GetKeyDown(KeyCode.R)&&credit>0&&bet[3]<maxbet){
            upc(3);
        }
        if(Input.GetKeyDown(KeyCode.T)&&credit>0&&bet[4]<maxbet){
            upc(4);
        }
        if(Input.GetKeyDown(KeyCode.Y)&&credit>0&&bet[5]<maxbet){
            upc(5);
        }
    }
    void upc(int f){
        credit--;
        bet[f]++;
        snd.PlayOneShot(clp[0]);
    }
    void rott(){
        r=Random.Range(0,36);
        tgpoint=r;
        int cntr=0;
        for(int i=0;i<bmtr.Length;i++){
            if(bet[i]>0){
                cntr++;
            }
        }
        r2=Random.Range(0,100);
        if(r2<de[cntr]){
            tgpoint=(tgpoint%2)*18+17;
        }
        StartCoroutine("spinlamp");
    }
    void takesc(){
        if(mode==4 && Input.GetKeyDown(KeyCode.D)){
            mode=5;
            ddbet=0;
            lcoff();
            StartCoroutine("doubleup");
        }
        if(mode==4 && Input.GetKeyDown(KeyCode.C)){
            credit+=win;
            lcoff();
            mode=0;
        }
    }
    void selectmode(){
        if(mode==5 && Input.GetKeyDown(KeyCode.F)){
            mode=6;
            ddbet=1;
        }
        if(mode==5 && Input.GetKeyDown(KeyCode.J)){
            mode=6;
            ddbet=2;
        }
    }
    void judge(){
        if(ddbet==pos2){
            win*=2;
            lcon();
            snd.PlayOneShot(clp[3]);
            mode=4;
        }else{
            win=0;
            snd.PlayOneShot(clp[4]);
            mode=0;
        }
    }
    IEnumerator spinlamp(){
        int n=Random.Range(98,151);
        int h=Random.Range(13,20);
        for(int i=0;i<n;i++){
            spin();
            yield return new WaitForSeconds(rpspd);
        }
        int itvl=tgpoint-h;
        if(itvl<0){
            itvl+=36;
        }
        do{
            spin();
            yield return new WaitForSeconds(rpspd);
        }while(point!=itvl);
        for(int i=0;i<h;i++){
            spin();
            yield return new WaitForSeconds(rpspd*2);
        }
        yield return new WaitForSeconds(0.25f);
        mode=3;
    }
    void spin(){
        point++;
        if(point>pos1.Length-1){
            point=0;
        }
        transform.rotation=Quaternion.Euler(0.0f,0.0f,rd[point]);
        snd.PlayOneShot(clp[1]);
    }
    IEnumerator doubleup(){
        do{
            point--;
            if(point<0){
                point=35;
            }
            transform.rotation=Quaternion.Euler(0.0f,0.0f,rd[point]);
            snd.PlayOneShot(clp[1]);
            yield return new WaitForSeconds(rpspd);
        }while(ddbet==0);
        int m=Random.Range(0,2);
        if(m==1){
            point--;
            if(point<0){
                point=35;
            }
            transform.rotation=Quaternion.Euler(0.0f,0.0f,rd[point]);
            snd.PlayOneShot(clp[1]);
            yield return new WaitForSeconds(rpspd);
        }
        pos2=point%2+1;
        judge();
    }
    void lcon(){
        LC.lampon();
        LC2.lampon();
    }
    void lcoff(){
        LC.lampoff();
        LC2.lampoff();
    }
}
