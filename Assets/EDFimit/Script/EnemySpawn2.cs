using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//敵スポナー
//画面外のランダム位置に生成
//ボスもこれで出すようにしてしまう

public class EnemySpawn2 : MonoBehaviour
{
    [Header("雑魚敵プレハブ")] public GameObject enemy;
    [Header("ボス敵プレハブ")] public GameObject boss;
    [Header("スポーンポイント")] public GameObject[] esp;
    [Header("敵数上限")] public int enLimit = 50; //デフォルト50とする

    private GameObject[] tagObj; //敵の数カウント用
    private GameObject spnEnemy; //出現させる敵キャラ(条件に応じて雑魚敵とボス敵を切替)
    private int enQty; //敵の数
    private int num; //スポーンポイントの番号
    private float interval; //再出現までのインターバル
    private float scoreNow; //現在スコア(Gマネージャーから取得)
    private float nextSpawn; //次のボススポーンスコア

    void Start()
    {
        nextSpawn = 1000f; //一体目のボスは1000スコアで出現とする
    }

    void Update()
    {

        transform.position = GManager.instance.pTracer.transform.position; //スポナーの位置更新
        //スポナーの位置はPトレーサー(一緒に回転するのを避けるためプレイヤーの子にはしない)

        scoreNow = GManager.instance.score; //Gマネージャーから現在スコアを取得

        if (scoreNow >= nextSpawn) //ボス出現スコアに達しているかを判定
        {
            spnEnemy = boss; //出現敵をボスに設定
        }
        else
        {
            spnEnemy = enemy; //出現敵を雑魚敵に設定
        }

        if (interval >= 0.5f)
        {
            QtyCheck("Enemy"); //Enemyタグの数を判定

            if(enQty <= enLimit) //上限未到達
            {
                ESpawn(); //敵を追加スポーン
            }
        }
        else
        {
            interval += Time.deltaTime;
        }

    }

    private void ESpawn()
    {
        //スポーンポイントを決定
        num = Random.Range(0, esp.Length);
        Vector3 espPos = esp[num].transform.position;

        //スポーンポイントにコライダーがあるかを判定(判定は半径2.5の球体)
        //判定位置が低いと床のコライダーに当たるため高めにする
        if (Physics.OverlapSphere(new Vector3(espPos.x, 2.6f, espPos.z), 2.5f).Length > 0) 
        {
            //生成しない
        }
        else
        {
            //敵を生成
            Instantiate(spnEnemy, new Vector3(espPos.x, 1.1f, espPos.z), Quaternion.identity);
            interval = 0; //生成出来た時だけタイマーをリセット

            if(spnEnemy == boss) //出現した敵がボスだった場合
            {
                GManager.instance.uic.ArertOn(); //アラートメッセージON

                nextSpawn += 1000f; //次のボス出現は+1000(暫定)
            }
        }
    }

    /// <summary>
    /// エネミー数確認
    /// </summary>
    private void QtyCheck(string tagname)
    {
        tagObj = GameObject.FindGameObjectsWithTag(tagname); //tagnameに入れたタグ名のオブジェクトを探す

        enQty = tagObj.Length; //数えた敵数
    }
}
