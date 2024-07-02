using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//全ての敵に共通でアタッチする、情報のやり取り用スクリプト
//スクリプタブルオブジェクトもこれを経由して扱う

public class EnemyParameter : MonoBehaviour
{
    [HideInInspector] public float hp; //現在HP
    [HideInInspector] public float kbForce; //バレットから取得
    [HideInInspector] public float kbTime; //バレットから取得
    [HideInInspector] public Vector3 kbDir; // //バレットから取得
    [HideInInspector] public bool _death; //死亡判定
    [HideInInspector] public bool _knockBack; //ノックバック判定

    [SerializeField] S_EnemyData eData; //スクリプタブルオブジェクト
    

    void Start()
    {
        hp = eData.hp; //HPを設定
    }

    void Update()
    {
        
    }

    /// <summary>
    /// ダメージ処理
    /// </summary>
    /// <param name="dam"></param>
    public void DamageCount(float dam, float kbF, float kbT, Vector3 kbD)
    {
        //受けた攻撃(バレット等)からダメージやノックバック情報を受け取り、HP減少処理とスコア処理を行い、本体スクリプトへノックバック情報を送る

        //HP減少処理
        hp -= dam;

        //死亡時追加処理
        if (hp <= 0)
        {
            _death = true; //エネミー本体側スクリプトが読み取るための死亡判定
            GManager.instance.ScoreCount(eData.score); //スコア加算呼び出し
            GManager.instance.ec.CountPuls(eData.num);//撃破カウント加算呼び出し
        }

        //ノックバック処理(生死により挙動を変えるため一応_deathの判定より後に入れる)
        kbForce = kbF;
        kbTime = kbT;
        kbDir = kbD;
        _knockBack = true;

        //※ボスはノックバックなし(本体スクリプト側にノックバックメソッドを入れなければ勝手に無視される)
    }
}
