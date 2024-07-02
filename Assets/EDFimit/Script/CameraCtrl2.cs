using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

//指定オブジェクトの座標を基準にカメラを旋回させる

public class CameraCtrl2 : MonoBehaviour
{
    [Header("回転速度")] public float rollSpeed = 100f;

    [SerializeField] private CinemachineVirtualCamera cvc; //バーチャルカメラを入れる変数

    private float rotRL = 0; //回転方向
    private float angBf; //回転前角度
    private float angAf; //回転後角度
    private bool isRoll; //回転中フラグ

    CinemachineOrbitalTransposer cot; //オービタルトランスポーザーを制御する変数

    void Start()
    {
        cot = cvc.GetCinemachineComponent<CinemachineOrbitalTransposer>(); //オービタルトランスポーザーを取得
        angBf = cot.m_XAxis.Value; //開始時のカメラ角度を保存
    }

    void Update()
    {
        if(!isRoll) //回転中でない時
        {
            if (Input.GetKeyDown(KeyCode.E)) //Eキーを押している
            {
                angAf = angBf + 45; //回転後の角度を決定
                rotRL = 1;
                isRoll = true;

                //回転後角度がValueRangeを超えた時は360を引いてRange内に戻す
                if(angAf >= cot.m_XAxis.m_MaxValue)
                {
                    angAf -= 360;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Q)) //Qキーを押している
            {
                angAf = angBf - 45; //回転後の角度を決定
                rotRL = -1;
                isRoll = true;

                //回転後角度がValueRangeを超えた時は360を足してRange内に戻す
                if(angAf <= cot.m_XAxis.m_MinValue)
                {
                    angAf += 360;
                }
            }
        }
        else //回転中
        {
            float rn = angAf - cot.m_XAxis.Value;

            if(rn >= 1f || rn <= -1f) //回転後との角度差が1以上
            {
                cot.m_XAxis.Value += rotRL * rollSpeed * Time.deltaTime; //回転を進める
            }
            else //回転終了
            {
                cot.m_XAxis.Value = angAf; //回転後角度に確定
                rotRL = 0; //回転方向リセット
                isRoll = false; //回転フラグリセット
                angBf = angAf; //回転前角度を更新
            }
        }
        
    }
}
