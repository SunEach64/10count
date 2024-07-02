using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy2 : MonoBehaviour
{
    [Header("重力値")] public float graSpeed;
    [Header("消滅距離")] public float desDis;
    [Header("接地判定")] public GroundCheck gc;
    [Header("死亡SE")] public AudioClip seDead;
    [Header("血しぶき")] public GameObject bloodEf;

    private EnemyParameter ep;
    private Rigidbody rb; //リジッドボディ取得用
    private NavMeshAgent agent; //この敵のナビメッシュエージェント
    private Transform target; //ナビメッシュ制御の移動先
    private bool gCheck; //接地判定
    private bool modeA = false; //状態フラグ：攻撃
    private bool deadShift; //死亡時初回挙動
    private bool isTarg; //ターゲット取得状態フラグ
    private float timeCount; //前進/死亡時間測定用
    private float nextTime; //次のターゲット再取得までの時間(ランダム設定する)
    private Vector3 hitPos; //弾丸の接触座標


    //・巡回モード
    //ランダムに動き続ける
    //被弾やプレイヤー視認によって攻撃モードへ以降
    //・攻撃モード
    //5秒間プレイヤーの位置を無条件で把握し、そこへ移動する(接触によりプレイヤーはダメージを受ける)
    //5秒経過時に索敵範囲にプレイヤーがいなければ、その場で？秒間待機、その後巡回or待機モードに以降


    void Start()
    {
        ep = GetComponent<EnemyParameter>();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        target = GManager.instance.player.transform;

        rb.isKinematic = true;
        modeA = true; //攻撃モードへ
    }

    void FixedUpdate()
    {
        if(!ep._knockBack)
        {
            //フォルスの時は何もなし
        }
        else
        {
            KnockBack(); //ノックバックメソッド

            ep._knockBack = false; //重複させないためリセット
        }
    }

    void Update()
    {
        if (!ep._death)
        {
            gCheck = gc.IsGround(); //接地判定

            if (gCheck) //接地判定中
            {
                if (modeA)
                {
                    MoveA();
                }
                else
                {
                    //動かない(停止モード)
                }
            }
            else //空中挙動
            {
                transform.position -= transform.up; //下に動かす(落ちる)
            }

        }
        else
        {
            if(!deadShift)
            {
                deadShift = true; //死亡時初回挙動を終了
                rb.isKinematic = false; //物理演算復活
                agent.enabled = false; //ナビメッシュエージェントを無効
                timeCount = 0; //カウントリセット(timeCount変数は移動時と死亡時で共有しているため)
                rb.constraints = RigidbodyConstraints.None; //オブジェクトの回転制限を無効(倒れる)
                SoundManager.instance.PlaySE(seDead);
            }
            else
            {
                Dead(); //死亡時挙動
                rb.velocity = new Vector3(0, -graSpeed, 0); //重力挙動のみ
            }
        }
    }


    /// <summary>
    /// モードA挙動
    /// </summary>
    private void MoveA()
    {
        if (!isTarg)
        {
            //ターゲット取得モード
            target = GManager.instance.pTracer.transform; //ターゲット指定
            agent.SetDestination(target.position); //ターゲット座標を取得
            nextTime = Random.Range(0.1f, 0.5f); //次の再取得時間設定
            isTarg = true;

            //プレイヤーとの距離を判定
            if(Vector3.Distance(this.transform.position, target.position) > desDis)
            {
                Debug.Log("自動消滅");
                Destroy(this.gameObject);
            }
        }
        else
        {
            //ターゲットへの移動モード
            if (timeCount >= nextTime) //再取得時間分移動した
            {
                isTarg = false; //ターゲット再取得へ
                timeCount = 0; //カウントリセット

                if (GManager.instance.pDeath) //プレイヤーの死亡判定
                {
                    modeA = false; //攻撃モードをリセット
                    agent.ResetPath(); //経路リセット
                }
            }

            timeCount += Time.deltaTime; //経過時間加算
        }

    }

    /// <summary>
    /// 死亡時挙動
    /// </summary>
    private void Dead()
    {
        if (timeCount <= 5.0f) //カウント5秒まで
        {
            timeCount += Time.deltaTime; //経過時間加算
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// ノックバック挙動
    /// </summary>
    private void KnockBack()
    {
        //ノックバックのコルーチンを開始
        StartCoroutine(KnockbackRoutine(ep.kbDir.normalized, ep.kbForce, ep.kbTime));
    }

    /// <summary>
    /// ノックバックコルーチン
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="force"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    private IEnumerator KnockbackRoutine(Vector3 direction, float force, float duration)
    {
        // NavMeshAgentを一時的に無効化
        agent.enabled = false;

        // RigidbodyをKinematicから解除
        rb.isKinematic = false;

        // ノックバックの力を加える
        rb.AddForce(direction * force, ForceMode.Impulse);

        // 指定された期間ノックバックを続ける
        yield return new WaitForSeconds(duration);

        if(!ep._death) //ノックバック終了時に死亡していなければ
        {
            // Rigidbodyを再びKinematicに設定
            rb.isKinematic = true;

            // NavMeshAgentを再度有効化
            agent.enabled = true;
        }
    }

    /// <summary>
    /// 接触時
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet") //被弾時
        {
            hitPos = collision.contacts[0].point; //接触座標を取得
            Instantiate(bloodEf, this.transform.position, Quaternion.LookRotation(hitPos), this.transform); //血しぶき生成
        }
        else if (collision.gameObject.tag == "Player") //攻撃時
        {
            GManager.instance.EAtkCange(1f); //将来的にはエネミーパラメータ側で処理？
        }

    }
}
