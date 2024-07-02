using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

//Vcamを揺らす処理
//主にダメージ時の画面振動

public class Cam_Shaker : MonoBehaviour
{
    private CinemachineImpulseSource impulse;

    void Start()
    {
        impulse = GetComponent<CinemachineImpulseSource>();
    }

    /// <summary>
    /// 振動実行
    /// </summary>
    public void ShakeOn()
    {
        //Gマネージャーから呼び出し
        impulse.GenerateImpulse();
    }
}
