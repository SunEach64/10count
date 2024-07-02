using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SoundVolume : MonoBehaviour
{
    private Slider sv; //スライダー
    private float startSV; //開始時スライダー値 

    void Start()
    {
        sv = GetComponent<Slider>();
        startSV = SoundManager.instance.bfVol;
        sv.value = startSV;
    }

    void Update()
    {
        if(!SoundManager.instance.startSet) //初回開始時(リトライ時は無視される)
        {
            //開始時音量にスライダーを調整
            if(sv.value == SoundManager.instance.firstVol)
            {
                SoundManager.instance.startSet = true; //初回設定挙動を終了
            }
            else
            {
                sv.value = SoundManager.instance.firstVol;
            }
        }
    }

    public void SetVolume(float volume)
    {
        SoundManager.instance.ChangeVolume(volume);
    }
}
