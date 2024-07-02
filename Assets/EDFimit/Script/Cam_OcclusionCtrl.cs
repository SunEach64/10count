using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//カメラからプレイヤーへ追従する壁透明化トリガー

public class Cam_OcclusionCtrl : MonoBehaviour
{
    [Header("カメラ")] public GameObject cam;
    [Header("ターゲット")] public GameObject target; //プレイヤーかPトレーサー？


    void FixedUpdate()
    {
        this.gameObject.transform.position = cam.transform.position;
        //this.transform.LookAt(target.transform.position);
    }

}
