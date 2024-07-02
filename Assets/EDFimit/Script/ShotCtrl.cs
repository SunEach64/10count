using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//左手にアタッチする、弾の発射用

public class ShotCtrl : MonoBehaviour
{
    [Header("銃口位置")] public Transform firePos;
    [Header("発射方向")] public Transform shotDir;
    [Header("射撃SE")] public AudioClip shotSE;
    [Header("弾丸")] public GameObject bullet;
    [Header("連射速度")] public float shotInt;
    [Header("マズルフラッシュ")] public ParticleSystem psMF;

    private bool isReShot = true; //再射撃可能判定
    private float reShotTime = 0.0f; //射撃後経過時間
    private Vector3 shotDir2; //発射方向(補正)

    // Start is called before the first frame update
    void Start()
    {
        psMF.Stop(); //開始時はパーティクルシステムを止めておく
    }

    void Update()
    {
        if(!GManager.instance.noShot) //noShotがfalseの時
        {
            //銃口をカーソル方向に向ける
            transform.LookAt(GManager.instance.aimPos);

            //射撃制御
            if (isReShot) //再射撃可能である
            {
                if (Input.GetButton("Fire1")) //射撃コマンドが入力された
                {
                    psMF.Play(); //マズルフラッシュ再生
                    SoundManager.instance.PlaySE(shotSE); //射撃SEを鳴らす

                    shotDir2 = new Vector3(shotDir.eulerAngles.x + 90f, shotDir.eulerAngles.y, shotDir.eulerAngles.z);
                    Instantiate(bullet, firePos.position, Quaternion.Euler(shotDir2)); //弾オブジェクトが出現

                    isReShot = false;

                }
            }
            else //射撃後インターバル中
            {
                if (reShotTime > shotInt) //射撃後経過時間が設定された連射間隔時間に達した
                {
                    reShotTime = 0.0f; //タイマーリセット
                    isReShot = true; //再射撃可能に
                }
                else
                {
                    reShotTime += Time.deltaTime; //経過時間を加算
                }
            }
        }
    }
}
