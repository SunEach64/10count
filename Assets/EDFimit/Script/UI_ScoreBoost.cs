using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//残り時間に応じたスコア増加率の表示
//Gマネージャーのスコアブースト変数がそのまま利用できる
//タイマーに残り時間10秒以上状態を判定させ、Gマネージャー経由でこのスクリプトがアタッチされたオブジェクトをアクティブにする

public class UI_ScoreBoost : MonoBehaviour
{
    private TMP_Text bt;

    void Start()
    {
        bt = GetComponent<TMP_Text>();
    }

    void Update()
    {
        bt.text = "ScoreBoost " + (GManager.instance.scoreBoost * 100).ToString("f0") + "%";
    }
}
