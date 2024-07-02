using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ポーズ解除ボタン

public class UI_ResumeButton : MonoBehaviour
{
    private bool isPush; //ボタン押された判定
    private float timer; //ボタン演出タイマー
    private RectTransform rt; //ボタンのレクトトランスフォーム

    void Start()
    {
        rt = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (isPush)
        {
            //時間経過でボタンを少し小さくする(押された演出)
            //演出後にゲーム開始処理

            if (timer >= 0.2f) //ボタン演出時間終了(時間は要調整)
            {
                isPush = false; //ボタン押し判定リセット
                timer = 0f; //タイマーリセット
                rt.localScale = Vector3.one; //ボタンサイズリセット
                Time.timeScale = 1; //タイムスケール1に変更
                GManager.instance.noShot = false; //射撃可能
                GManager.instance.uic.PauseOut(); //ポーズ画面解除
            }

            timer += Time.unscaledDeltaTime;
        }
    }

    public void OnPressed()
    {
        isPush = true;
        rt.localScale = rt.localScale * 0.9f;
    }
}
