using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reelscr : MonoBehaviour
{
    public int reelmode,randNum,reelid,reelSymbolNum=26,reelStage; //reelid=1でメインセンターリール
    int chanceControl;
    float targetPosition,timer,startReelUpSpeed=2.9f,stopReelUpSpeed=3.1f,scrollReelLimit=-25.8f;
    public float spd;
    float[,] multiPosMain={{-1.0f,-10.0f,-17.0f,-25.0f},{-6.0f,-14.0f,-18.0f,-24.0f},{-2.0f,-8.0f,-12.0f,-22.0f},{-4.0f,-15.0f,-20.0f,0},
                           {-3.0f,-9.0f,-21.0f,0},{-7.0f,-19.0f,0,0},{-11.0f,-16.0f,0,0},{-5.0f,0,0,0},{-13.0f,0,0,0},{-23.0f,0,0,0},{0.0f,0,0,0}},
             multiPosCenter={{-1.0f,-15.0f,-25.0f,-15.0f},{-2.0f,-6.0f,-13.0f,-20.0f},{-8.0f,-11.0f,-18.0f,-22.0f},{-4.0f,-17.0f,-24.0f,0},{-3.0f,-9.0f,-21.0f,0},
                             {-7.0f,-10.0f,-19.0f,0},{-12.0f,-16.0f,0,0},{-5.0f,0,0,0},{-23.0f,0,0,0},{-14.0f,0,0,0},{0.0f,0,0,0}};
    int[,] positionIndex={{4,4,4,3,3,2,2,1,1,1,1},{4,4,4,3,3,3,2,1,1,1,1}};
    public int symbol;
    public bool stopchk,fac,sevenFlag,rsfx;
    public bgmMngr bgms;
    public FeatureBgmPlay ftBGM;

    public AudioClip[] se;
    AudioSource snd;
    public SymbolGenerator symbolGenerator;

    void Start()
    {
        reelmode=0;
        timer=0.0f;
        spd=-16.7f;
        stopchk=true;
        snd=GetComponent<AudioSource>();
        symbolGenerator.ReelInstantiate();
    }

    // Update is called once per frame
    void Update()
    {
        switch(reelmode){
            case 1: transform.Translate(0.0f,startReelUpSpeed*Time.deltaTime,0.0f); timer+=Time.deltaTime; break;
            case 2: timer+=Time.deltaTime; break;
            case 3: transform.Translate(0.0f,spd*Time.deltaTime,0.0f); if(transform.position.y<scrollReelLimit){transform.position=new Vector2(transform.position.x,transform.position.y+26.0f);} timer+=Time.deltaTime; break;
            case 4: transform.Translate(0.0f,spd*Time.deltaTime,0.0f); timer+=Time.deltaTime; break;
            case 5: transform.Translate(0.0f,stopReelUpSpeed*Time.deltaTime,0.0f); break;
            case 6: timer+=Time.deltaTime;
                    if(timer>=1.5f){
                        timer=0.0f;
                        if(symbol==10){
                            timer+=Random.Range(0.0f,1.0f);
                            if(targetPosition==-1.0f){
                                reelmode=7;
                                bgms.StartBGM();
                            }else if(targetPosition==-25.0f){
                                reelmode=8;
                                bgms.StartBGM();
                            }
                        }else{
                            randNum=Random.Range(0,3);
                            timer+=Random.Range(0.0f,2.0f);
                            if(targetPosition==-1.0f && randNum==0){
                                reelmode=7;
                                bgms.StartBGM();
                            }else if(targetPosition==-25.0f && randNum==0){
                                reelmode=8;
                                bgms.StartBGM();
                            }else{
                                stopchk=true;
                                ftBGM.MuteCancel();
                                reelmode=0;
                            }
                        }
                    }
                    break;
            case 7: if(transform.position.y<0.0f){
                        transform.Translate(0.0f,0.215f*Time.deltaTime,0.0f);
                    }else{
                        timer+=Time.deltaTime; 
                        bgms.StopBGM();
                        transform.position=new Vector2(transform.position.x,0.0f);
                    }
                    break;
            case 8: if(transform.position.y>-26.0f){
                        transform.Translate(0.0f,-0.215f*Time.deltaTime,0.0f);
                    }else{
                        timer+=Time.deltaTime;
                        bgms.StopBGM();
                        transform.position=new Vector2(transform.position.x,-26.0f);
                    }
                    break;
            case 9: if(transform.position.y<1.0f){
                        targetPosition=1.0f;
                        transform.Translate(0.0f,0.74f*Time.deltaTime,0.0f);
                    }else{
                        timer+=Time.deltaTime; 
                        transform.position=new Vector2(transform.position.x,targetPosition);
                    }
                    break;
            case 10: if(transform.position.y>-27.0f){
                        targetPosition=-27.0f;
                        transform.Translate(0.0f,-0.74f*Time.deltaTime,0.0f);
                    }else{
                        timer+=Time.deltaTime; 
                        transform.position=new Vector2(transform.position.x,targetPosition);
                    }
                    break;
            default: break;
        }
        if(reelmode==1 && timer>=0.12f){
            timer=0.0f;
            reelmode=2;
        }
        //リール回転
        if(reelmode==2 && timer>=0.12f){
            timer=0.0f;
            reelmode=3;
            if(reelid==1 || reelid==2){
                bgms.ReelSpinBGM();
                if(reelid==1){
                    Time.timeScale=pub.gameSpeed;
                }
            }
        }
        if(reelmode==4 && transform.position.y<targetPosition-0.48f){
            reelmode=5;
            transform.position=new Vector2(transform.position.x,targetPosition-0.48f);
            if(rsfx || reelid==1 || reelid==2){
                snd.PlayOneShot(se[0]);
            }
        }
        if(reelmode==5 && transform.position.y>=targetPosition){
            reelmode=0;
            transform.position=new Vector2(transform.position.x,targetPosition);
            //ワイルド前後で止まった場合
            if(reelid==1 && (targetPosition==-1.0f || targetPosition==-25.0f)){
                bgms.StopBGM();
                ftBGM.SoundMute();
                reelmode=6;
            }else{
                stopchk=true;
            }
            timer=0.0f;
        }
        if((reelmode==7 || reelmode==8)&& timer>=3.0f){
            if(symbol!=10){
                timer=0.0f;
                reelmode+=2;
                snd.PlayOneShot(se[1]);
            }else{
                timer=0.0f;
                stopchk=true;
                ftBGM.SoundStop();
                reelmode=0;
            }
        }
        if((reelmode==9 || reelmode==10)&& timer>=0.5f){
            timer=0.0f;
            stopchk=true;
            ftBGM.MuteCancel();
            reelmode=0;
        }
        if(pub.bonusRen>1){
            chanceControl=3;
        }else{
            chanceControl=25;
        }
    }

    public void ReelStart(){
        timer=0.0f;
        stopchk=false;
        rsfx=true;
        reelmode=1;
    }
    public void ReelStop(int f,bool sc){
        timer=0.0f;
        symbol=f;
        sevenFlag=false;
        if(reelid==0){
            randNum=Random.Range(0,positionIndex[0,symbol]);
            targetPosition=multiPosMain[symbol,randNum];
        }else{
            randNum=Random.Range(0,positionIndex[1,symbol]);
            targetPosition=multiPosCenter[symbol,randNum];
            //センターリールがジョーカーの場合たまにずらす
            if(reelid==1 && symbol==10){
                randNum=Random.Range(0,chanceControl);
                if(randNum==0){
                    //何もしない
                }else if(randNum%2==0){
                    targetPosition-=1.0f;
                }else{
                    targetPosition-=25.0f;
                }
            }
        }
        if(symbol>=8){
            sevenFlag=true;
        }
        transform.position=new Vector2(transform.position.x,targetPosition+3.1f);
        rsfx=sc;
        reelmode=4;
    }
}
