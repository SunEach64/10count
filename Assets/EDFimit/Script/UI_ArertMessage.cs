using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ArertMessage : MonoBehaviour
{
    [Header("枠")] public Image frame;
    [Header("RectTransform")] public RectTransform rt;

    private int blinkCount; //点滅回数
    private float scaleY; //アラートメッセージ出現時のYスケール
    private float al = 1f; //フレーム明滅時のアルファ値(1から開始)
    private bool alUp; //明滅中のフラグ切替用
    private bool ef1; //演出1：出現
    private bool ef2; //演出2：明滅
    private bool ef3; //演出3：消滅

    void Start()
    {
        ef1 = true; //演出1開始
    }
    void Update()
    {
        if(ef1)
        {
            ArertEf1();
        }
        else if(ef2)
        {
            ArertEf2();
        }
        else if(ef3)
        {
            ArertEf3();
        }
    }

    /// <summary>
    /// メッセージ出現演出
    /// </summary>
    private void ArertEf1()
    {
        if(scaleY >= 1)
        {
            scaleY = 1;

            //フラグ切替
            ef1 = false;
            ef2 = true;
        }
        else
        {
            scaleY += Time.deltaTime * 2; //Yスケールを上げる
        }

        rt.localScale = new Vector3(1, scaleY, 1); //サイズ可変
    }

    /// <summary>
    /// フレーム明滅演出
    /// </summary>
    private void ArertEf2()
    {
        if(blinkCount < 2) //2回点滅するまで
        {
            if(!alUp)
            {
                al -= Time.deltaTime * 2; //アルファ値を下げる

                if (al <= 0.2f) //アルファ値が0.2を下回った
                {
                    alUp = true; //フラグ切替
                }
            }
            else 
            {
                al += Time.deltaTime * 2; //アルファ値を上げる

                if(al >= 1f) //アルファ値が1に達した
                {
                    alUp = false; //フラグ切替
                    blinkCount += 1; //明滅カウント+1
                }
            }
        }
        else
        {
            al = 1;

            //フラグ切替
            ef2 = false;
            ef3 = true;
            blinkCount = 0; //カウントリセット
        }
        
        frame.color = new Color(1, 0, 0, al); //透過度を可変
    }


    private void ArertEf3()
    {
        if (scaleY <= 0)
        {
            scaleY = 0;

            //フラグ切替
            ef3 = false;
            ef1 = true;

            //Gマネージャーからセットアクティブを呼び出す
            GManager.instance.uic.ArertOff();
        }
        else
        {
            scaleY -= Time.deltaTime * 2; //Yスケールを下げる
        }

        rt.localScale = new Vector3(1, scaleY, 1); //サイズ可変
    }
}
