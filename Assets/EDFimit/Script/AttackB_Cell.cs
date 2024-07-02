using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//サブアタック用の弾(原則共通)
//挙動は概ねバレットに準じた直進

public class AttackB_Cell : MonoBehaviour
{
    [Header("生成サブアタック")] public GameObject subAttack;

    private Rigidbody rb = null; //リジッドボディ取得
    private Vector3 shotVec = Vector3.zero; //弾の進行方向

    [SerializeField] S_AttackData aData; //基本的には生成されるサブアタック本体と同じスクリプタブルオブジェクトを使い回す

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        shotVec = transform.up; //発射時点での正面を保存(弾オブジェクトが縦基準のため上向き)
    }

    void FixedUpdate()
    {
        rb.velocity = shotVec * aData.speed; //進行方向へリジッドボディ加速
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //プレイヤーだけはレイヤーで非接触にし、他は敵でも建物でも接触したら作動
        Instantiate(subAttack, this.transform.position, Quaternion.identity);

        Destroy(this.gameObject);
    }
}
