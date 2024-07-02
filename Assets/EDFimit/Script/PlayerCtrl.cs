using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

//プレイヤーキャラの操作
//見下ろし視点でマウスカーソル方向を向き、Q/Eキーで視点を回転、WASDで移動

public class PlayerCtrl : MonoBehaviour
{
    [Header("MainUnit")] public GameObject mu;
    [Header("HP")] public float hp; //キャラクター別の基礎最大HP値
    [Header("移動速度")] public float runSpeed;
    [Header("ダッシュ力")][Range(0, 50)] public float dashSpeed;
    [Header("ジャンプ力")] public float jumpSpeed;
    [Header("ジャンプ横加速")] public float jumpAccel; //ジャンプ時の横方向への加速補正
    [Header("ジャンプカーブ")] public AnimationCurve jumpCurve;
    [Header("ジャンプ中回転値上限")] public float jumpSpin;
    [Header("重力値")] public float graSpeed;
    [Header("見下ろし時向き基準")] public GameObject lookPosN0;
    [Header("メインカメラ")] public GameObject mainCam;
    [Header("接地判定")] public GroundCheck gc;
    [Header("ジャンプSE")] public AudioClip seJump;
    [Header("被弾SE")] public AudioClip seDamage;
    [Header("死亡SE")] public AudioClip seDead;
    [Header("死亡エフェクト")] public GameObject efDead;

    [HideInInspector] public bool isDash; //ダッシュ中判定：Pトレーサーから読み取らせるためパブリックにする

    private Rigidbody rb; //リジッドボディ取得用
    private GameObject hitEnemy; //接触した敵	
    private Vector3 moveVel; //移動速度値
    private Vector3 wsVel; //前後移動値
    private Vector3 adVel; //左右移動値
    private Vector3 vVel; //垂直移動量(ジャンプ上昇値と重力落下値の合計)
    private Vector3 jumpSpinVel; //ジャンプ中回転量
    private Vector3 dashSpinVel; //ダッシュ時回転
    private Quaternion dashSpin; //ダッシュ時の回転を行うクォータニオン
    private float dashPow; //ダッシュ力2
    private float dashTime; //ダッシュ時間
    private float jumpTime; //ジャンプ時間
    private float kbTime; //ノックバック時間
    private float deathTime; //死んでからリザルトに映るまでのタイムラグ
    private bool gCheck; //接地判定
    private bool isKnockback = false; //ノックバック判定
    private bool isDamage = false; //ダメージ判定(HP減少処理)
    private bool isDeath; //死亡判定
    private bool isJump; //ジャンプ中判定
    private bool isJumpDown; //ジャンプ後落下判定
    private bool dashAf; //ダッシュ後から再ダッシュ可能の判定(falseの時にダッシュ可能)
    private int layerP; //プレイヤーのレイヤー
    private int layerE; //敵のレイヤー

   
    void Start()
    {
        //コンポーネント取得
        rb = GetComponent<Rigidbody>();
        //GetComponent<Rigidbody>().maxAngularVelocity = 100f;

        GManager.instance.hpBase = hp; //Gマネージャーの最大HPベース値変数にこのキャラのHP値を入れる
        GManager.instance.HPActive(); //HPを設定
        layerP = LayerMask.NameToLayer("Player"); //プレイヤーのレイヤー取得
        layerE = LayerMask.NameToLayer("Enemy"); //エネミーのレイヤー取得
        //layerP = 8;
        //layerE = 9;
    }
    
    void FixedUpdate()
    {
        gCheck = gc.IsGround(); //接地状態取得

        if (!isDeath && GManager.instance.hpNow > 0) //死亡判定がファルス且つHPが0ではない
        {
            if (gCheck && !isKnockback) //接地判定トゥルー、且つノックバック判定ファルス(通常時挙動)
            {
                jumpAccel = 1f; //ジャンプ時以外は加速補正を切る

                if(isJumpDown) //ジャンプ着地後に接地
                {
                    isJumpDown = false; //落下後は回転を切る
                    dashPow = 0; //空中でダッシュが切れると加速が残る場合があるためここでもリセットを掛ける
                }
                

                if (Input.GetButton("Jump")) //ジャンプする
                {
                    //ジャンプフラグをトゥルー
                    //上への加速を取得(ベクター3)
                    //※ジャンプ後は接地判定が離れるため入力も自然と切れる

                    if (!isJump)
                    {
                        SoundManager.instance.PlaySE(seJump); //効果音再生(重複を避ける)
                    }

                    vVel = new Vector3(0, jumpSpeed, 0); //ジャンプ加速
                    isJump = true; //ジャンプ中判定トゥルー

                    //ここでジャンプ中のランダム回転量を決定
                    jumpSpinVel.x = Random.Range(0, jumpSpin);
                    jumpSpinVel.y = Random.Range(0, jumpSpin);
                    jumpSpinVel.z = Random.Range(0, jumpSpin);
                    jumpSpinVel = jumpSpinVel.normalized;
                    jumpSpinVel.x *= 2f; //縦回転を大きめに

                    
                }
                else //ジャンプしない
                {
                    if(!isDash) //ダッシュ中でない
                    {
                        MoveCtrl3(); //移動処理
                        vVel = Vector3.zero; //落下OFF

                        //ダッシュ挙動
                        if (!dashAf && Input.GetKey(KeyCode.LeftShift)) //再ダッシュ可能時にShiftキーを入力
                        {
                            isDash = true;
                            dashPow = dashSpeed; //ダッシュ速度補正を入れる
                            dashSpinVel = Quaternion.Euler(0, 90, 0) * moveVel; //進行方向moveVelのY軸90度回転をダッシュ時回転軸とする
                            SoundManager.instance.PlaySE(seJump); //効果音再生
                            Physics.IgnoreLayerCollision(layerP, layerE, true); //衝突判定を無視(無敵状態)
                        }
                    }
                    else //ダッシュ中
                    {
                        DashCtrl(); //ダッシュ挙動
                    }
                }
            }
            else
            {
                if (isKnockback) //ノックバック時(空中で食らっても動作する)
                {
                    KBackCtrl();
                    vVel = Vector3.zero; //落下OFF
                }
                else if (!gCheck) //空中時、ジャンプ中含む
                {
                    if(isJump) //ジャンプ中
                    {
                        if (jumpTime >= 0.5f) //ジャンプ上昇時間は暫定0.5秒
                        {
                            jumpTime = 0; //ジャンプ上昇時間リセット
                            isJump = false; //落下にシフト
                            isJumpDown = true;
                            isDash = false; //慣性ジャンプ中はダッシュ判定もリセット
                        }

                        vVel *= jumpCurve.Evaluate(jumpTime);
                        jumpTime += Time.deltaTime;
                    }
                    else //落下中
                    {
                        //※ダッシュ中は落ちない
                        if (!isDash)
                        {
                            vVel = new Vector3(0, -graSpeed, 0); //落下
                        }
                        else
                        {
                            DashCtrl(); //ダッシュ挙動
                        }
                    }
                }
            }

            rb.velocity = (moveVel * ((runSpeed + dashPow) * jumpAccel)) + vVel; //最終的な移動速度決定
            //※現状jumpAccelは使ってない
        }
        else //死亡時は重力挙動のみ
        {
            if (isDeath) //死亡時処理
            {
                if (deathTime >= 2f) //死んで2秒経ったら
                {
                    GManager.instance.pDeath = true; //Gマネージャーに死亡判定を送る
                }

                vVel = new Vector3(0, -graSpeed, 0);

                deathTime += Time.deltaTime; //死んでからの時間カウント
            }
            else //死亡判定が出ていないがHPが0の時
            {
                Death(); //死亡する
            }

            rb.velocity = (moveVel * ((runSpeed + dashPow) * jumpAccel)) + vVel; //追加の移動はできないが慣性は残す
        }
    }

    void Update()
    {
        if(!isDeath)
        {
            if (gCheck && !isDash)
            {

                DirCtrl3(); //地上での向き処理

                if (dashAf) //ダッシュ後処理
                {
                    if(!Input.GetKey(KeyCode.LeftShift)) //Shiftキーを離した
                    {
                        dashAf = false;
                    }
                }
            }
            else
            {
                if(isKnockback) //ノックバック時は回転を止める
                {

                }
                else
                {
                    if (isJump || isJumpDown) //ジャンプ中回転
                    {
                        mu.transform.Rotate(jumpSpinVel * 720f * Time.deltaTime, Space.World);
                    }
                    else if (isDash) //ダッシュ中回転
                    {
                        mu.transform.Rotate(dashSpinVel * 720f * Time.deltaTime, Space.World); //1秒で720度回転(0.5秒で1回転)
                    }
                }
                
            }
                
        }
        else
        {
            
        }
    }


    /// <summary>
    /// 移動制御v3
    /// </summary>
    private void MoveCtrl3()
    {
        //※方向補正なしの単純移動
        float ad = Input.GetAxis("Horizontal"); //横入力
        float ws = Input.GetAxis("Vertical"); //縦入力

        wsVel = new Vector3(0, 0, ws); //前後移動値の基準値
        adVel = new Vector3(ad, 0, 0); //左右移動値の基準値

        //操作方向をメインカメラの平面投影ローカルに変換
        Transform lookA = mainCam.transform; //基準Aを定義
        lookA.localPosition = new Vector3(lookA.localPosition.x, this.transform.position.y, lookA.localPosition.z); //基準Aをプレイヤーと同じ高さに変更
        Vector3 lookB = lookPosN0.transform.position; //基準Bを定義
        lookB.y = this.transform.position.y; //基準Bをプレイヤーと同じ高さに変更
        lookA.transform.LookAt(lookB); //基準Aを基準Bに向ける(操作方向ローカル基準)
        moveVel = lookA.TransformDirection(wsVel + adVel).normalized; //前後移動値と左右移動値を合体、且つメインカメラの平面投影ローカル方向に変換(且つノーマライズ)
    }

    /// <summary>
    /// ダッシュ制御
    /// </summary>
    private void DashCtrl()
    {
        if (dashTime >= 0.5f) //ダッシュ時間は暫定0.5秒
        {
            isDash = false; //ダッシュ中判定リセット
            dashPow = 0f; //ダッシュ加速をリセット
            dashTime = 0f; //ダッシュ時間リセット
            dashAf = true; //ダッシュ後判定開始
            Physics.IgnoreLayerCollision(layerP, layerE, false); //衝突判定を復活
        }
        else
        {
            dashTime += Time.deltaTime; //ダッシュ時間更新
        }
    }

    /// <summary>
    /// 向き制御v3
    /// </summary>
    private void DirCtrl3()
    {
        Vector3 dir = GManager.instance.cursorPos.position - this.transform.position;
        dir.y = 0;

        mu.transform.rotation = Quaternion.LookRotation(dir, Vector3.up); //Y軸回転でカーソル方向を向く
    }

    /// <summary>
    /// 被弾時制御
    /// </summary>
    private void KBackCtrl()
    {
        kbTime += Time.deltaTime; //経過時間加算

        if (kbTime <= 0.5f) //カウント0.5秒まで
        {
            if(!isDamage)
            {
                isDamage = true; //ダメージ処理を追加で行わないようにする(ノックバック後に解除)
                HPDown(); //HP減少処理

                if (GManager.instance.hpNow <= 0) //HPが0を下回った時
                {
                    Death(); //死亡処理
                }
            }
        }
        else //0.5秒経過
        {
            kbTime = 0f; //カウントリセット
            isKnockback = false; //ノックバック判定リセット
            isDamage = false; //ダメージ判定リセット
        }
            
        //ノックバック中は他の敵に接触しても追加ダメージを受けない
        //当たった相手の相対方向を取得し、反対側にノックバックするようにする
    }

    /// <summary>
    /// HP減少処理
    /// </summary>
    private void HPDown()
    {
        GManager.instance.PHPDown(); //HP減少処理
        GManager.instance.isDam = false; //Gマネージャーの被弾処理中判定をリセット
    }


    /// <summary>
    /// 死亡処理
    /// </summary>
    private void Death()
    {
        isDeath = true; //死亡判定をトゥルーに
        SoundManager.instance.PlaySE(seDead);
        Instantiate(efDead, this.transform.position, Quaternion.identity, this.transform); //死亡時血しぶきエフェクト発生
        rb.constraints = RigidbodyConstraints.None; //オブジェクトの回転制限を無効(倒れる)
    }

    /// <summary>
    /// 被弾時判定
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if(!isKnockback) //ノックバック中でない時
        {
            if (collision.gameObject.tag == "Enemy")
            {
                isKnockback = true; //ノックバック判定をトゥルー
                hitEnemy = collision.gameObject; //ぶつかった相手を保存
                moveVel = transform.position - hitEnemy.transform.position; //ノックバック方向を決定
                moveVel = new Vector3(moveVel.x, 0, moveVel.z).normalized; //ノックバック方向を水平方向でノーマライズ
                SoundManager.instance.PlaySE(seDamage);
            }
        }
        
    }
}
