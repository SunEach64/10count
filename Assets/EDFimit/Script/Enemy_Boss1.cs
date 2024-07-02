using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Boss1 : MonoBehaviour
{
    [Header("重力値")] public float graSpeed;
    [Header("接地判定")] public GroundCheck gc;
    [Header("索敵判定")] public SightCheck sc;
    [Header("被弾SE")] public AudioClip seDamage;
    [Header("死亡SE")] public AudioClip seDead;
    [Header("血しぶき")] public GameObject bloodEf;

    private EnemyParameter ep;
    private Rigidbody rb; //リジッドボディ取得用
    private NavMeshAgent agent; //この敵のナビメッシュエージェント
    private Transform target; //ナビメッシュ制御の移動先
    private bool gCheck; //接地判定
    private bool sCheck; //索敵判定
    private bool modeA = false; //状態フラグ：攻撃
    private bool deadShift; //死亡時初回挙動
    private bool isTarg; //ターゲット取得状態フラグ
    private float timeCount; //前進/死亡時間測定用
    private float nextTime; //次のターゲット再取得までの時間(ランダム設定する)
    private Vector3 hitPos; //弾丸の接触座標


    void Start()
    {
        ep = GetComponent<EnemyParameter>();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        target = GManager.instance.player.transform;

        rb.isKinematic = true;
        modeA = true; //攻撃モードへ
    }


    void Update()
    {
        if (!ep._death)
        {
            gCheck = gc.IsGround(); //接地判定
            sCheck = sc.IsSight(); //索敵判定

            if (gCheck) //接地判定中
            {
                if(!sCheck)
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
                else
                {
                    ModeD();
                }
            }
            else //空中挙動
            {
                transform.position -= transform.up; //下に動かす(落ちる)
            }

        }
        else
        {
            if (!deadShift)
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
            nextTime = Random.Range(0.3f, 1.0f); //次の再取得時間設定
            isTarg = true;

            //※ボスは距離による自動消滅なし
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
    /// モードD挙動
    /// </summary>
    private void ModeD()
    {
        //誘導のインターバルなし
        target = GManager.instance.pTracer.transform; //ターゲット指定
        agent.SetDestination(target.position); //ターゲット座標を取得
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
    /// 接触時
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet") //被弾時
        {
            hitPos = collision.contacts[0].point; //接触座標を取得
            Instantiate(bloodEf, this.transform.position, Quaternion.LookRotation(hitPos), this.transform); //血しぶき
            SoundManager.instance.PlaySE(seDamage); //被弾SE再生
        }
        else if (collision.gameObject.tag == "Player") //攻撃時
        {
            GManager.instance.EAtkCange(1f); //将来的にはエネミーパラメータ側で処理？
        }

    }
}
