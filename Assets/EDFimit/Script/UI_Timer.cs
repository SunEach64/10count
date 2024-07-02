using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//ゲーム中の制限時間
//0になったらタイムスケールでゲームを止めてリザルトとリセットボタン表示

public class UI_Timer : MonoBehaviour
{
    [Header("制限時間")] public float timeCount = 5f;

    private TMP_Text timerText; //制限時間の表示
    private bool isDead; //死亡判定(DeadForceを連続作動させないため)
    private bool isGOver; //ゲームオーバーフラグ
    private bool isResult; //リザルトへ遷移フラグ(遷移後は無駄な処理を止める)
    private float countGOver; //ゲームオーバーからの経過時間

    void Start()
    {
        timerText = GetComponent<TMP_Text>();
    }

    void Update()
    {
        if (!isGOver)
        {
            if (timeCount <= 0 || GManager.instance.pDeath) //時間切れorプレイヤー死亡を判定
            {
                if(GManager.instance.pDeath)
                {
                    PlayOut();
                }
                else //生存状態で時間切れ
                {
                    if(!isDead)
                    {
                        //強制で死亡にし、次フレームでPlayOut
                        GManager.instance.DeadForce();
                        isDead = true;
                    }
                }
            }
            else
            {
                timeCount -= Time.deltaTime; //経過時間を減算
                TimePlus(); //時間回復処理

                //20秒を上限にする
                if(timeCount >= 20f)
                {
                    timeCount = 20f;
                }

                //残り時間が10秒を超えている場合スコアにブーストがかかる
                if(timeCount >= 10f)
                {
                    GManager.instance.scoreBoost = timeCount * 0.1f;
                    timerText.color = Color.magenta; //文字色をマゼンタに
                    GManager.instance.uic.boostUI.SetActive(true);
                }
                else
                {
                    GManager.instance.scoreBoost = 1f;
                    timerText.color = Color.red; //文字色を赤に
                    GManager.instance.uic.boostUI.SetActive(false);
                }

                timerText.text = timeCount.ToString("f2"); //残り時間を表示
            }
        }
        else
        {
            if(!isResult)
            {
                if (countGOver >= 1f)
                {
                    GManager.instance.uic.ResultActive(); //リザルト表示
                    isResult = true;
                }

                countGOver += Time.unscaledDeltaTime; //タイムスケール0状態でも時間をカウントする
            }
        }
    }

    /// <summary>
    /// リザルト遷移中処理
    /// </summary>
    private void PlayOut()
    {
        //時間切れ、HP切れそれぞれでシフトするためメソッドにまとめる

        timerText.text = "GAME OVER";
        GManager.instance.uic.dScreenUI.SetActive(true);

        Time.timeScale = 0; //ゲームの時間を止める

        isGOver = true;
    }

    /// <summary>
    /// 時間の加算処理
    /// </summary>
    private void TimePlus()
    {
        timeCount += GManager.instance.timeAd; //時間を回復
        GManager.instance.timeAd = 0; //時間回復量をリセット
    }
}
