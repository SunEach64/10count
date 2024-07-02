using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ゲームマネージャー

public class GManager : MonoBehaviour
{
    public static GManager instance = null;

    [Header("プレイヤー")] public GameObject player;
    [Header("プレイヤー接地判定")] public GroundCheck gc;
    [Header("Pトレーサー")] public GameObject pTracer;
    [Header("照準カーソル")] public Transform cursorPos;
    [Header("補正有り照準")] public Transform aimPos;
    [Header("MAP中心")] public Transform mapCenter;
    [Header("UI_HP")] public UI_HP uihp;
    [Header("画面振動")] public Cam_Shaker camShake;
    [Header("敵撃破カウント")] public EnemyCount ec; //各エネミーとリザルトが呼び出し
    [Header("UI_Ctrl")] public UI_Ctrl uic;

    [HideInInspector] public bool noShot; //射撃不可判定(死亡時、ポーズ時など)
    [HideInInspector] public bool isDam; //被弾処理中判定
    [HideInInspector] public bool isDam_uihp; //UI_HPへの指令用
    [HideInInspector] public bool pDeath; //死亡判定
    [HideInInspector] public bool isGOver; //ゲームオーバーフラグ(主に外部からの読み取り用)
    [HideInInspector] public float eAtk; //敵攻撃力
    [HideInInspector] public float hpBase; //最大HP基礎値
    [HideInInspector] public float hpMax; //最大HP値
    [HideInInspector] public float hpNow; //現在HP値
    [HideInInspector] public float hpRate; //残HP割合
    [HideInInspector] public float pAtk; //プレイヤー攻撃力
    [HideInInspector] public float score; //撃破スコア
    [HideInInspector] public float timeAd; //時間回復値(スコア獲得時に蓄積され、タイマーが呼び出し値を消費して時間を回復する) 
    [HideInInspector] public float scoreBoost; //スコア倍率(タイマーから補正率を受け取る)
    [HideInInspector] public int destCount; //撃破数


    void Awake()
    {
        //ゲームマネージャーをシングルトンにする
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(this.gameObject);
            //パブリック変数でアタッチしていた各種インスタンスがシーン再ロード時にエラーの原因となっているため、
            //とりあえず今作に関してはゲームマネージャーのシーン跨ぎは諦める(必要なら別なゲームマネージャーを改めて作るが、メモリ消費が増えるため最小限に絞る)
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale = 0; //時間を止める
            noShot = true; //射撃不可
            uic.PauseIn(); //ポーズ画面へ
        }
    }


    /// <summary>
    /// 開始時のHP設定
    /// </summary>
    public void HPActive()
    {
        //HP値を設定(今はまだ基礎値のみ)
        hpNow = hpBase;
        hpMax = hpBase;
    }


    /// <summary>
    /// 被ダメージ時処理諸々
    /// </summary>
    public void PHPDown()
    {
        //プレイヤースクリプトから呼び出される

        hpNow = hpNow - eAtk; //減少処理
        uihp.HPDown(); //UI_HPの減少処理呼び出し
        Instantiate(uic.damef, uic.damefPos.position, Quaternion.Euler(uic.damefRot.eulerAngles), uic.damefPos); //ダメージエフェクト出現(Canvas2の子オブジェクトにする)
        camShake.ShakeOn(); //画面振動演出

        if(hpNow <= 0f)
        {
            uic.rScreenUI.SetActive(true); //レッドスクリーン演出開始
            noShot = true; //射撃不可
        }
    }

    /// <summary>
    /// 強制死亡
    /// </summary>
    public void DeadForce()
    {
        hpNow = 0; //HPを強制で0に
        uihp.HPDownAll(); //UI_HPの減少処理呼び出し(強制0)
        camShake.ShakeOn(); //画面振動演出
        uic.rScreenUI.SetActive(true); //レッドスクリーン演出開始
        noShot = true; //射撃不可
    }


    /// <summary>
    /// 攻撃力変更
    /// </summary>
    /// <param name="a"></param>
    public void PAtkChange(float pa)
    {
        pAtk = pa;
    }

    /// <summary>
    /// 敵攻撃力決定
    /// </summary>
    /// <param name="ea"></param>
    public void EAtkCange(float ea)
    {
        if(!isDam)
        {
            eAtk = ea;
            isDam = true; //ダメージ処理中に追加で接触してもダメージ量が変わらないようにする
        }
        
    }

    /// <summary>
    /// 敵の攻撃力
    /// </summary>
    /// <returns></returns>
    public float EAtk()
    {
        return eAtk;
    }

    /// <summary>
    /// スコア管理
    /// </summary>
    public void ScoreCount(float enScore)
    {
        score += enScore * scoreBoost; //基礎スコア×残り時間によるブースト率、基礎スコアは敵ごとに設定され、enScore引数で取得
        destCount++; //撃破カウント加算
        timeAd++; //時間回復値加算
    }

    /// <summary>
    /// マップ中心座標
    /// </summary>
    /// <returns></returns>
    public Vector3 MapCenter()
    {
        return mapCenter.position;
    }


    //アイテムの獲得処理
    //プレイヤーのアイテムチェックがアイテムに接触
    //アイテムチェックのIsItem()がトゥルーに
    //プレイヤーのUpdateでIsItem()を判定し、トゥルーならGマネージャーを呼び出す
    //プレイヤーのUpdateの判定時は一つずつ取得処理を行うようにする
    //Gマネージャーを呼び出しアイテム獲得時のメソッドを処理
    //Gマネージャーがアイテムの情報を取得
    //この際ゲームオブジェクト自体をGameObject変数で取得する(最後にDestroyで消すため)
    //取得したアイテムのパラメータをGマネージャーで管理しているステータスに加える
    //処理が完了したらDestroyでアイテムを消す
    //
    //※(変数) = other.gameObjectでコリジョンしたオブジェクトを取得？
}
