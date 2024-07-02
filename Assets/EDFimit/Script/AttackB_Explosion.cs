using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//爆発本体
//特殊攻撃用の弾が接触すると生成される

public class AttackB_Explosion : MonoBehaviour
{
    [SerializeField] S_AttackData aData;

    private SphereCollider sCol;
    private float timer;

    void Start()
    {
        sCol = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        //時間経過でコライダーをオフ→デストロイ
        if (timer >= 3f)
        {
            Destroy(gameObject);
        }
        else if (timer >= 0.5f)
        {
            sCol.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            // SphereColliderの中心をワールド座標で取得
            Vector3 sphereCenterWorldPosition = transform.TransformPoint(sCol.center);

            // 接触したオブジェクトの位置を取得
            Vector3 otherPosition = other.transform.position;

            // SphereColliderの中心から接触したオブジェクトへのノックバックベクトルを計算
            Vector3 directionVector = otherPosition - sphereCenterWorldPosition;

            EnemyParameter ep = other.GetComponent<EnemyParameter>(); //接触した敵のエネミーパラメータを取得
            ep.DamageCount(aData.atk, aData.kbForce, aData.kbTime, directionVector); //ダメージ処理を呼び出し(ダメージ、ノックバック力、ノックバック時間, ノックバックベクトル)

            sCol.enabled = false;
        }
    }
}
