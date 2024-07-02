using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Canvasの表示管理
//主にGマネージャー経由で呼び出させる

public class UI_Ctrl : MonoBehaviour
{
    [Header("プレイ中UI")] public GameObject playUI;
    [Header("スタート画面UI")] public GameObject startUI;
    [Header("リザルトUI")] public GameObject resultUI;
    [Header("リセットボタンUI")] public GameObject resetUI;
    [Header("サウンドUI")] public GameObject soundUI;
    [Header("ポーズ画面UI")] public GameObject pauseUI;
    [Header("黒スクリーンUI")] public GameObject dScreenUI;
    [Header("赤スクリーンUI")] public GameObject rScreenUI;
    [Header("スコアUI")] public GameObject ScoreUI;
    [Header("スコアブースト")] public GameObject boostUI; //タイマーからSetActiveを制御する
    [Header("ダメージエフェクト")] public GameObject damef;
    [Header("ダメージエフェクト位置")] public Transform damefPos; //親オブジェクトにするCanvas2
    [Header("ダメージエフェクト角度")] public Transform damefRot; //メインカメラの角度
    [Header("アラートUI")] public GameObject arertUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 開始前状態
    /// </summary>
    public void PlayStay()
    {
        //プレイヤーを非表示
        GManager.instance.player.SetActive(false);
        //非表示UIを設定
        playUI.SetActive(false);
        resultUI.SetActive(false);
        resetUI.SetActive(false);
        rScreenUI.SetActive(false);
    }

    /// <summary>
    /// スタートボタン挙動
    /// </summary>
    public void PlayStart()
    {
        //プレイヤーをアクティブ
        GManager.instance.player.SetActive(true);
        //UIをアクティブ、スタートボタンと音量UIを非表示
        playUI.SetActive(true);
        startUI.SetActive(false);
        soundUI.SetActive(false);
    }

    /// <summary>
    /// 半透過画面解除
    /// </summary>
    public void DarkScreenOut()
    {
        dScreenUI.SetActive(false);
    }


    /// <summary>
    /// リザルト画面表示
    /// </summary>
    public void ResultActive()
    {
        ScoreUI.SetActive(false);
        resultUI.SetActive(true);
        GManager.instance.isGOver = true;
    }


    /// <summary>
    /// リザルト：リセットボタン表示
    /// </summary>
    public void ResetActive()
    {
        resetUI.SetActive(true);
    }


    /// <summary>
    /// アラートメッセージON
    /// </summary>
    public void ArertOn()
    {
        arertUI.SetActive(true);
    }

    /// <summary>
    /// アラートメッセージOFF
    /// </summary>
    public void ArertOff()
    {
        arertUI.SetActive(false);
    }

    /// <summary>
    /// ポーズ画面
    /// </summary>
    public void PauseIn()
    {
        pauseUI.SetActive(true);
        resetUI.SetActive(true);
        soundUI.SetActive(true);
        dScreenUI.SetActive(true);
    }

    /// <summary>
    /// ポーズ画面解除
    /// </summary>
    public void PauseOut()
    {
        pauseUI.SetActive(false);
        resetUI.SetActive(false);
        soundUI.SetActive(false);
        //※dScreenUIはタイムスケールを動かせば勝手に透明化後消える
    }

}
