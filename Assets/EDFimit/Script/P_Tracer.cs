using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Tracer : MonoBehaviour
{
    [Header("PlayerCtrl")] public PlayerCtrl pc;
    [Header("追従プレイヤー")] public Transform target;

    private Vector3 cPos; //遅延追従中のPトレーサー座標
    private Vector3 cVel; //currentVelocity(追従速度を入れる)
    private float smTime = 0.2f; //ターゲットへの凡その到達時間(変動させる)
    private float dafTime; //ダッシュ後時間
    private bool gCheck;
    private bool dashAf; //ダッシュ後判定

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(pc.isDash) //ダッシュ中
        {
            dashAf = true;

            cPos = this.transform.position; //現在座標を保存
            cPos = Vector3.SmoothDamp(cPos, target.position, ref cVel, smTime); //遅延追従の値を計算

            this.transform.position = new Vector3(cPos.x, 1.05f, cPos.z);
        }
        else 
        {
            if(dashAf) //ダッシュ後
            {
                if(dafTime >= smTime)
                {
                    //※遅延時間を0.2に設定しているため、ダッシュ後0.2秒経ちカメラがプレイヤーに追いついたらダッシュ後処理を終える
                    dashAf = false;
                    dafTime = 0f;
                }
                else //ダッシュ後も遅延追従継続
                {
                    dashAf = true;

                    cPos = this.transform.position; //現在座標を保存
                    cPos = Vector3.SmoothDamp(cPos, target.position, ref cVel, smTime); //遅延追従の値を計算

                    this.transform.position = new Vector3(cPos.x, 1.05f, cPos.z);
                }
            }
            else //通常移動時
            {
                this.transform.position = new Vector3(target.position.x, 1.05f, target.position.z);
            }
        }
        
    }
}
