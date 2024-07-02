using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//スタートボタンの挙動

public class UI_StartBotton : MonoBehaviour
{
    [Header("スタート音")] public AudioClip seStart;

    private bool isPush; //ボタン押された判定
    private float timer; //ボタン演出タイマー
    private RectTransform rt; //ボタンのレクトトランスフォーム

    void Start()
    {
        rt = GetComponent<RectTransform>();

        Time.timeScale = 0; //開始前は時間を止める
        GManager.instance.uic.PlayStay(); //開始前状態
    }

    // Update is called once per frame
    void Update()
    {
        if(isPush)
        {
            //時間経過でボタンを少し小さくする(押された演出)
            //演出後にゲーム開始処理

            if(timer >= 0.2f) //ボタン演出時間終了(時間は要調整)
            {
                Time.timeScale = 1; //タイムスケールをリセット(リトライ時に0のままになるため)

                //プレイヤー、敵、プレイUIをアクティブ、スタートボタンを非表示化
                GManager.instance.uic.PlayStart();
            }

            timer += Time.unscaledDeltaTime;
        }
    }

    public void OnPressed()
    {
        isPush = true;
        SoundManager.instance.PlaySE(seStart);
        rt.localScale = rt.localScale * 0.9f;
    }
}
