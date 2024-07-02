using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//リザルト画面での半透明赤スクリーン

public class UI_RedScreen : MonoBehaviour
{
    [Header("マテリアル")] public Material rs;

    private float alpha; //マテリアルのアルファ値(透過率)

    void Start()
    {
        
    }

    void Update()
    {
        if (alpha >= 0.7f)
        {
            rs.SetColor("_Color", new Color(0.2f, 0, 0, 0.7f));
        }
        else
        {
            alpha += 0.2f * Time.deltaTime; //時間経過でアルファ値を上げていく

            rs.SetColor("_Color", new Color(0.2f, 0, 0, alpha));
        }


    }
}
