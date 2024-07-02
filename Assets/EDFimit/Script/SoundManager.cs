using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] AudioSource aSouce;
    [SerializeField] AudioClip testSE; //ボリューム変更時のテスト音
    [HideInInspector] public float bfVol = 0.5f; //前のボリューム
    [HideInInspector] public float firstVol = 0.5f; //初期音量
    [HideInInspector] public Slider slider;

    [HideInInspector] public bool startSet; //ゲーム開始時の音量0不具合を躱すための処理用

    private bool isChangeVolume; //音量変更を行った際のフラグ

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    void Start()
    {
        
        aSouce.volume = 0.5f; //初期音量
    }

    void Update()
    {
        //マウスボタンを離した、且つボリュームが変化
        if (Input.GetMouseButtonUp(0) && isChangeVolume)
        {
            PlaySE(testSE); //テスト音鳴らす
            bfVol = aSouce.volume; //前ボリューム更新
            isChangeVolume = false; //フラグリセット
        }
        
    }

    /// <summary>
    /// ボリュームの設定
    /// </summary>
    /// <param name="volume"></param>
    public void ChangeVolume(float volume)
    {
        //UI_SoundVolumeから呼び出され、スライダー値でボリューム変更
        aSouce.volume = volume;

        if(!startSet)
        {
            //開始時音量調整が終わるまで何もしない
        }
        else
        {
            isChangeVolume = true; //音量変更判定をトゥルー
        }
    }

    public void PlaySE(AudioClip clip)
    {
        aSouce.PlayOneShot(clip);
    }

}
