using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ゲームスタート時、黒の半透明画面を時間経過で透明化する

public class UI_DarkScreen : MonoBehaviour
{
    [Header("マテリアル")] public Material ds;

    private float alpha; //マテリアルのアルファ値(透過率)
    private bool isStart; //初回挙動フラグ

    void Start()
    {
        
    }

    void Update()
    {
        if (!isStart) //初回挙動
        {
            isStart = true;
            alpha = 0.5f; //半透明に設定
        }
        else //演出挙動
        {
            alpha -= Time.deltaTime; //時間経過でアルファ値を下げていく

            ds.SetColor("_BaseColor", new Color(0, 0, 0, alpha));

            if (alpha <= 0)
            {
                isStart = false; //初回挙動フラグをリセット
                GManager.instance.uic.DarkScreenOut(); //非表示処理
            }
        }
        
    }
}
