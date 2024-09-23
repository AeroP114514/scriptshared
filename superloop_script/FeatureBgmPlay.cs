    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class FeatureBgmPlay : MonoBehaviour
    {
        public AudioClip[] featureHead,featureBgm,finishJingle;
        AudioSource audioSource; 
        GameMain gameMain;
        public bool loopFlag,fadeout;
        int index;

        void Start()
        {
            audioSource=GetComponent<AudioSource>();
            GameObject obj=GameObject.Find("gamemain");
            gameMain=obj.GetComponent<GameMain>();
        }
        void Update(){
            if(!audioSource.isPlaying && loopFlag){
                loopFlag=false;
                audioSource.clip=featureBgm[index];
                audioSource.loop=true;
                audioSource.Play();
            }
            if(fadeout){
                audioSource.volume-=0.4f*Time.deltaTime;
                if(audioSource.volume<=0.0f){
                    SoundStop();
                    fadeout=false;
                    audioSource.volume=0.4f;
                }
            }
        }
        //CZ中BGMの再生
        public void ChanceSoundPlay(){
            if(!audioSource.isPlaying){
                if(gameMain.chanceState==GameMain.ChanceState.CHANCEMODE){
                    index=0;
                }else if(gameMain.chanceState==GameMain.ChanceState.HIGHCHANCE){
                    index=2;
                }
                MuteCancel();
                audioSource.clip=featureHead[index];
                audioSource.loop=false;
                audioSource.Play();
                loopFlag=true;
            }
        }
        public void FreeSoundPlay(){
            if(!audioSource.isPlaying){
                if(gameMain.freeState==GameMain.FreeState.FREESPIN){
                    index=1;
                }else if(gameMain.freeState==GameMain.FreeState.SUPERFREE){
                    index=3;
                }
                MuteCancel();
                audioSource.clip=featureHead[index];
                audioSource.loop=false;
                audioSource.Play();
                loopFlag=true;
            }
        }
        //ミュート
        public void SoundMute(){
            audioSource.mute=true;
        }
        //解除
        public void MuteCancel(){
            audioSource.mute=false;
        }
        //BGM停止
        public void SoundStop(){
            audioSource.Stop();
            loopFlag=false;
        }
        public void FreeFinish(int i){
            audioSource.mute=false;
            audioSource.PlayOneShot(finishJingle[i]);
        }
        public void ChanceOver(){
            fadeout=true;
        }
    }
