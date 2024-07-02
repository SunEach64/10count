using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("着弾エフェクト")] public GameObject hitEf;

    private Rigidbody rb = null; //リジッドボディ取得
    private Vector3 shotVec = Vector3.zero; //弾の進行方向

    [SerializeField] S_AttackData bulletData;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        shotVec = transform.up; //発射時点での正面を保存(弾オブジェクトが縦基準のため上向き)
    }

    private void FixedUpdate()
    {
        rb.velocity = shotVec * bulletData.speed; //進行方向へリジッドボディ加速
    }

    /// <summary>
    /// 着弾時
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            EnemyParameter ep = collision.gameObject.GetComponent<EnemyParameter>(); //接触した敵のエネミーパラメータを取得

            Vector3 kbDirection = collision.contacts[0].point - transform.position; //ノックバックベクトルを計算
            ep.DamageCount(bulletData.atk, bulletData.kbForce, bulletData.kbTime, kbDirection); //ダメージ処理を呼び出し(ダメージ、ノックバック力、ノックバック時間, ノックバックベクトル)
        }

        Instantiate(hitEf, this.transform.position, Quaternion.identity); //着弾エフェクト生成(細かい座標は要調整)
        Destroy(this.gameObject); //消滅
    }


    /// <summary>
    /// 画面外に出たら消える
    /// </summary>
    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
